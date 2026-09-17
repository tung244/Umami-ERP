using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QuanLiKhoHang.Services;

/// <summary>
/// Service gửi email
/// </summary>
public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task<bool> SendEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        return await SendEmailAsync(new List<string> { to }, subject, body, isHtml);
    }

    public async Task<bool> SendEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true)
    {
        try
        {
            // Lấy cấu hình SMTP từ appsettings.json
            var smtpServer = _configuration["Email:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var senderEmail = _configuration["Email:SenderEmail"];
            var senderPassword = _configuration["Email:SenderPassword"];
            var enableSsl = bool.Parse(_configuration["Email:EnableSSL"] ?? "true");

            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(senderEmail))
            {
                _logger.LogWarning("Cấu hình email chưa được thiết lập đầy đủ");
                return false;
            }

            // Tạo MailMessage
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isHtml;

                // Thêm recipients
                foreach (var recipient in recipients.Where(r => !string.IsNullOrEmpty(r)))
                {
                    mailMessage.To.Add(new MailAddress(recipient));
                }

                if (mailMessage.To.Count == 0)
                {
                    _logger.LogWarning("Không có người nhận nào");
                    return false;
                }

                // Tạo SmtpClient
                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = enableSsl;
                    smtpClient.Timeout = 10000;

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email gửi thành công tới {recipients.Count} người nhận: {subject}");
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi khi gửi email: {ex.Message}");
            return false;
        }
    }
}
