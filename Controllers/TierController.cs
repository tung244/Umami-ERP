using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý hạng thành viên
    /// </summary>
    public class TierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TierController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách hạng thành viên
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tiers = await _context.TierConfigs
                .Include(t => t.TierBenefits)
                    .ThenInclude(tb => tb.Benefit)
                .OrderBy(t => t.DisplayOrder)
                .ToListAsync();

            ViewBag.Title = "Quản lý hạng thành viên";
            return View(tiers);
        }

        /// <summary>
        /// Hiển thị form tạo mới hạng
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm hạng thành viên";
            ViewBag.AvailableTiers = GetAvailableTiers();
            return View();
        }

        /// <summary>
        /// Xử lý tạo mới hạng
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TierConfig model)
        {
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                // Kiểm tra tier đã tồn tại chưa
                var exists = await _context.TierConfigs.AnyAsync(t => t.Tier == model.Tier);
                if (exists)
                {
                    ModelState.AddModelError("Tier", "Hạng thành viên này đã tồn tại!");
                    ViewBag.Title = "Thêm hạng thành viên";
                    ViewBag.AvailableTiers = GetAvailableTiers();
                    return View(model);
                }

                model.TierConfigId = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;

                await _context.TierConfigs.AddAsync(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm hạng thành viên thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Thêm hạng thành viên";
            ViewBag.AvailableTiers = GetAvailableTiers();
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa hạng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tier = await _context.TierConfigs.FindAsync(id);
            if (tier == null)
                return NotFound();

            ViewBag.Title = $"Chỉnh sửa - {tier.TierName}";
            return View(tier);
        }

        /// <summary>
        /// Xử lý chỉnh sửa hạng
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TierConfig model)
        {
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _context.TierConfigs.Update(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật hạng thành viên thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = $"Chỉnh sửa - {model.TierName}";
            return View(model);
        }

        /// <summary>
        /// Xóa hạng (soft delete)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tier = await _context.TierConfigs.FindAsync(id);
            if (tier == null)
                return NotFound();

            tier.IsActive = false;
            tier.UpdatedAt = DateTime.Now;

            _context.TierConfigs.Update(tier);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã vô hiệu hóa hạng thành viên!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Lấy danh sách tier chưa được sử dụng
        /// </summary>
        private List<LoyaltyTier> GetAvailableTiers()
        {
            var usedTiers = _context.TierConfigs.Select(t => t.Tier).ToList();
            var allTiers = Enum.GetValues<LoyaltyTier>().ToList();
            return allTiers.Where(t => !usedTiers.Contains(t)).ToList();
        }
    }
}
