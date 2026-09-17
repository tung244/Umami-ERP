using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý vai trò và quyền truy cập
    /// </summary>
    public class RolesAndAccessController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public RolesAndAccessController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách vai trò
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            var query = from r in _context.Roles.AsNoTracking()
                        select r;

            if (!string.IsNullOrEmpty(search))
            {
                query = from r in query
                        where r.Name.Contains(search) || (r.Description != null && r.Description.Contains(search))
                        select r;
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderBy(r => r.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.Title = "Quản lý vai trò";
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết vai trò và quyền
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .ThenInclude(rp => rp.Permission)
                .Include(r => r.StaffMembers)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
                return NotFound();

            ViewBag.Item = role;
            ViewBag.Title = $"Chi tiết vai trò - {role.Name}";
            return View();
        }

        /// <summary>
        /// Hiển thị form tạo vai trò mới
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var permissions = await _context.Permissions
                .OrderBy(p => p.Name)
                .ToListAsync();

            ViewBag.Permissions = permissions;
            ViewBag.Title = "Tạo vai trò mới";
            return View();
        }

        /// <summary>
        /// Xử lý tạo vai trò mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role model, Guid[] selectedPermissions)
        {
            ModelState.Remove("RoleId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");

            if (ModelState.IsValid)
            {
                model.RoleId = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;

                _context.Roles.Add(model);

                // Thêm quyền cho vai trò
                if (selectedPermissions != null && selectedPermissions.Length > 0)
                {
                    foreach (var permissionId in selectedPermissions)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = model.RoleId,
                            PermissionId = permissionId
                        };
                        _context.RolePermissions.Add(rolePermission);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo vai trò thành công!";
                return RedirectToAction("Index");
            }

            var permissions = await _context.Permissions.OrderBy(p => p.Name).ToListAsync();
            ViewBag.Permissions = permissions;
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa vai trò
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
                return NotFound();

            var permissions = await _context.Permissions.OrderBy(p => p.Name).ToListAsync();
            var selectedPermissionIds = role.Permissions.Select(rp => rp.PermissionId).ToArray();

            ViewBag.Permissions = permissions;
            ViewBag.SelectedPermissionIds = selectedPermissionIds;
            ViewBag.Title = $"Chỉnh sửa vai trò - {role.Name}";
            return View(role);
        }

        /// <summary>
        /// Xử lý chỉnh sửa vai trò
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Role model, Guid[] selectedPermissions)
        {
            if (id != model.RoleId)
                return NotFound();

            ModelState.Remove("UpdatedAt");

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;

                _context.Update(model);

                // Xóa quyền cũ
                var existingPermissions = _context.RolePermissions.Where(rp => rp.RoleId == id);
                _context.RolePermissions.RemoveRange(existingPermissions);

                // Thêm quyền mới
                if (selectedPermissions != null && selectedPermissions.Length > 0)
                {
                    foreach (var permissionId in selectedPermissions)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = model.RoleId,
                            PermissionId = permissionId
                        };
                        _context.RolePermissions.Add(rolePermission);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật vai trò thành công!";
                return RedirectToAction("Index");
            }

            var permissions = await _context.Permissions.OrderBy(p => p.Name).ToListAsync();
            ViewBag.Permissions = permissions;
            return View(model);
        }

        /// <summary>
        /// Xóa vai trò
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _context.Roles
                .Include(r => r.StaffMembers)
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
                return NotFound();

            // Kiểm tra xem vai trò có đang được sử dụng không
            if (role.StaffMembers.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa vai trò đang được gán cho nhân viên!";
                return RedirectToAction("Index");
            }

            // Xóa quyền của vai trò
            var rolePermissions = _context.RolePermissions.Where(rp => rp.RoleId == id);
            _context.RolePermissions.RemoveRange(rolePermissions);

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa vai trò thành công!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Hiển thị danh sách quyền
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Permissions(string? search, int page = 1)
        {
            var query = from p in _context.Permissions.AsNoTracking()
                        select p;

            if (!string.IsNullOrEmpty(search))
            {
                query = from p in query
                        where p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search))
                        select p;
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.Title = "Quản lý quyền truy cập";
            return View();
        }

        /// <summary>
        /// Hiển thị form tạo quyền mới
        /// </summary>
        [HttpGet]
        public IActionResult CreatePermission()
        {
            ViewBag.Title = "Tạo quyền mới";
            return View();
        }

        /// <summary>
        /// Xử lý tạo quyền mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePermission(Permission model)
        {
            ModelState.Remove("PermissionId");

            if (ModelState.IsValid)
            {
                model.PermissionId = Guid.NewGuid();
                _context.Permissions.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Tạo quyền thành công!";
                return RedirectToAction("Permissions");
            }

            return View(model);
        }

        /// <summary>
        /// Gán quyền cho nhân viên cụ thể
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignStaffPermissions(Guid staffId)
        {
            var staff = await _context.Staff
                .Include(s => s.Role)
                .Include(s => s.Permissions)
                .ThenInclude(sp => sp.Permission)
                .FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
                return NotFound();

            var allPermissions = await _context.Permissions.OrderBy(p => p.Name).ToListAsync();
            var staffPermissionIds = staff.Permissions.Select(sp => sp.PermissionId).ToArray();

            ViewBag.Staff = staff;
            ViewBag.AllPermissions = allPermissions;
            ViewBag.StaffPermissionIds = staffPermissionIds;
            ViewBag.Title = $"Gán quyền cho {staff.FullName}";
            return View();
        }

        /// <summary>
        /// Xử lý gán quyền cho nhân viên
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignStaffPermissions(Guid staffId, Guid[] selectedPermissions)
        {
            var staff = await _context.Staff
                .Include(s => s.Permissions)
                .FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
                return NotFound();

            // Xóa quyền cũ của nhân viên
            var existingPermissions = _context.StaffPermissions.Where(sp => sp.StaffId == staffId);
            _context.StaffPermissions.RemoveRange(existingPermissions);

            // Thêm quyền mới
            if (selectedPermissions != null && selectedPermissions.Length > 0)
            {
                foreach (var permissionId in selectedPermissions)
                {
                    var staffPermission = new StaffPermission
                    {
                        StaffId = staffId,
                        PermissionId = permissionId
                    };
                    _context.StaffPermissions.Add(staffPermission);
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật quyền cho nhân viên thành công!";
            return RedirectToAction("AssignStaffPermissions", new { staffId });
        }
    }
}
