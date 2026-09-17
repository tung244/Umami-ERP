using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller quản lý danh mục món ăn
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public CategoryController(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// Hiển thị danh sách danh mục với tìm kiếm, sắp xếp, phân trang
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? sort, int page = 1)
        {
            var query =
                from c in _context.Categories.AsNoTracking()
                select c;

            if (!string.IsNullOrEmpty(search))
                query = from c in query where c.Name.Contains(search) select c;

            query = sort switch
            {
                "name" => from c in query orderby c.Name select c,
                "displayorder" => from c in query orderby c.DisplayOrder select c,
                _ => from c in query orderby c.CreatedAt descending select c
            };

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.Items = items;                 // List<Category>
            ViewBag.CurrentPage = page;            // int
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.SortOrder = sort ?? "default";
            ViewBag.Title = "Quản lý danh mục món ăn";
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết danh mục
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            ViewBag.Item = category;  // Category
            ViewBag.Title = "Chi tiết danh mục";
            return View();
        }

        /// <summary>
        /// Hiển thị form tạo danh mục mới
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            var parentCategories = _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.ParentCategories = parentCategories;  // List<Category>
            ViewBag.Title = "Tạo danh mục mới";
            return View();
        }

        /// <summary>
        /// Xử lý tạo danh mục mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.Categories.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var parentCategories = _context.Categories
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.ParentCategories = parentCategories;
            ViewBag.Title = "Tạo danh mục mới";
            return View(model);
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa danh mục
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            var parentCategories = _context.Categories
                .Where(c => c.Active && c.CategoryId != id)
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.ParentCategories = parentCategories;
            ViewBag.Title = "Chỉnh sửa danh mục";
            return View(category);
        }

        /// <summary>
        /// Xử lý chỉnh sửa danh mục
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category model)
        {
            if (id != model.CategoryId)
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
                    if (!CategoryExists(model.CategoryId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var parentCategories = _context.Categories
                .Where(c => c.Active && c.CategoryId != id)
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.ParentCategories = parentCategories;
            ViewBag.Title = "Chỉnh sửa danh mục";
            return View(model);
        }

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}