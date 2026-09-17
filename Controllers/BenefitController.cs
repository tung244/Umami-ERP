using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý quyền lợi (benefits)
    /// </summary>
    public class BenefitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BenefitController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách quyền lợi
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var benefits = await _context.Benefits
                .Include(b => b.TierBenefits)
                    .ThenInclude(tb => tb.TierConfig)
                .OrderBy(b => b.BenefitName)
                .ToListAsync();

            ViewBag.Title = "Quản lý quyền lợi";
            return View(benefits);
        }

        /// <summary>
        /// Hiển thị form tạo quyền lợi mới
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm quyền lợi";
            return View();
        }

        /// <summary>
        /// Xử lý tạo quyền lợi mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Benefit model)
        {
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                model.BenefitId = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;

                await _context.Benefits.AddAsync(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm quyền lợi thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Thêm quyền lợi";
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa quyền lợi
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null)
                return NotFound();

            ViewBag.Title = $"Chỉnh sửa - {benefit.BenefitName}";
            return View(benefit);
        }

        /// <summary>
        /// Xử lý chỉnh sửa quyền lợi
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Benefit model)
        {
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _context.Benefits.Update(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật quyền lợi thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Title = $"Chỉnh sửa - {model.BenefitName}";
            return View(model);
        }

        /// <summary>
        /// Xóa quyền lợi (hard delete)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null)
                return NotFound();

            // Xóa tất cả TierBenefit liên quan
            var tierBenefits = await _context.TierBenefits
                .Where(tb => tb.BenefitId == id)
                .ToListAsync();
            _context.TierBenefits.RemoveRange(tierBenefits);

            // Xóa quyền lợi
            _context.Benefits.Remove(benefit);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã xóa quyền lợi!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Hiển thị form gán quyền lợi cho hạng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignToTier(Guid id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null)
                return NotFound();

            var tiers = await _context.TierConfigs
                .Where(t => t.IsActive)
                .OrderBy(t => t.DisplayOrder)
                .ToListAsync();

            var assignedTierIds = await _context.TierBenefits
                .Where(tb => tb.BenefitId == id && tb.IsActive)
                .Select(tb => tb.TierConfigId)
                .ToListAsync();

            ViewBag.Benefit = benefit;
            ViewBag.Tiers = tiers;
            ViewBag.AssignedTierIds = assignedTierIds;
            ViewBag.Title = $"Gán quyền lợi - {benefit.BenefitName}";
            return View();
        }

        /// <summary>
        /// Xử lý gán quyền lợi cho hạng
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToTier(Guid id, List<Guid> selectedTierIds)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null)
                return NotFound();

            // Lấy danh sách gán hiện tại
            var existingAssignments = await _context.TierBenefits
                .Where(tb => tb.BenefitId == id)
                .ToListAsync();

            // Xóa các gán không còn được chọn
            var toRemove = existingAssignments
                .Where(tb => selectedTierIds == null || !selectedTierIds.Contains(tb.TierConfigId))
                .ToList();
            _context.TierBenefits.RemoveRange(toRemove);

            // Thêm các gán mới
            if (selectedTierIds != null)
            {
                var existingTierIds = existingAssignments.Select(tb => tb.TierConfigId).ToList();
                var newTierIds = selectedTierIds.Where(tid => !existingTierIds.Contains(tid)).ToList();

                foreach (var tierId in newTierIds)
                {
                    var tierBenefit = new TierBenefit
                    {
                        TierBenefitId = Guid.NewGuid(),
                        TierConfigId = tierId,
                        BenefitId = id,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                    await _context.TierBenefits.AddAsync(tierBenefit);
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã cập nhật gán quyền lợi cho các hạng!";
            return RedirectToAction("Index");
        }
    }
}
