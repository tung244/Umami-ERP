using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý lịch sử giá món ăn
    /// </summary>
    public class MenuPriceHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public MenuPriceHistoryController(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// Hiển thị tất cả lịch sử giá món ăn
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, Guid? dishFilter, int page = 1)
        {
            var query = _context.MenuPriceHistory
                .Include(p => p.Dish)
                .Include(p => p.ChangedByStaff)
                .AsNoTracking()
                .AsQueryable();

            // Search by dish name
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Dish!.Name.Contains(search) || 
                                        (p.Dish.Sku != null && p.Dish.Sku.Contains(search)));
            }

            // Filter by dish
            if (dishFilter.HasValue)
            {
                query = query.Where(p => p.DishId == dishFilter.Value);
            }

            query = query.OrderByDescending(p => p.EffectiveFrom);

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            // Get all dishes for filter dropdown
            var dishes = await _context.Dishes
                .OrderBy(d => d.Name)
                .Select(d => new { d.DishId, d.Name, d.Sku })
                .ToListAsync();

            ViewBag.Items = items;                 // List<MenuPriceHistory>
            ViewBag.Dishes = dishes;               // Dish list for dropdown
            ViewBag.CurrentPage = page;            // int
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search;
            ViewBag.DishFilter = dishFilter;
            ViewBag.Title = "Quản lý lịch sử giá";
            return View();
        }

        /// <summary>
        /// Hiển thị form cập nhật giá mới
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create(Guid? dishId)
        {
            Dish? dish = null;
            decimal currentPrice = 0;

            if (dishId.HasValue)
            {
                dish = await _context.Dishes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.DishId == dishId.Value);

                if (dish != null)
                {
                    var currentPriceHistory = await _context.MenuPriceHistory
                        .Where(p => p.DishId == dishId.Value && p.EffectiveTo == null)
                        .OrderByDescending(p => p.EffectiveFrom)
                        .FirstOrDefaultAsync();

                    currentPrice = currentPriceHistory?.Price ?? dish.DefaultServingPrice;
                }
            }

            var allDishes = await _context.Dishes
                .Where(d => d.Active)
                .OrderBy(d => d.Name)
                .ToListAsync();

            var staffList = await _context.Staff
                .Where(s => s.IsActive)
                .OrderBy(s => s.FirstName)
                .ToListAsync();

            ViewBag.Dish = dish;
            ViewBag.AllDishes = allDishes;        // For dropdown
            ViewBag.CurrentPrice = currentPrice;
            ViewBag.StaffList = staffList;         // List<Staff>
            ViewBag.Title = dish != null ? $"Cập nhật giá - {dish.Name}" : "Cập nhật giá món ăn";
            return View();
        }

        /// <summary>
        /// Xử lý cập nhật giá mới và thông tin món ăn
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid dishId, decimal newPrice, DateTime effectiveFrom, Guid? changedByStaffId, string? reason,
            bool? taxable, bool? isAvailableOnline, string? description, string? portionSize, int? preparationTimeMinutes, string? imageUrl)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
                return NotFound();

            // Kết thúc giá hiện tại
            var currentPrice = await _context.MenuPriceHistory
                .Where(p => p.DishId == dishId && p.EffectiveTo == null)
                .FirstOrDefaultAsync();

            if (currentPrice != null)
            {
                currentPrice.EffectiveTo = effectiveFrom.AddSeconds(-1);
                _context.Update(currentPrice);
            }

            // Cập nhật giá hiện tại và thông tin món ăn
            dish.DefaultServingPrice = newPrice;
            if (taxable.HasValue) dish.Taxable = taxable.Value;
            if (isAvailableOnline.HasValue) dish.IsAvailableOnline = isAvailableOnline.Value;
            if (!string.IsNullOrEmpty(description)) dish.Description = description;
            if (!string.IsNullOrEmpty(portionSize)) dish.PortionSize = portionSize;
            if (preparationTimeMinutes.HasValue) dish.PreparationTimeMinutes = preparationTimeMinutes.Value;
            if (!string.IsNullOrEmpty(imageUrl)) dish.ImageUrl = imageUrl;
            dish.UpdatedAt = DateTime.Now;
            _context.Update(dish);

            // Tạo giá mới
            var priceHistory = new MenuPriceHistory
            {
                DishId = dishId,
                Price = newPrice,
                EffectiveFrom = effectiveFrom,
                ChangedByStaffId = changedByStaffId,
                Reason = reason,
                CreatedAt = DateTime.Now
            };

            _context.MenuPriceHistory.Add(priceHistory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Hiển thị chi tiết lịch sử giá
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var priceHistory = await _context.MenuPriceHistory
                .Include(p => p.Dish)
                .Include(p => p.ChangedByStaff)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.MenuPriceId == id);

            if (priceHistory == null)
                return NotFound();

            ViewBag.Item = priceHistory;  // MenuPriceHistory
            ViewBag.Title = "Chi tiết lịch sử giá";
            return View();
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa lịch sử giá
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var priceHistory = await _context.MenuPriceHistory
                .Include(p => p.Dish)
                .FirstOrDefaultAsync(p => p.MenuPriceId == id);

            if (priceHistory == null)
                return NotFound();

            var staffList = await _context.Staff
                .Where(s => s.IsActive)
                .OrderBy(s => s.FirstName)
                .ToListAsync();

            ViewBag.Item = priceHistory;
            ViewBag.StaffList = staffList;
            ViewBag.Title = $"Chỉnh sửa lịch sử giá - {priceHistory.Dish?.Name}";
            return View();
        }

        /// <summary>
        /// Xử lý chỉnh sửa lịch sử giá
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, decimal price, DateTime effectiveFrom, DateTime? effectiveTo, Guid? changedByStaffId, string? reason)
        {
            var priceHistory = await _context.MenuPriceHistory.FindAsync(id);
            if (priceHistory == null)
                return NotFound();

            priceHistory.Price = price;
            priceHistory.EffectiveFrom = effectiveFrom;
            priceHistory.EffectiveTo = effectiveTo;
            priceHistory.ChangedByStaffId = changedByStaffId;
            priceHistory.Reason = reason;
            priceHistory.UpdatedAt = DateTime.Now;

            _context.Update(priceHistory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Xóa lịch sử giá
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var priceHistory = await _context.MenuPriceHistory.FindAsync(id);
            if (priceHistory == null)
                return NotFound();

            _context.MenuPriceHistory.Remove(priceHistory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}