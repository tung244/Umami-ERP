using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Services;

/// <summary>
/// Background service theo dõi tồn kho và gửi thông báo khi dưới ngưỡng
/// </summary>
public class StockAlertBackgroundService : BackgroundService
{
    private readonly ILogger<StockAlertBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private Timer? _timer;

    public StockAlertBackgroundService(
        ILogger<StockAlertBackgroundService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StockAlertBackgroundService đang khởi động...");

        // Chạy lần đầu ngay khi start
        await CheckStockAlerts(null);

        // Thiết lập timer chạy mỗi 5 phút (300,000 ms)
        _timer = new Timer(
            async _ => await CheckStockAlerts(null),
            null,
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(5));

        await Task.CompletedTask;
    }

    private async Task CheckStockAlerts(object? state)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                // Lấy giá trị ngưỡng global từ config
                var globalThreshold = decimal.Parse(_configuration["StockAlert:GlobalThreshold"] ?? "10");
                var globalAlertEmails = _configuration["StockAlert:GlobalAlertEmails"]?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => e.Trim())
                    .ToList() ?? new List<string>();

                // Lấy tất cả ngưỡng cảnh báo được bật
                var thresholds = await Task.Run(() =>
                    context.StockAlertThresholds
                        .AsNoTracking()
                        .Where(t => t.IsEnabled)
                        .ToList());

                _logger.LogInformation($"Kiểm tra {thresholds.Count} sản phẩm có bật cảnh báo...");

                foreach (var threshold in thresholds)
                {
                    try
                    {
                        // Lấy item inventory
                        var item = context.InventoryItems
                            .AsNoTracking()
                            .FirstOrDefault(i => i.ItemId == threshold.ItemId);

                        if (item == null)
                        {
                            _logger.LogWarning($"Không tìm thấy item {threshold.ItemId}");
                            continue;
                        }

                        // Xác định ngưỡng để so sánh (dùng global nếu per-product = 0)
                        var minimumThreshold = threshold.MinimumThreshold > 0
                            ? threshold.MinimumThreshold
                            : globalThreshold;

                        // Kiểm tra nếu dưới ngưỡng
                        if (item.CurrentQuantity <= minimumThreshold)
                        {
                            // Xác định email người nhận
                            var recipients = threshold.AlertEmails?
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => e.Trim())
                                .ToList() ?? new List<string>();

                            // Thêm email global nếu không có email per-product
                            if (recipients.Count == 0)
                            {
                                recipients = globalAlertEmails;
                            }

                            if (recipients.Count > 0)
                            {
                                // Kiểm tra xem đã có thông báo chưa được gửi hôm nay chưa
                                var existingNotification = context.StockAlertNotifications
                                    .AsNoTracking()
                                    .Where(n => n.ItemId == threshold.ItemId
                                        && n.CreatedAt.Date == DateTime.Now.Date
                                        && n.NotificationStatus == "Sent")
                                    .FirstOrDefault();

                                if (existingNotification == null)
                                {
                                    // Tạo nội dung email
                                    var subject = $"⚠️ Cảnh báo tồn kho: {item.Name}";
                                    var body = GenerateEmailBody(item, minimumThreshold);

                                    // Gửi email
                                    var emailSuccess = await emailService.SendEmailAsync(recipients, subject, body, true);

                                    // Lưu lịch sử thông báo
                                    var notification = new StockAlertNotification
                                    {
                                        NotificationId = Guid.NewGuid(),
                                        ItemId = threshold.ItemId,
                                        CurrentQuantity = item.CurrentQuantity,
                                        ThresholdQuantity = minimumThreshold,
                                        RecipientEmails = string.Join(", ", recipients),
                                        NotificationStatus = emailSuccess ? "Sent" : "Failed",
                                        ErrorMessage = emailSuccess ? null : "Lỗi gửi email",
                                        SentAt = emailSuccess ? DateTime.Now : null,
                                        CreatedAt = DateTime.Now,
                                        UpdatedAt = DateTime.Now
                                    };

                                    context.StockAlertNotifications.Add(notification);
                                    await context.SaveChangesAsync();

                                    if (emailSuccess)
                                    {
                                        _logger.LogInformation($"Gửi cảnh báo cho {item.Name} (SL: {item.CurrentQuantity}, ngưỡng: {minimumThreshold})");
                                    }
                                    else
                                    {
                                        _logger.LogError($"Lỗi gửi cảnh báo cho {item.Name}");
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"Không có email người nhận cho {item.Name}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Lỗi khi kiểm tra threshold {threshold.ThresholdId}: {ex.Message}");
                    }
                }

                _logger.LogInformation("Hoàn thành kiểm tra cảnh báo tồn kho");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi trong StockAlertBackgroundService: {ex.Message}");
        }
    }

    private string GenerateEmailBody(InventoryItem item, decimal threshold)
    {
        var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ background-color: #f5f5f5; padding: 20px; }}
        .content {{ background-color: white; padding: 20px; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }}
        .alert-title {{ color: #d9534f; font-size: 24px; font-weight: bold; margin-bottom: 15px; }}
        .info-box {{ background-color: #f0f0f0; padding: 15px; border-left: 4px solid #d9534f; margin: 10px 0; }}
        .label {{ font-weight: bold; color: #555; }}
        .value {{ color: #333; margin-left: 10px; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #999; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='content'>
            <div class='alert-title'>⚠️ CẢNH BÁO TỒN KHO</div>
            
            <div class='info-box'>
                <div><span class='label'>Sản phẩm:</span><span class='value'>{item.Name}</span></div>
                <div><span class='label'>SKU:</span><span class='value'>{item.Sku}</span></div>
                <div><span class='label'>Số lượng hiện tại:</span><span class='value'>{item.CurrentQuantity} {item.Unit}</span></div>
                <div><span class='label'>Ngưỡng cảnh báo:</span><span class='value'>{threshold} {item.Unit}</span></div>
                <div><span class='label'>Thời gian:</span><span class='value'>{DateTime.Now:dd/MM/yyyy HH:mm:ss}</span></div>
            </div>

            <p>Sản phẩm <strong>{item.Name}</strong> đã dưới ngưỡng cảnh báo tồn kho. Vui lòng kiểm tra và bổ sung hàng.</p>

            <div class='footer'>
                <p>Email này được gửi tự động từ hệ thống quản lý nhà hàng. Vui lòng không reply trực tiếp vào email này.</p>
            </div>
        </div>
    </div>
</body>
</html>";
        return html;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StockAlertBackgroundService đang dừng...");
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
