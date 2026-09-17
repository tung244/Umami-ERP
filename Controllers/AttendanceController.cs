using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 10;

        // GET: Attendance
        public async Task<IActionResult> Index(int? pageNumber, string searchString, DateTime? startDate, DateTime? endDate)
        {
            ViewBag.SearchString = searchString;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            var attendancesQuery = _context.Attendances
                .Include(a => a.Staff)
                .Include(a => a.Shift)
                .Include(a => a.ApprovedByStaff)
                .AsQueryable();

            // Áp dụng bộ lọc tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                attendancesQuery = attendancesQuery.Where(a =>
                    a.Staff.FirstName.Contains(searchString) ||
                    a.Staff.LastName.Contains(searchString) ||
                    a.Staff.EmployeeNumber.Contains(searchString));
            }

            // Áp dụng bộ lọc ngày
            if (startDate.HasValue)
            {
                attendancesQuery = attendancesQuery.Where(a => a.ClockInAt.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                attendancesQuery = attendancesQuery.Where(a => a.ClockInAt.Date <= endDate.Value.Date);
            }

            // Sắp xếp theo thời gian check-in mới nhất
            attendancesQuery = attendancesQuery.OrderByDescending(a => a.ClockInAt);

            var page = pageNumber ?? 1;
            var totalItems = await attendancesQuery.CountAsync();

            // Execute query và lấy strongly-typed list
            var attendances = await attendancesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.Items = attendances;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.Title = "Danh sách chấm công";

            return View();
        }

        // GET: Attendance/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Staff)
                .Include(a => a.Shift)
                .Include(a => a.ApprovedByStaff)
                .FirstOrDefaultAsync(m => m.AttendanceId == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendance/Create
        public IActionResult Create()
        {
            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            ViewBag.ShiftList = _context.Shifts.ToList();
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,ClockInAt,ClockOutAt,ClockType,Location,ShiftId,Notes")] Attendance attendance)
        {
            // Xóa validation cho các field tự động
            ModelState.Remove("AttendanceId");
            ModelState.Remove("Status");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");

            if (ModelState.IsValid)
            {
                attendance.AttendanceId = Guid.NewGuid();
                attendance.Status = AttendanceStatus.PendingApproval;
                attendance.CreatedAt = DateTime.Now;

                _context.Add(attendance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm bản ghi chấm công thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            ViewBag.ShiftList = _context.Shifts.ToList();
            return View(attendance);
        }

        // GET: Attendance/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            ViewBag.ShiftList = _context.Shifts.ToList();
            return View(attendance);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AttendanceId,StaffId,ClockInAt,ClockOutAt,ClockType,Location,ShiftId,Notes,Status")] Attendance attendance)
        {
            if (id != attendance.AttendanceId)
            {
                return NotFound();
            }
    
            // Loại bỏ validation cho các field tự động nếu cần
            ModelState.Remove("UpdatedAt");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                var existing = await _context.Attendances.FindAsync(id);
                if (existing == null)
                    return NotFound();

                // Cập nhật các trường được phép chỉnh
                existing.StaffId = attendance.StaffId;
                existing.ShiftId = attendance.ShiftId;
                existing.Location = attendance.Location;
                existing.Notes = attendance.Notes;
                existing.Status = attendance.Status;
                existing.ClockType = attendance.ClockType;

                // Hàm rút giây (chỉ giữ đến phút)
                DateTime TruncateToMinute(DateTime dt) =>
                    new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Kind);

                existing.ClockInAt = TruncateToMinute(attendance.ClockInAt);
                existing.ClockOutAt = attendance.ClockOutAt.HasValue ? TruncateToMinute(attendance.ClockOutAt.Value) : null;

                if(existing.ClockInAt >= existing.ClockOutAt)
                {
                    TempData["ErrorMessage"] = "Thời gian out phải hơn thời gian in";
                    ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
                    ViewBag.ShiftList = _context.Shifts.ToList();
                    return View(attendance);
                }

                existing.UpdatedAt = DateTime.Now;

                _context.Update(existing);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật chấm công thành công.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            ViewBag.ShiftList = _context.Shifts.ToList();
            return View(attendance);
        }
        // GET: Attendance/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Staff)
                .Include(a => a.Shift)
                .FirstOrDefaultAsync(m => m.AttendanceId == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa bản ghi chấm công thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendance/ClockIn
        public IActionResult ClockIn()
        {
            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            ViewBag.ShiftList = _context.Shifts.ToList();
            return View();
        }

        // POST: Attendance/ClockIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockIn(Guid staffId, Guid? shiftId, string location, string notes)
        {
            var attendance = new Attendance
            {
                AttendanceId = Guid.NewGuid(),
                StaffId = staffId,
                ClockInAt = DateTime.Now,
                ClockType = ClockType.Manual,
                Location = location,
                ShiftId = shiftId,
                Status = AttendanceStatus.Approved, // Tự động duyệt cho check-in
                Notes = notes,
                CreatedAt = DateTime.Now
            };

            _context.Add(attendance);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã check-in thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Attendance/ClockOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockOut(Guid id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null && !attendance.ClockOutAt.HasValue)
            {


                var now = DateTime.Now;
                var truncated = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Local);
                attendance.ClockOutAt = truncated;
                attendance.ClockType = ClockType.Manual;
                attendance.Status = AttendanceStatus.Approved; // Tự động duyệt cho check-out
                attendance.UpdatedAt = DateTime.Now;

                _context.Update(attendance);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đã check-out thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Attendance/Report
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate, Guid? staffId)
        {
            var start = startDate ?? DateTime.Now.AddDays(-30);
            var end = endDate ?? DateTime.Now;
            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            ViewBag.StaffId = staffId;
            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();

            var attendances = _context.Attendances
                .Include(a => a.Staff)
                .Include(a => a.Shift)
                .Where(a => a.ClockInAt.Date >= start.Date &&
                           a.ClockInAt.Date <= end.Date)
                .AsQueryable();

            if (staffId.HasValue)
            {
                attendances = attendances.Where(a => a.StaffId == staffId.Value);
            }

            var reportData = await attendances
                .ToListAsync(); // Execute query first

            var groupedData = reportData
                .GroupBy(a => a.StaffId)
                .Select(g => new AttendanceReportViewModel
                {
                    StaffId = g.Key,
                    StaffName = (g.First().Staff?.FirstName ?? "") + " " + (g.First().Staff?.LastName ?? ""),
                    TotalDays = g.Count(),
                    TotalHours = g.Where(a => a.ClockOutAt.HasValue).Sum(a => a.ClockOutAt.HasValue ? (decimal)(a.ClockOutAt.Value - a.ClockInAt).TotalHours : 0),
                    AverageHours = g.Where(a => a.ClockOutAt.HasValue).Any() ?
                        g.Where(a => a.ClockOutAt.HasValue).Average(a => a.ClockOutAt.HasValue ? (decimal)(a.ClockOutAt.Value - a.ClockInAt).TotalHours : 0) : 0
                })
                .ToList();

            return View(groupedData);
        }

        // GET: Attendance/DailyAttendance
        // Báo cáo điểm danh cho một ngày cụ thể (mặc định: hôm nay)
        [HttpGet]
        public async Task<IActionResult> DailyAttendance(DateTime? date)
        {
            var targetDate = (date ?? DateTime.Now).Date;
            ViewBag.Date = targetDate;
            ViewBag.Title = "Báo cáo điểm danh hàng ngày";
            // Danh sách nhân viên để có thể lọc/hiển thị
            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();

            // Lấy các bản ghi chấm công của ngày đích
            var attendancesQuery = _context.Attendances
                .Include(a => a.Staff)
                .Include(a => a.Shift)
                .Where(a => a.ClockInAt.Date == targetDate)
                .AsQueryable();

            var items = await attendancesQuery
                .OrderBy(a => a.ClockInAt)
                .Select(a => new AttendanceDailyViewModel
                {
                    AttendanceId = a.AttendanceId,
                    StaffId = a.StaffId,
                    StaffName = (a.Staff.FirstName ?? "") + " " + (a.Staff.LastName ?? ""),
                    ClockInAt = a.ClockInAt,
                    ClockOutAt = a.ClockOutAt,
                    ShiftName = a.Shift != null ? a.Shift.Name : string.Empty,
                    Status = a.Status,
                    TotalHours = a.ClockOutAt.HasValue ? (decimal?)(a.ClockOutAt.Value - a.ClockInAt).TotalHours : null
                })
                .ToListAsync();

            return View(items);
        }

        private bool AttendanceExists(Guid id)
        {
            return _context.Attendances.Any(e => e.AttendanceId == id);
        }
    }

    public class AttendanceReportViewModel
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public decimal TotalHours { get; set; }
        public decimal AverageHours { get; set; }
    }

    // ViewModel cho báo cáo điểm danh theo ngày
    public class AttendanceDailyViewModel
    {
        public Guid AttendanceId { get; set; }
        public Guid StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public DateTime ClockInAt { get; set; }
        public DateTime? ClockOutAt { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public AttendanceStatus Status { get; set; }
        // Tổng số giờ (nếu có ClockOut)
        public decimal? TotalHours { get; set; }
    }
}