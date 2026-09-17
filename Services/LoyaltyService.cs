using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Services
{
    /// <summary>
    /// Service xử lý logic chương trình khách hàng thân thiết
    /// </summary>
    public class LoyaltyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportUpdateService? _reportUpdateService;

        public LoyaltyService(ApplicationDbContext context, IReportUpdateService? reportUpdateService = null)
        {
            _context = context;
            _reportUpdateService = reportUpdateService;
        }

        /// <summary>
        /// Xử lý tích điểm khi thanh toán thành công
        /// - Tính điểm dựa trên cấu hình: giá trị >= 50000 và cứ 10000 = 1 điểm
        /// - Cộng vào TotalEarnedPoints (không cộng PointsBalance vì điểm có thể tiêu)
        /// - Cập nhật tier dựa trên TotalEarnedPoints
        /// </summary>
        public async Task ProcessPaymentLoyaltyAsync(Guid orderId, decimal paidAmount)
        {
            try
            {
                // Lấy đơn hàng
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null || !order.CustomerId.HasValue)
                    return;

                // Kiểm tra cấu hình tích điểm
                var minAmountSetting = await _context.LoyaltySettings
                    .FirstOrDefaultAsync(s => s.SettingKey == "MinPointsAmount" && s.IsActive);

                var pointsPerAmountSetting = await _context.LoyaltySettings
                    .FirstOrDefaultAsync(s => s.SettingKey == "PointsPerAmount" && s.IsActive);

                // Giá trị mặc định: >= 50000 và cứ 10000 = 1 điểm
                decimal minAmount = 50000;
                decimal pointsPerAmount = 10000;

                if (minAmountSetting != null && decimal.TryParse(minAmountSetting.SettingValue, out var minVal))
                    minAmount = minVal;

                if (pointsPerAmountSetting != null && decimal.TryParse(pointsPerAmountSetting.SettingValue, out var pointsVal))
                    pointsPerAmount = pointsVal;

                // Nếu số tiền < giá trị tối thiểu, không tích điểm
                if (paidAmount < minAmount)
                    return;

                // Tính điểm: cứ 10000 = 1 điểm (làm tròn xuống)
                decimal earnedPoints = Math.Floor(paidAmount / pointsPerAmount);

                if (earnedPoints <= 0)
                    return;

                // Lấy hoặc tạo LoyaltyAccount
                var loyaltyAccount = await _context.LoyaltyAccounts
                    .FirstOrDefaultAsync(la => la.CustomerId == order.CustomerId.Value);

                if (loyaltyAccount == null)
                {
                    // Tạo tài khoản loyalty mới
                    loyaltyAccount = new LoyaltyAccount
                    {
                        LoyaltyAccountId = Guid.NewGuid(),
                        CustomerId = order.CustomerId.Value,
                        PointsBalance = 0,
                        Tier = LoyaltyTier.None,
                        TierEffectiveFrom = DateTime.Now,
                        TotalEarnedPoints = 0,
                        TotalRedeemedPoints = 0,
                        CreatedAt = DateTime.Now
                    };
                    _context.LoyaltyAccounts.Add(loyaltyAccount);
                }

                // Cộng điểm vào TotalEarnedPoints (điểm tích lũy tổng cộng)
                loyaltyAccount.TotalEarnedPoints += earnedPoints;
                loyaltyAccount.PointsBalance += earnedPoints; // Cộng vào balance hiện tại
                loyaltyAccount.UpdatedAt = DateTime.Now;

                // Tạo transaction ghi nhận
                var transaction = new LoyaltyTransaction
                {
                    LoyaltyTransactionId = Guid.NewGuid(),
                    LoyaltyAccountId = loyaltyAccount.LoyaltyAccountId,
                    OrderId = orderId,
                    Type = LoyaltyTransactionType.Earn,
                    Points = earnedPoints,
                    Reason = $"Thanh toán đơn hàng {order.OrderNumber} - {paidAmount:N0} VND",
                    CreatedAt = DateTime.Now
                };
                _context.LoyaltyTransactions.Add(transaction);

                // Cập nhật tier dựa trên TotalEarnedPoints
                await UpdateCustomerTierAsync(loyaltyAccount);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"[LoyaltyService] Error processing payment loyalty: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật tier cho khách hàng dựa trên TotalEarnedPoints
        /// </summary>
        private async Task UpdateCustomerTierAsync(LoyaltyAccount loyaltyAccount)
        {
            var tierConfigs = await _context.TierConfigs
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.MinPoints)
                .ToListAsync();

            if (!tierConfigs.Any())
                return;

            var totalPoints = loyaltyAccount.TotalEarnedPoints;

            // Tìm tier cao nhất (MinPoints cao nhất) mà khách đủ điều kiện
            var matchingTier = tierConfigs
                .FirstOrDefault(t =>
                    totalPoints >= t.MinPoints &&
                    (!t.MaxPoints.HasValue || totalPoints <= t.MaxPoints.Value));

            var newTier = matchingTier?.Tier ?? LoyaltyTier.None;

            // Nếu tier thay đổi, cập nhật
            if (loyaltyAccount.Tier != newTier)
            {
                var oldTier = loyaltyAccount.Tier;
                loyaltyAccount.Tier = newTier;
                loyaltyAccount.TierEffectiveFrom = DateTime.Now;
                loyaltyAccount.UpdatedAt = DateTime.Now;
                
                // Thông báo cập nhật customer report khi tier thay đổi
                if (_reportUpdateService != null)
                {
                    await _reportUpdateService.NotifyCustomerUpdate($"Khách hàng lên hạng từ {oldTier} sang {newTier}");
                }
            }
        }

        /// <summary>
        /// Lấy thông tin loyalty account của khách hàng
        /// </summary>
        public async Task<LoyaltyAccount?> GetLoyaltyAccountAsync(Guid customerId)
        {
            return await _context.LoyaltyAccounts
                .FirstOrDefaultAsync(la => la.CustomerId == customerId);
        }

        /// <summary>
        /// Tính toán giảm giá tự động từ quyền lợi của tier hiện tại
        /// Chỉ áp dụng các quyền lợi loại "Discount"
        /// </summary>
        public async Task<decimal> CalculateAutoDiscountAsync(Guid customerId, decimal orderAmount)
        {
            try
            {
                // Lấy loyalty account của khách hàng
                var loyaltyAccount = await _context.LoyaltyAccounts
                    .FirstOrDefaultAsync(la => la.CustomerId == customerId);

                if (loyaltyAccount == null)
                    return 0m;

                // Lấy tier config hiện tại
                var tierConfig = await _context.TierConfigs
                    .FirstOrDefaultAsync(t => t.Tier == loyaltyAccount.Tier && t.IsActive);

                if (tierConfig == null)
                    return 0m;

                // Lấy tất cả quyền lợi loại "Discount" của tier này
                var discountBenefits = await _context.TierBenefits
                    .Include(tb => tb.Benefit)
                    .Where(tb => tb.TierConfigId == tierConfig.TierConfigId &&
                                 tb.IsActive &&
                                 tb.Benefit != null &&
                                 tb.Benefit.IsActive &&
                                 tb.Benefit.BenefitType.ToLower() == "discount")
                    .ToListAsync();

                if (!discountBenefits.Any())
                    return 0m;

                decimal totalDiscount = 0m;

                // Tính tổng giảm giá từ tất cả quyền lợi discount
                foreach (var tierBenefit in discountBenefits)
                {
                    var benefit = tierBenefit.Benefit;
                    if (benefit == null)
                        continue;

                    decimal discountValue = 0m;

                    // Nếu có CustomValue trong TierBenefit, sử dụng nó
                    if (!string.IsNullOrEmpty(tierBenefit.CustomValue) && 
                        decimal.TryParse(tierBenefit.CustomValue, out var customVal))
                    {
                        discountValue = customVal;
                    }
                    // Nếu là giảm giá theo phần trăm
                    else if (benefit.DiscountPercent.HasValue && benefit.DiscountPercent.Value > 0)
                    {
                        discountValue = (orderAmount * benefit.DiscountPercent.Value) / 100;
                    }
                    // Nếu là giảm giá cố định
                    else if (benefit.DiscountAmount.HasValue && benefit.DiscountAmount.Value > 0)
                    {
                        discountValue = benefit.DiscountAmount.Value;
                    }

                    totalDiscount += discountValue;
                }

                // Đảm bảo giảm giá không vượt quá số tiền đơn hàng
                return Math.Min(totalDiscount, orderAmount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoyaltyService] Error calculating auto discount: {ex.Message}");
                return 0m;
            }
        }
    }
}
