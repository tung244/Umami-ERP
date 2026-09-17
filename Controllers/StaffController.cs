using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý nhân viên
    /// </summary>
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;
        private readonly ILogger<StaffController> _logger;

        public StaffController(ApplicationDbContext context, ILogger<StaffController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Hiển thị danh sách nhân viên
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            var query = _context.Staff
                .Include(s => s.Role)
                .Include(s => s.Branch)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(search) ||
                    s.LastName.Contains(search) ||
                    s.Email.Contains(search) ||
                    s.EmployeeNumber.Contains(search) ||
                    s.PhoneNumber.Contains(search));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.Title = "Danh sách nhân viên";
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết nhân viên
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var staff = await _context.Staff
                .Include(s => s.Role)
                .Include(s => s.Branch)
                .Include(s => s.Attendances.OrderByDescending(a => a.ClockInAt).Take(10))
                .Include(s => s.PayrollRecords.OrderByDescending(p => p.PayrollPeriodEnd).Take(5))
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StaffId == id);

            if (staff == null)
                return NotFound();

            ViewBag.Item = staff;
            ViewBag.Title = $"Chi tiết nhân viên - {staff.FullName}";
            return View();
        }

        /// <summary>
        /// Hiển thị form thêm nhân viên mới
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // var roles = await _context.Roles
            //     .Where(r => r.IsActive)
            //     .OrderBy(r => r.Name)
            //     .ToListAsync();
            var roles = await _context.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Roles = roles;
            ViewBag.Branches = branches;
            ViewBag.Title = "Thêm nhân viên mới";
            return View();
        }

        /// <summary>
        /// Xử lý thêm nhân viên mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff model)
        {
            _logger.LogInformation("=== DEBUG: Bắt đầu hàm Create ===");
            _logger.LogInformation("Dữ liệu nhận được - FirstName: {FirstName}, LastName: {LastName}, RoleId: {RoleId}, BranchId: {BranchId}, Password: {HasPassword}", 
                model.FirstName, model.LastName, model.RoleId, model.BranchId, !string.IsNullOrEmpty(model.Password));

            // Xóa validation cho những field sẽ được tự động tạo
            ModelState.Remove("EmployeeNumber");
            ModelState.Remove("StaffId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("IsActive");

            try
            {
                // Gán GUID mới cho nhân viên
                model.StaffId = Guid.NewGuid();
                _logger.LogInformation("Generated StaffId: {StaffId}", model.StaffId);

                // Set giá trị mặc định nếu chưa có
                if (model.EmploymentType == 0)
                {
                    model.EmploymentType = EmploymentType.FullTime;
                    _logger.LogInformation("Set default EmploymentType: FullTime");
                }

                // Tạo mã nhân viên tự động (theo prefix dựa trên role: Manager -> MGR, Administrator -> ADM)
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == model.RoleId);
                string prefix;
                int numberWidth;

                if (role != null && role.Name != null && role.Name.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                {
                    prefix = "MGR"; // manager prefix
                    numberWidth = 3; // MGR001
                }
                else if (role != null && role.Name != null && (role.Name.Equals("Administrator", StringComparison.OrdinalIgnoreCase) || role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    prefix = "ADM"; // admin prefix
                    numberWidth = 3; // ADM001
                }
                else
                {
                    prefix = "NV"; // default prefix
                    numberWidth = 3; // NV001 (keeps previous behavior)
                }

                // Lấy nhân viên cuối cùng có cùng prefix để tăng số
                var lastEmployeeWithPrefix = await _context.Staff
                    .Where(s => !string.IsNullOrEmpty(s.EmployeeNumber) && EF.Functions.Like(s.EmployeeNumber, prefix + "%"))
                    .OrderByDescending(s => s.CreatedAt)
                    .FirstOrDefaultAsync();

                var nextNumber = 1;
                if (lastEmployeeWithPrefix != null && !string.IsNullOrEmpty(lastEmployeeWithPrefix.EmployeeNumber))
                {
                    var numberPart = lastEmployeeWithPrefix.EmployeeNumber.Substring(prefix.Length);
                    if (int.TryParse(numberPart, out int num))
                        nextNumber = num + 1;
                }

                model.EmployeeNumber = prefix + nextNumber.ToString($"D{numberWidth}");
                model.CreatedAt = DateTime.Now;
                model.IsActive = true;
                _logger.LogInformation("Generated EmployeeNumber: {EmployeeNumber}", model.EmployeeNumber);

                // Xử lý Password
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    model.Password = "123456";
                    _logger.LogWarning("Password trống, đã set mặc định: 123456");
                }

                // Check RoleId và BranchId có tồn tại không
                var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == model.RoleId);
                var branchExists = await _context.Branches.AnyAsync(b => b.BranchId == model.BranchId);

                if (!roleExists)
                {
                    ModelState.AddModelError("RoleId", "Vai trò không tồn tại");
                    _logger.LogError("RoleId không tồn tại: {RoleId}", model.RoleId);
                }

                if (!branchExists)
                {
                    ModelState.AddModelError("BranchId", "Chi nhánh không tồn tại");
                    _logger.LogError("BranchId không tồn tại: {BranchId}", model.BranchId);
                }

                // Log tất cả field để debug
                _logger.LogInformation("All fields - StaffId: {StaffId}, EmployeeNumber: {EmployeeNumber}, FirstName: {FirstName}, LastName: {LastName}, Password: {Password}, RoleId: {RoleId}, BranchId: {BranchId}, EmploymentType: {EmploymentType}, IsActive: {IsActive}, CreatedAt: {CreatedAt}",
                    model.StaffId, model.EmployeeNumber, model.FirstName, model.LastName, model.Password, model.RoleId, model.BranchId, model.EmploymentType, model.IsActive, model.CreatedAt);

                 // Kiểm tra trùng email (case-insensitive)
                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    var normalized = model.Email.Trim().ToLower();
                    var emailExists = await _context.Staff
                        .AnyAsync(s => s.Email != null && s.Email.ToLower() == normalized);
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại");
                        _logger.LogWarning("Email duplicate when creating staff: {Email}", model.Email);
                    }
                }

                // Check ModelState
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                        .Select(x => new { Field = x.Key, Errors = string.Join("; ", x.Value?.Errors.Select(e => e.ErrorMessage) ?? new List<string>()) })
                        .ToList();
                    
                    _logger.LogWarning("ModelState errors: {Errors}", string.Join(" | ", errors.Select(e => $"{e.Field}: {e.Errors}")));

                    // Load lại data cho dropdown
                    var roles = await _context.Roles.OrderBy(r => r.Name).ToListAsync();
                    var branches = await _context.Branches.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
                    ViewBag.Roles = roles;
                    ViewBag.Branches = branches;
                    return View(model);
                }

                _context.Staff.Add(model);
                _logger.LogInformation("=== DEBUG: Đã add vào context, chuẩn bị SaveChangesAsync ===");
                
                await _context.SaveChangesAsync();
                _logger.LogInformation("=== DEBUG: Đã lưu thành công vào database ===");

                TempData["SuccessMessage"] = "Thêm nhân viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error khi tạo nhân viên: {Message}, InnerException: {InnerMessage}", 
                    dbEx.Message, dbEx.InnerException?.Message);
                ModelState.AddModelError("", $"Lỗi database: {dbEx.InnerException?.Message ?? dbEx.Message}");

                // Load lại data cho dropdown
                var roles = await _context.Roles.OrderBy(r => r.Name).ToListAsync();
                var branches = await _context.Branches.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
                ViewBag.Roles = roles;
                ViewBag.Branches = branches;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo nhân viên: {Message}", ex.Message);
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo nhân viên: " + ex.Message);

                // Load lại data cho dropdown
                var roles = await _context.Roles.OrderBy(r => r.Name).ToListAsync();
                var branches = await _context.Branches.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
                ViewBag.Roles = roles;
                ViewBag.Branches = branches;
                return View(model);
            }
        }
        /// <summary>
        /// Hiển thị form chỉnh sửa nhân viên
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
                return NotFound();

            var roles = await _context.Roles
                // .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Roles = roles;
            ViewBag.Branches = branches;
            ViewBag.Title = $"Chỉnh sửa nhân viên - {staff.FullName}";
            return View(staff);
        }

        /// <summary>
        /// Xử lý cập nhật thông tin nhân viên
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Staff model)
        {
            if (id != model.StaffId)
                return NotFound();

            // Xóa validation cho những field không được edit từ form
            ModelState.Remove("EmployeeNumber");
            ModelState.Remove("CreatedAt");
         
            ModelState.Remove("Password"); // Password không edit qua form này

            try
            {
                var staff = await _context.Staff.FindAsync(id);
                var existingEmail =  _context.Staff.
                    Where(s => s.StaffId != id && s.Email != null)
                    .Select(s => s.Email.ToLower())
                    .ToList();
                if (staff == null)
                    return NotFound();
                if(!string.IsNullOrWhiteSpace(model.Email))
                {
                    var normalized = model.Email.Trim().ToLower();
                    if (existingEmail.Equals(normalized))
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại");
                    }
                }
                // Check RoleId và BranchId có tồn tại không
                var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == model.RoleId);
                var branchExists = await _context.Branches.AnyAsync(b => b.BranchId == model.BranchId);

                if (!roleExists)
                {
                    ModelState.AddModelError("RoleId", "Vai trò không tồn tại");
                }

                if (!branchExists)
                {
                    ModelState.AddModelError("BranchId", "Chi nhánh không tồn tại");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                        .Select(x => new { Field = x.Key, Errors = string.Join("; ", x.Value?.Errors.Select(e => e.ErrorMessage) ?? new List<string>()) })
                        .ToList();
                    
                    _logger.LogWarning("ModelState errors when editing staff: {Errors}", string.Join(" | ", errors.Select(e => $"{e.Field}: {e.Errors}")));

                    // Load lại data cho dropdown
                    var roles = await _context.Roles.OrderBy(r => r.Name).ToListAsync();
                    var branches = await _context.Branches.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
                    ViewBag.Roles = roles;
                    ViewBag.Branches = branches;
                    return View(model);
                }

                // Cập nhật thông tin
                staff.FirstName = model.FirstName;
                staff.LastName = model.LastName;
                staff.Email = model.Email;
                staff.PhoneNumber = model.PhoneNumber;
                staff.RoleId = model.RoleId;
                staff.BranchId = model.BranchId;
                staff.HireDate = model.HireDate;
                staff.EmploymentType = model.EmploymentType;
                staff.BaseSalary = model.BaseSalary;
                staff.HourlyRate = model.HourlyRate;
                staff.Address = model.Address;
                staff.TaxId = model.TaxId;
                staff.AvatarUrl = model.AvatarUrl;
                staff.UpdatedAt = DateTime.Now;
                staff.IsActive = model.IsActive;
              
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật nhân viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error khi cập nhật nhân viên: {Message}", dbEx.InnerException?.Message ?? dbEx.Message);
                ModelState.AddModelError("", $"Lỗi database: {dbEx.InnerException?.Message ?? dbEx.Message}");

                var roles = await _context.Roles.OrderBy(r => r.Name).ToListAsync();
                var branches = await _context.Branches.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
                ViewBag.Roles = roles;
                ViewBag.Branches = branches;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhân viên: {Message}", ex.Message);
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);

                var roles = await _context.Roles.OrderBy(r => r.Name).ToListAsync();
                var branches = await _context.Branches.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
                ViewBag.Roles = roles;
                ViewBag.Branches = branches;
                return View(model);
            }
        }

        /// <summary>
        /// Xóa nhân viên (soft delete)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
                return NotFound();

            staff.IsActive = false;
            staff.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa nhân viên thành công!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Khôi phục nhân viên
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
                return NotFound();

            staff.IsActive = true;
            staff.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Khôi phục nhân viên thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}