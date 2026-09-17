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
    /// Controller quản lý khách hàng
    /// </summary>
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hiển thị danh sách khách hàng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,             // Tìm kiếm chung
            string? searchName,         // Tìm theo tên
            string? searchPhone,        // Tìm theo SĐT
            string? searchEmail,        // Tìm theo email
            string? sort,              // Sắp xếp
            bool? isActive,            // Lọc theo trạng thái
            Guid? branchId,            // Lọc theo chi nhánh
            LoyaltyTier? tier,         // Lọc theo hạng thành viên
            int page = 1)
        {
            var query = from c in _context.Customers
                       .Include(c => c.PreferredBranch)
                       .Include(c => c.LoyaltyAccounts)
                       .AsNoTracking()
                       select c;

            // Áp dụng bộ lọc chung
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = from c in query
                       where c.FullName.ToLower().Contains(search) ||
                             (c.Email != null && c.Email.ToLower().Contains(search)) ||
                             (c.PhoneNumber != null && c.PhoneNumber.Contains(search)) ||
                             (c.Address != null && c.Address.ToLower().Contains(search))
                       select c;
            }

            // Tìm theo tên
            if (!string.IsNullOrEmpty(searchName))
            {
                searchName = searchName.ToLower().Trim();
                query = from c in query
                       where (c.FirstName + " " + c.LastName).ToLower().Contains(searchName)
                       select c;
            }

            // Tìm theo số điện thoại
            if (!string.IsNullOrEmpty(searchPhone))
            {
                searchPhone = searchPhone.Trim();
                query = from c in query
                       where c.PhoneNumber != null && c.PhoneNumber.Contains(searchPhone)
                       select c;
            }

            // Tìm theo email
            if (!string.IsNullOrEmpty(searchEmail))
            {
                searchEmail = searchEmail.ToLower().Trim();
                query = from c in query
                       where c.Email != null && c.Email.ToLower().Contains(searchEmail)
                       select c;
            }

            // Lọc theo trạng thái
            if (isActive.HasValue)
            {
                query = from c in query
                       where c.IsActive == isActive.Value
                       select c;
            }

            // Lọc theo chi nhánh
            if (branchId.HasValue)
            {
                query = from c in query
                       where c.PreferredBranchId == branchId.Value
                       select c;
            }

            // Lọc theo hạng thành viên
            if (tier.HasValue)
            {
                query = from c in query
                       where c.LoyaltyAccounts.Any(la => la.Tier == tier.Value)
                       select c;
            }

            // Sắp xếp
            // Sắp xếp kết quả
            query = sort switch
            {
                "name" => from c in query orderby c.FullName select c,
                "nameDesc" => from c in query orderby c.FullName descending select c,
                "email" => from c in query orderby c.Email ?? "" select c, 
                "phone" => from c in query orderby c.PhoneNumber ?? "" select c,
                "branch" => from c in query orderby c.PreferredBranch != null ? c.PreferredBranch.Name : "" select c,
                "status" => from c in query orderby c.IsActive select c,
                "tier" => from c in query 
                         let highestTier = c.LoyaltyAccounts
                             .OrderByDescending(la => la.Tier)
                             .Select(la => la.Tier)
                             .FirstOrDefault()
                         orderby highestTier
                         select c,
                _ => from c in query orderby c.CreatedAt descending select c
            };

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            var branches = await (from b in _context.Branches 
                                orderby b.Name 
                                select b).ToListAsync();

            ViewBag.Title = "Quản lý khách hàng"; 
            ViewBag.Items = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.SearchName = searchName ?? "";
            ViewBag.SearchPhone = searchPhone ?? ""; 
            ViewBag.SearchEmail = searchEmail ?? "";
            ViewBag.SortOrder = sort ?? "default";
            ViewBag.IsActive = isActive;
            ViewBag.BranchId = branchId;
            ViewBag.Tier = tier;
            ViewBag.Branches = branches;
            ViewBag.LoyaltyTiers = Enum.GetValues(typeof(LoyaltyTier))
                                     .Cast<LoyaltyTier>()
                                     .Select(t => new
                                     {
                                         Id = (int)t,
                                         Name = t.ToString()
                                     })
                                     .ToList();
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết khách hàng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var customer = await _context.Customers
                .Include(c => c.PreferredBranch)
                .Include(c => c.LoyaltyAccounts)
                    .ThenInclude(la => la.Transactions.OrderByDescending(t => t.CreatedAt).Take(10))
                .Include(c => c.Orders.OrderByDescending(o => o.PlacedAt).Take(10))
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            // Lấy cấu hình hạng từ database để hiển thị tên hạng tiếng Việt
            var tierConfigs = await _context.TierConfigs
                .Where(t => t.IsActive)
                .ToDictionaryAsync(t => t.Tier, t => new { t.TierName, t.Color });

            ViewBag.Customer = customer;           // Customer
            ViewBag.TierConfigs = tierConfigs;
            ViewBag.Title = $"Chi tiết - {customer.FullName}";
            return View();
        }

        /// <summary>
        /// Hiển thị form tạo khách hàng mới
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;           // List<Branch>
            ViewBag.Title = "Thêm khách hàng mới";
            return View();
        }

        /// <summary>
        /// Xử lý tạo khách hàng mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer model, Guid? preferredBranchId)
        {
            ModelState.Remove("CustomerId");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");
            ModelState.Remove("FullName");

            if (ModelState.IsValid)
            {
                model.CustomerId = Guid.NewGuid();
                model.CreatedAt = DateTime.Now;
                model.PreferredBranchId = preferredBranchId;

                _context.Customers.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                return RedirectToAction("Index");
            }

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;
            ViewBag.Title = "Thêm khách hàng mới";
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa khách hàng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;
            ViewBag.Title = $"Chỉnh sửa - {customer.FullName}";
            return View(customer);
        }

        /// <summary>
        /// Xử lý chỉnh sửa khách hàng
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer model, Guid? preferredBranchId)
        {
            ModelState.Remove("CreatedAt");
            ModelState.Remove("FullName");

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                model.PreferredBranchId = preferredBranchId;

                _context.Customers.Update(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật khách hàng thành công!";
                return RedirectToAction("Index");
            }

            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            ViewBag.Branches = branches;
            ViewBag.Title = $"Chỉnh sửa - {model.FullName}";
            return View(model);
        }

        /// <summary>
        /// Xóa khách hàng (soft delete - chuyển trạng thái IsActive thành false)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            // Soft delete: chuyển trạng thái IsActive thành false
            customer.IsActive = false;
            customer.UpdatedAt = DateTime.Now;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã chuyển khách hàng sang trạng thái ngừng hoạt động!";
            return RedirectToAction("Index");
        }
    }
}