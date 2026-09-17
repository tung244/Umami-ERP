using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý cấu hình chương trình tích điểm
    /// </summary>
    public class LoyaltySettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoyaltySettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách cấu hình
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _context.LoyaltySettings
                .OrderBy(s => s.SettingKey)
                .ToListAsync();

            // Đảm bảo có setting mặc định
            await EnsureDefaultSettings();

            ViewBag.Title = "Cấu hình tích điểm";
            ViewBag.Settings = settings;
            return View();
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa cấu hình
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var setting = await _context.LoyaltySettings.FindAsync(id);
            if (setting == null)
                return NotFound();

            ViewBag.Title = $"Chỉnh sửa - {setting.SettingName}";
            return View(setting);
        }

        /// <summary>
        /// Xử lý chỉnh sửa cấu hình
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LoyaltySettings model)
        {
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _context.LoyaltySettings.Update(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật cấu hình thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = $"Chỉnh sửa - {model.SettingName}";
            return View(model);
        }

        /// <summary>
        /// Tạo các setting mặc định nếu chưa có
        /// </summary>
        private async Task EnsureDefaultSettings()
        {
            var existingKeys = await _context.LoyaltySettings
                .Select(s => s.SettingKey)
                .ToListAsync();

            var defaultSettings = new List<LoyaltySettings>();

            if (!existingKeys.Contains("POINTS_PER_AMOUNT"))
            {
                defaultSettings.Add(new LoyaltySettings
                {
                    LoyaltySettingsId = Guid.NewGuid(),
                    SettingKey = "POINTS_PER_AMOUNT",
                    SettingName = "Số tiền để nhận 1 điểm",
                    Description = "Ví dụ: 15000 VND = 1 điểm",
                    SettingValue = "15000",
                    DataType = "decimal",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                });
            }

            if (!existingKeys.Contains("MIN_ORDER_FOR_POINTS"))
            {
                defaultSettings.Add(new LoyaltySettings
                {
                    LoyaltySettingsId = Guid.NewGuid(),
                    SettingKey = "MIN_ORDER_FOR_POINTS",
                    SettingName = "Đơn hàng tối thiểu để tích điểm",
                    Description = "Giá trị đơn hàng tối thiểu (VND)",
                    SettingValue = "50000",
                    DataType = "decimal",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                });
            }

            if (defaultSettings.Any())
            {
                await _context.LoyaltySettings.AddRangeAsync(defaultSettings);
                await _context.SaveChangesAsync();
            }
        }
    }
}
