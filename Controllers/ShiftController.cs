using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý ca làm việc
    /// </summary>
    public class ShiftController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public ShiftController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách ca làm việc
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            var query = from s in _context.Shifts
                        .Include(s => s.Branch)
                        .AsNoTracking()
                        select s;

            if (!string.IsNullOrEmpty(search))
            {
                query = from s in query
                        where s.Name.Contains(search) ||
                              (s.Branch != null && s.Branch.Name.Contains(search))
                        select s;
            }

            var totalItems = await query.CountAsync();
            var items = await query
            
                .OrderBy(s => s.StartTime)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.Title = "Quản lý ca làm việc";
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết ca làm việc
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var shift = await _context.Shifts
                .Include(s => s.Branch)
                .Include(s => s.Attendances)
                .ThenInclude(a => a.Staff)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ShiftId == id);

            if (shift == null)
                return NotFound();

            ViewBag.Item = shift;
            ViewBag.Title = $"Chi tiết ca làm việc - {shift.Name}";
            return View();
        }

        /// <summary>
        /// Hiển thị form tạo ca làm việc mới
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;
            ViewBag.Title = "Tạo ca làm việc mới";
            return View();
        }

        /// <summary>
        /// Xử lý tạo ca làm việc mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shift model)
        {
            ModelState.Remove("ShiftId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");

            // Kiểm tra thời gian hợp lệ
            if (model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "Thời gian kết thúc phải sau thời gian bắt đầu");
            }

            // Kiểm tra trùng tên trong cùng chi nhánh
            var existingShift = await _context.Shifts
                .AnyAsync(s => s.BranchId == model.BranchId && s.Name == model.Name);

            if (existingShift)
            {
                ModelState.AddModelError("Name", "Tên ca làm việc đã tồn tại trong chi nhánh này");
            }

            if (ModelState.IsValid)
            {
                model.ShiftId = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;

                _context.Shifts.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Tạo ca làm việc thành công!";
                return RedirectToAction("Index");
            }

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa ca làm việc
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var shift = await _context.Shifts
                .Include(s => s.Branch)
                .FirstOrDefaultAsync(s => s.ShiftId == id);

            if (shift == null)
                return NotFound();

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;
            ViewBag.Title = $"Chỉnh sửa ca làm việc - {shift.Name}";
            return View(shift);
        }

        /// <summary>
        /// Xử lý chỉnh sửa ca làm việc
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Shift model)
        {
            if (id != model.ShiftId)
                return NotFound();

            ModelState.Remove("UpdatedAt");

            // Kiểm tra thời gian hợp lệ
            if (model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "Thời gian kết thúc phải sau thời gian bắt đầu");
            }

            // Kiểm tra trùng tên trong cùng chi nhánh (trừ ca hiện tại)
            var existingShift = await _context.Shifts
                .AnyAsync(s => s.BranchId == model.BranchId && s.Name == model.Name && s.ShiftId != id);

            if (existingShift)
            {
                ModelState.AddModelError("Name", "Tên ca làm việc đã tồn tại trong chi nhánh này");
            }

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;

                _context.Update(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật ca làm việc thành công!";
                return RedirectToAction("Index");
            }

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;
            return View(model);
        }

        /// <summary>
        /// Xóa ca làm việc
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var shift = await _context.Shifts
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.ShiftId == id);

            if (shift == null)
                return NotFound();

            // Kiểm tra xem ca có đang được sử dụng không
            if (shift.Attendances.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa ca làm việc đang có chấm công!";
                return RedirectToAction("Index");
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa ca làm việc thành công!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Hiển thị form đăng ký lịch làm việc cho nhân viên
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignSchedule(Guid? staffId, DateTime? weekStart)
        {
            var weekStartDate = weekStart ?? DateTime.Now.StartOfWeek(DayOfWeek.Monday);

            var staff = await _context.Staff
                .Include(s => s.Branch)
                .Where(s => s.IsActive)
                .ToListAsync();

            var shifts = await _context.Shifts
                .Include(s => s.Branch)
                .Where(s => s.Branch != null && s.Branch.IsActive)
                .ToListAsync();

            // Lấy lịch làm việc hiện tại cho tuần
            var existingSchedules = await _context.StaffShifts
                .Include(ss => ss.Staff)
                .Include(ss => ss.Shift)
                .Where(ss => ss.WorkDate >= weekStartDate && ss.WorkDate < weekStartDate.AddDays(7))
                .ToListAsync();

            ViewBag.StaffList = staff;
            ViewBag.Shifts = shifts;
            ViewBag.WeekStart = weekStartDate;
            ViewBag.ExistingSchedules = existingSchedules;
            ViewBag.SelectedStaffId = staffId;
            ViewBag.Title = "Đăng ký lịch làm việc";

            return View();
        }

        /// <summary>
        /// Xử lý đăng ký lịch làm việc
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSchedule(Guid staffId, Guid shiftId, DateTime workDate, string action)
        {
            var staff = await _context.Staff.FindAsync(staffId);
            var shift = await _context.Shifts.FindAsync(shiftId);

            if (staff == null || shift == null)
            {
                TempData["ErrorMessage"] = "Nhân viên hoặc ca làm việc không tồn tại!";
                return RedirectToAction("AssignSchedule");
            }

            // Kiểm tra nhân viên và ca có cùng chi nhánh không
            if (staff.BranchId != shift.BranchId)
            {
                TempData["ErrorMessage"] = "Nhân viên và ca làm việc phải cùng chi nhánh!";
                return RedirectToAction("AssignSchedule");
            }

            // Kiểm tra lịch làm việc đã tồn tại chưa
            var existingSchedule = await _context.StaffShifts
                .FirstOrDefaultAsync(ss => ss.StaffId == staffId && ss.WorkDate.Date == workDate.Date);

            if (action == "assign")
            {
                if (existingSchedule != null)
                {
                    // Cập nhật ca làm việc
                    existingSchedule.ShiftId = shiftId;
                    existingSchedule.UpdatedAt = DateTime.Now;
                    _context.Update(existingSchedule);
                }
                else
                {
                    // Tạo lịch làm việc mới
                    var staffShift = new StaffShift
                    {
                        StaffId = staffId,
                        ShiftId = shiftId,
                        WorkDate = workDate.Date,
                        Status = ShiftStatus.Scheduled,
                        CreatedAt = DateTime.Now
                    };
                    _context.StaffShifts.Add(staffShift);
                }

                TempData["SuccessMessage"] = "Đăng ký lịch làm việc thành công!";
            }
            else if (action == "remove" && existingSchedule != null)
            {
                _context.StaffShifts.Remove(existingSchedule);
                TempData["SuccessMessage"] = "Xóa lịch làm việc thành công!";
            }

            await _context.SaveChangesAsync();

            var weekStart = workDate.StartOfWeek(DayOfWeek.Monday);
            return RedirectToAction("AssignSchedule", new { staffId, weekStart });
        }

        /// <summary>
        /// Phê duyệt lịch làm việc
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveSchedule(Guid scheduleId)
        {
            var schedule = await _context.StaffShifts.FindAsync(scheduleId);

            if (schedule == null)
                return NotFound();

            schedule.Status = ShiftStatus.Approved;
            schedule.ApprovedAt = DateTime.Now;
            schedule.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Phê duyệt lịch làm việc thành công!";
            return RedirectToAction("AssignSchedule");
        }

        /// <summary>
        /// Từ chối lịch làm việc
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSchedule(Guid scheduleId, string reason)
        {
            var schedule = await _context.StaffShifts.FindAsync(scheduleId);

            if (schedule == null)
                return NotFound();

            schedule.Status = ShiftStatus.Rejected;
            schedule.RejectionReason = reason;
            schedule.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Từ chối lịch làm việc thành công!";
            return RedirectToAction("AssignSchedule");
        }
    }

    // Extension method để lấy ngày đầu tuần
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}