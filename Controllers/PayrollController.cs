using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    public class PayrollController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public PayrollController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payroll
        public async Task<IActionResult> Index(int? pageNumber, string searchString, int? month, int? year)
        {
            ViewBag.SearchString = searchString;
            ViewBag.Month = month ?? DateTime.Now.Month;
            ViewBag.Year = year ?? DateTime.Now.Year;

            var payrolls = _context.PayrollRecords
                .Include(p => p.Staff)
                .AsQueryable();

            // Áp dụng bộ lọc tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                payrolls = payrolls.Where(p =>
                    (p.Staff != null && p.Staff.FirstName.Contains(searchString)) ||
                    (p.Staff != null && p.Staff.LastName.Contains(searchString)) ||
                    (p.Staff != null && p.Staff.EmployeeNumber.Contains(searchString)));
            }

            // Áp dụng bộ lọc tháng/năm
            if (month.HasValue && year.HasValue)
            {
                payrolls = payrolls.Where(p =>
                    p.PayrollPeriodStart.Month == month.Value &&
                    p.PayrollPeriodStart.Year == year.Value);
            }

            // Sắp xếp theo kỳ lương mới nhất
            payrolls = payrolls.OrderByDescending(p => p.PayrollPeriodStart).ThenBy(p => p.Staff != null ? p.Staff.FirstName : "");

            var page = pageNumber ?? 1;
            var totalItems = await payrolls.CountAsync();
            var items = await payrolls
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.TotalEmployees = payrolls.Select(p => p.StaffId).Distinct().Count();
            ViewBag.TotalNetPay = payrolls.Sum(p => p.NetPay);
            ViewBag.TotalTax = payrolls.Sum(p => p.Taxes);
            ViewBag.TotalDeductions = payrolls.Sum(p => p.Deductions);

            var paidPayrolls = payrolls.Where(p => p.PaidAt.HasValue);
            System.Console.WriteLine($"so nguoi da tra luong:{paidPayrolls.Count()}");

            ViewBag.PaidAmount = paidPayrolls.Sum(p => p.NetPay);

            var unpaidPayrolls = payrolls.Where(p => !p.PaidAt.HasValue);
            System.Console.WriteLine($"so nguoi chua tra luong:{unpaidPayrolls.Count()}");
 ViewBag.UnpaidAmount = unpaidPayrolls.Sum(p => p.NetPay);

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.Title = "Danh sách bảng lương";

            return View();
        }

        // GET: Payroll/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollRecord = await _context.PayrollRecords
                .Include(p => p.Staff)
                .FirstOrDefaultAsync(m => m.PayrollId == id);

            if (payrollRecord == null)
            {
                return NotFound();
            }

            return View(payrollRecord);
        }

        // GET: Payroll/Create
        public IActionResult Create()
        {
            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            return View();
        }

        // POST: Payroll/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,PayrollPeriodStart,PayrollPeriodEnd,GrossPay,NetPay,Deductions,PaidAt,PaymentMethod")] PayrollRecord payrollRecord)
        {
            var staff = await _context.Staff.FindAsync(payrollRecord.StaffId);
            if (ModelState.IsValid && staff != null)
            {
                // Tính lương dựa trên loại hợp đồng
                if (staff.EmploymentType == EmploymentType.PartTime)
                {
                    var totalWorkHours = await GetTotalWorkHours(payrollRecord.StaffId, payrollRecord.PayrollPeriodStart, payrollRecord.PayrollPeriodEnd);
                    payrollRecord.GrossPay = totalWorkHours * (staff.HourlyRate ?? 0m);
                }
               
                payrollRecord.Taxes = payrollRecord.GrossPay * 0.1m;
                payrollRecord.NetPay = payrollRecord.GrossPay - payrollRecord.Deductions - payrollRecord.Taxes;
                payrollRecord.CreatedAt = DateTime.Now;
                
                _context.Add(payrollRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            return View(payrollRecord);
        }

        // GET: Payroll/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollRecord = await _context.PayrollRecords.FindAsync(id);
            if (payrollRecord == null)
            {
                return NotFound();
            }

            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            return View(payrollRecord);
        }

        // POST: Payroll/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PayrollId,StaffId,PayrollPeriodStart,PayrollPeriodEnd,GrossPay,NetPay,Deductions,Taxes,PaidAt,PaymentMethod")] PayrollRecord payrollRecord)
        {
            if (id != payrollRecord.PayrollId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    payrollRecord.UpdatedAt = DateTime.Now;
                    _context.Update(payrollRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollRecordExists(payrollRecord.PayrollId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            return View(payrollRecord);
        }

        // GET: Payroll/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollRecord = await _context.PayrollRecords
                .Include(p => p.Staff)
                .FirstOrDefaultAsync(m => m.PayrollId == id);

            if (payrollRecord == null)
            {
                return NotFound();
            }

            return View(payrollRecord);
        }

        // POST: Payroll/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var payrollRecord = await _context.PayrollRecords.FindAsync(id);
            if (payrollRecord != null)
            {
                _context.PayrollRecords.Remove(payrollRecord);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Payroll/Generate
        public IActionResult Generate(int? month, int? year)
        {
            ViewBag.Month = month ?? DateTime.Now.Month;
            ViewBag.Year = year ?? DateTime.Now.Year;
            ViewBag.StaffList = _context.Staff.Where(s => s.IsActive).ToList();
            return View();
        }

        // POST: Payroll/Generate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(int month, int year, Guid[] selectedStaffIds)
        {
            if (selectedStaffIds == null || selectedStaffIds.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một nhân viên!";
                return RedirectToAction("Generate", new { month, year });
            }

            var payPeriod = new DateTime(year, month, 1);
            var generatedCount = 0;

            foreach (var staffId in selectedStaffIds)
            {
                // Kiểm tra xem đã có bảng lương cho nhân viên này trong kỳ chưa
                var existingPayroll = await _context.PayrollRecords
                    .FirstOrDefaultAsync(p => p.StaffId == staffId &&
                                             p.PayrollPeriodStart.Month == month &&
                                             p.PayrollPeriodStart.Year == year);

                if (existingPayroll != null)
                {
                    continue; // Bỏ qua nếu đã có
                }

                var staff = await _context.Staff.FindAsync(staffId);
                if (staff == null) continue;

                var startOfMonth = new DateOnly(year, month, 1);
                var endOfMonth = new DateOnly(year, month, DateTime.DaysInMonth(year, month));

                // Tính lương dựa trên loại hợp đồng
                decimal grossPay;
                if (staff.EmploymentType == EmploymentType.PartTime)
                {
                    // Tính từ giờ làm việc thực tế
                    var totalWorkHours = await GetTotalWorkHours(staffId, startOfMonth, endOfMonth);
                    grossPay = totalWorkHours * (staff.HourlyRate ?? 0m);
                }
                else
                {
                    // FullTime: Lấy lương cơ bản
                    grossPay = staff.BaseSalary ?? 0m;
                }

                var deductions = grossPay * 0.05m; // 5% khấu trừ
                var taxes = grossPay * 0.1m; // 10% thuế
                var netPay = grossPay - deductions - taxes;

                var payrollRecord = new PayrollRecord
                {
                    PayrollId = Guid.NewGuid(),
                    StaffId = staffId,
                    PayrollPeriodStart = startOfMonth,
                    PayrollPeriodEnd = endOfMonth,
                    GrossPay = grossPay,
                    NetPay = netPay,
                    Deductions = deductions,
                    Taxes = taxes,
                    PaidAt = null,
                    PaymentMethod = PayrollPaymentMethod.BankTransfer,
                    CreatedAt = DateTime.Now
                };

                _context.PayrollRecords.Add(payrollRecord);
                generatedCount++;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã tạo bảng lương cho {generatedCount} nhân viên!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Payroll/Report
        public async Task<IActionResult> Report(int? month, int? year)
        {
            ViewBag.Month = month ?? DateTime.Now.Month;
            ViewBag.Year = year ?? DateTime.Now.Year;

            var startOfMonth = new DateTime(ViewBag.Year, ViewBag.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var payrolls = await _context.PayrollRecords
                .Include(p => p.Staff)
                .Where(p => p.PayrollPeriodStart >= DateOnly.FromDateTime(startOfMonth) &&
                           p.PayrollPeriodStart <= DateOnly.FromDateTime(endOfMonth))
                .ToListAsync();

            var report = new PayrollReportViewModel
            {
                Month = ViewBag.Month,
                Year = ViewBag.Year,
                TotalEmployees = payrolls.Select(p => p.StaffId).Distinct().Count(),
                TotalGrossPay = payrolls.Sum(p => p.GrossPay),
                TotalNetPay = payrolls.Sum(p => p.NetPay),
                TotalTaxes = payrolls.Sum(p => p.Taxes),
                TotalDeductions = payrolls.Sum(p => p.Deductions),
                PayrollDetails = payrolls
            };

            return View(report);
        }

        // POST: Payroll/MarkAsPaid/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(Guid id)
        {
            var payrollRecord = await _context.PayrollRecords.FindAsync(id);
            if (payrollRecord != null && !payrollRecord.PaidAt.HasValue)
            {
                payrollRecord.PaidAt = DateTime.Now;
                payrollRecord.UpdatedAt = DateTime.Now;

                _context.Update(payrollRecord);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Không thể cập nhật trạng thái thanh toán" });
        }

        private bool PayrollRecordExists(Guid id)
        {
            return _context.PayrollRecords.Any(e => e.PayrollId == id);
        }

        /// <summary>
        /// Tính tổng số giờ làm việc của nhân viên trong kỳ lương
        /// Lấy từ bảng Attendance: (ClockOut - ClockIn) * số ngày
        /// </summary>
        private async Task<decimal> GetTotalWorkHours(Guid staffId, DateOnly startDate, DateOnly endDate)
        {
            var attendances = await _context.Attendances
                .Where(a => a.StaffId == staffId &&
                           a.ClockInAt.Date >= startDate.ToDateTime(TimeOnly.MinValue) &&
                           a.ClockInAt.Date <= endDate.ToDateTime(TimeOnly.MaxValue) &&
                           a.ClockOutAt.HasValue)
                .ToListAsync();

            decimal totalHours = 0m;

            foreach (var attendance in attendances)
            {
                if (attendance.ClockOutAt.HasValue)
                {
                    var workDuration = attendance.ClockOutAt.Value - attendance.ClockInAt;
                    totalHours += (decimal)workDuration.TotalHours;
                }
            }

            return totalHours;
        }
    }

    public class PayrollReportViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalGrossPay { get; set; }
        public decimal TotalNetPay { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal TotalDeductions { get; set; }
        public List<PayrollRecord> PayrollDetails { get; set; } = new List<PayrollRecord>();
    }
}