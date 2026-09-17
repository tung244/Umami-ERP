using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using QuanLiKhoHang.Hubs;
using QuanLiKhoHang.Models.Enums;
using System.Text.Json;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý món ăn
    /// </summary>
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DishHub> _hubContext;
        private const int PageSize = 10;

        public DishController(ApplicationDbContext context, IHubContext<DishHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Hiển thị danh sách món ăn với tìm kiếm, sắp xếp, phân trang
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? sort, Guid? categoryId, int page = 1)
        {
            var query =
                from d in _context.Dishes
                    .Include(d => d.Category)
                    .AsNoTracking()
                select d;

            if (!string.IsNullOrEmpty(search))
                query = from d in query where d.Name.Contains(search) select d;

            if (categoryId.HasValue)
                query = from d in query where d.CategoryId == categoryId select d;

            query = sort switch
            {
                "name" => from d in query orderby d.Name select d,
                "price" => from d in query orderby d.DefaultServingPrice select d,
                _ => from d in query orderby d.CreatedAt descending select d
            };

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            var categories = await _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Items = items;                 // List<Dish>
            ViewBag.Categories = categories;       // List<Category>
            ViewBag.CurrentPage = page;            // int
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.SortOrder = sort ?? "default";
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Title = "Quản lý món ăn";
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết món ăn
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dish = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.KitchenSection)
                .Include(d => d.Ingredients).ThenInclude(i => i.InventoryItem)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish == null)
                return NotFound();

            ViewBag.Item = dish;  // Dish
            ViewBag.Title = "Chi tiết món ăn";
            return View();
        }

        /// <summary>
        /// Hiển thị form tạo món ăn mới
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var kitchenSections = await _context.KitchenSections
                .OrderBy(k => k.Name)
                .ToListAsync();

            ViewBag.Categories = categories;          // List<Category>
            ViewBag.KitchenSections = kitchenSections; // List<KitchenSection>
            ViewBag.Title = "Tạo món ăn mới";
            return View();
        }

        /// <summary>
        /// Xử lý tạo món ăn mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dish model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.Dishes.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var kitchenSections = await _context.KitchenSections
                .OrderBy(k => k.Name)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.KitchenSections = kitchenSections;
            ViewBag.Title = "Tạo món ăn mới";
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa món ăn
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
                return NotFound();

            var categories = await _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var kitchenSections = await _context.KitchenSections
                .OrderBy(k => k.Name)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.KitchenSections = kitchenSections;
            ViewBag.Title = "Chỉnh sửa món ăn";
            return View(dish);
        }

        /// <summary>
        /// Xử lý chỉnh sửa món ăn
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Dish model)
        {
            if (id != model.DishId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.UpdatedAt = DateTime.Now;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(model.DishId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var kitchenSections = await _context.KitchenSections
                .OrderBy(k => k.Name)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.KitchenSections = kitchenSections;
            ViewBag.Title = "Chỉnh sửa món ăn";
            return View(model);
        }

        /// <summary>
        /// Ẩn/Hiện món ăn
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                dish.Active = !dish.Active;
                dish.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                // Send real-time update via SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveDishStatusUpdate", id, dish.Active);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Xóa món ăn
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();

                // Send real-time update via SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveDishUpdate", dish, "deleted");
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Hiển thị thực đơn dành cho khách hàng với giỏ hàng phiên làm việc
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Notes(Guid? tableId)
        {
            RestaurantTable? table = null;
            if (tableId.HasValue)
            {
                table = await _context.RestaurantTables
                    .Include(t => t.Branch)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TableId == tableId && t.Status != TableStatus.OutOfService);

                if (table == null)
                {
                    TempData["WarningMessage"] = "Bàn không hợp lệ hoặc đang tạm ngừng phục vụ.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var dishes = await (from d in _context.Dishes
                                .Include(d => d.Category)
                                .AsNoTracking()
                                where d.Active
                                orderby d.Category != null ? d.Category.Name : string.Empty, d.Name
                                select d).ToListAsync();

            if (tableId.HasValue)
            {
                dishes = dishes
                    .Where(d => d.IsAvailableOnline)
                    .ToList();
            }

            var categories = await (from c in _context.Categories.AsNoTracking()
                                    where c.Active
                                    orderby c.Name
                                    select c).ToListAsync();

            var cart = GetCartFromSession();

            ViewBag.Dishes = dishes;
            ViewBag.Categories = categories;
            ViewBag.Table = table;
            ViewBag.TableId = tableId;
            ViewBag.Cart = cart;
            ViewBag.CartCount = cart.Sum(x => x.Value);
            ViewBag.Title = table != null
                ? $"Gọi món - Bàn {table.Name}"
                : "Thực đơn dành cho khách";

            return View();
        }

        private bool DishExists(Guid id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }

        /// <summary>
        /// Lấy giỏ hàng khách đang chọn từ session PublicOrder
        /// </summary>
        private Dictionary<Guid, int> GetCartFromSession()
        {
            const string cartKey = "PUBLIC_ORDER_CART";
            var cartJson = HttpContext.Session.GetString(cartKey);
            if (string.IsNullOrEmpty(cartJson))
                return new Dictionary<Guid, int>();

            try
            {
                return JsonSerializer.Deserialize<Dictionary<Guid, int>>(cartJson)
                       ?? new Dictionary<Guid, int>();
            }
            catch
            {
                return new Dictionary<Guid, int>();
            }
        }
    }
}