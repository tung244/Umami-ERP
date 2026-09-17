using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý nguyên liệu của món ăn
    /// </summary>
    public class DishIngredientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public DishIngredientController(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// Hiển thị nguyên liệu của một món ăn
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(Guid dishId, int page = 1)
        {
            var dish = await _context.Dishes
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DishId == dishId);

            if (dish == null)
                return NotFound();

            var query =
                from di in _context.DishIngredients
                    .Include(di => di.InventoryItem)
                    .Where(di => di.DishId == dishId)
                    .AsNoTracking()
                orderby di.InventoryItem.Name
                select di;

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.Dish = dish;                   // Dish
            ViewBag.Items = items;                 // List<DishIngredient>
            ViewBag.CurrentPage = page;            // int
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.Title = $"Nguyên liệu - {dish.Name}";
            return View();
        }

        /// <summary>
        /// Hiển thị form thêm nguyên liệu cho món
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create(Guid dishId)
        {
            var dish = await _context.Dishes
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DishId == dishId);

            if (dish == null)
                return NotFound();

            var availableItems = await _context.InventoryItems
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();

            ViewBag.Dish = dish;
            ViewBag.AvailableItems = availableItems; // List<InventoryItem>
            ViewBag.Title = $"Thêm nguyên liệu - {dish.Name}";
            return View();
        }

        /// <summary>
        /// Xử lý thêm nguyên liệu cho món
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid dishId, Guid inventoryItemId, decimal quantityPerPortion, bool isOptional)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
            var inventoryItem = await _context.InventoryItems.FindAsync(inventoryItemId);

            if (dish == null || inventoryItem == null)
                return NotFound();

            // Kiểm tra đã tồn tại chưa
            var existing = await _context.DishIngredients
                .FirstOrDefaultAsync(di => di.DishId == dishId && di.InventoryItemId == inventoryItemId);

            if (existing != null)
            {
                ModelState.AddModelError("", "Nguyên liệu này đã được thêm cho món ăn.");
                var availableItems = await _context.InventoryItems
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.Name)
                    .ToListAsync();
                ViewBag.Dish = dish;
                ViewBag.AvailableItems = availableItems;
                ViewBag.Title = $"Thêm nguyên liệu - {dish.Name}";
                return View();
            }

            var dishIngredient = new DishIngredient
            {
                DishId = dishId,
                InventoryItemId = inventoryItemId,
                QuantityPerPortion = quantityPerPortion,
                IsOptional = isOptional,
                CreatedAt = DateTime.Now
            };

            _context.DishIngredients.Add(dishIngredient);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { dishId });
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa nguyên liệu
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dishIngredient = await _context.DishIngredients
                .Include(di => di.Dish)
                .Include(di => di.InventoryItem)
                .FirstOrDefaultAsync(di => di.DishIngredientId == id);

            if (dishIngredient == null)
                return NotFound();

            ViewBag.Title = $"Sửa nguyên liệu - {dishIngredient.Dish?.Name}";
            return View(dishIngredient);
        }

        /// <summary>
        /// Xử lý chỉnh sửa nguyên liệu
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, decimal quantityPerPortion, bool isOptional)
        {
            var dishIngredient = await _context.DishIngredients
                .Include(di => di.Dish)
                .FirstOrDefaultAsync(di => di.DishIngredientId == id);

            if (dishIngredient == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dishIngredient.QuantityPerPortion = quantityPerPortion;
                    dishIngredient.IsOptional = isOptional;
                    dishIngredient.UpdatedAt = DateTime.Now;

                    _context.Update(dishIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishIngredientExists(dishIngredient.DishIngredientId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index), new { dishId = dishIngredient.DishId });
            }

            ViewBag.Title = $"Sửa nguyên liệu - {dishIngredient.Dish?.Name}";
            return View(dishIngredient);
        }

        /// <summary>
        /// Xóa nguyên liệu khỏi món
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dishIngredient = await _context.DishIngredients
                .Include(di => di.Dish)
                .FirstOrDefaultAsync(di => di.DishIngredientId == id);

            if (dishIngredient != null)
            {
                _context.DishIngredients.Remove(dishIngredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { dishId = dishIngredient.DishId });
            }

            return NotFound();
        }

        private bool DishIngredientExists(Guid id)
        {
            return _context.DishIngredients.Any(e => e.DishIngredientId == id);
        }
    }
}