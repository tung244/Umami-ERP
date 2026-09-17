using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý chương trình khách hàng thân thiết
    /// </summary>
    public class LoyaltyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoyaltyController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cập nhật lại hạng thành viên cho tất cả tài khoản loyalty
        /// dựa trên cấu hình TierConfigs và tổng điểm tích lũy
        /// </summary>
        private async Task RecalculateCustomerTiersAsync()
        {
            var tierConfigs = await _context.TierConfigs
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.MinPoints)
                .ToListAsync();

            if (!tierConfigs.Any())
                return;

            var accounts = await _context.LoyaltyAccounts.ToListAsync();
            var now = System.DateTime.Now;

            foreach (var account in accounts)
            {
                var totalPoints = account.TotalEarnedPoints;

                // Tìm tier cao nhất (MinPoints cao nhất) mà khách đủ điều kiện
                // Ưu tiên tier có MinPoints cao nhất trong các tier phù hợp
                var matchingTier = tierConfigs
                    .FirstOrDefault(t => 
                        totalPoints >= t.MinPoints && 
                        (!t.MaxPoints.HasValue || totalPoints <= t.MaxPoints.Value));

                var newTier = matchingTier?.Tier ?? LoyaltyTier.None;

                if (account.Tier != newTier)
                {
                    account.Tier = newTier;
                    account.TierEffectiveFrom = now;
                    account.UpdatedAt = now;
                }
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Hiển thị trang giới thiệu chương trình khách hàng thân thiết
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Đảm bảo hạng thành viên của khách được tính lại theo cấu hình hiện tại
            await RecalculateCustomerTiersAsync();

            // Thống kê tổng quan
            var totalCustomers = await _context.Customers.CountAsync();
            var activeLoyaltyAccounts = await _context.LoyaltyAccounts.CountAsync();
            var totalTransactions = await _context.LoyaltyTransactions.CountAsync();
            var totalPointsEarned = await _context.LoyaltyTransactions
                .Where(t => t.Type == LoyaltyTransactionType.Earn)
                .SumAsync(t => t.Points);
            var totalPointsRedeemed = await _context.LoyaltyTransactions
                .Where(t => t.Type == LoyaltyTransactionType.Redeem)
                .SumAsync(t => t.Points);

            // Thống kê theo hạng
            var tierStats = await _context.LoyaltyAccounts
                .GroupBy(la => la.Tier)
                .Select(g => new
                {
                    Tier = g.Key,
                    Count = g.Count(),
                    TotalPoints = g.Sum(la => la.PointsBalance)
                })
                .ToListAsync();

            // Top 10 khách hàng có nhiều điểm nhất
            var topCustomers = await _context.LoyaltyAccounts
                .Include(la => la.Customer)
                .OrderByDescending(la => la.PointsBalance)
                .Take(10)
                .Select(la => new
                {
                    CustomerName = la.Customer.FullName,
                    Points = la.PointsBalance,
                    Tier = la.Tier
                })
                .ToListAsync();

            // Lấy cấu hình hạng từ database để hiển thị tên hạng
            var tierConfigs = await _context.TierConfigs
                .Where(t => t.IsActive)
                .ToDictionaryAsync(t => t.Tier, t => new { t.TierName, t.Color });

            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.ActiveLoyaltyAccounts = activeLoyaltyAccounts;
            ViewBag.TotalTransactions = totalTransactions;
            ViewBag.TotalPointsEarned = totalPointsEarned;
            ViewBag.TotalPointsRedeemed = totalPointsRedeemed;
            ViewBag.TierStats = tierStats.Cast<dynamic>().ToList();
            ViewBag.TopCustomers = topCustomers.Cast<dynamic>().ToList();
            ViewBag.TierConfigs = tierConfigs;
            ViewBag.Title = "Chương trình khách hàng thân thiết";
            return View();
        }
    }
}