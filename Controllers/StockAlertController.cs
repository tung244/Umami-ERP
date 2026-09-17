using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Controllers;

[Authorize]
public class StockAlertController : Controller
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 10;

    public StockAlertController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Danh sách các threshold cảnh báo tồn kho
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        // Lấy tất cả thresholds
        var thresholds = await _context.StockAlertThresholds
            .AsNoTracking()
            .ToListAsync();

        // Lấy tất cả inventory items
        var inventoryItems = await _context.InventoryItems
            .AsNoTracking()
            .ToListAsync();

        // Join in-memory
        var query = from t in thresholds
                    join i in inventoryItems on t.ItemId equals i.ItemId into joined
                    from i in joined.DefaultIfEmpty()
                    select new { Threshold = t, Item = i };

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(q => 
                q.Item?.Name?.Contains(search) == true || 
                q.Item?.Sku?.Contains(search) == true);
        }

        var totalItems = query.Count();
        var items = query
            .OrderByDescending(q => q.Threshold.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewBag.Items = items;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
        ViewBag.SearchTerm = search ?? "";
        ViewBag.Title = "Cấu hình cảnh báo tồn kho";

        return View();
    }

    /// <summary>
    /// Auto-route tới Create hoặc Edit tùy threshold tồn tại
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ConfigureAlert(Guid itemId)
    {
        // Kiểm tra threshold đã tồn tại
        var existing = await _context.StockAlertThresholds
            .FirstOrDefaultAsync(t => t.ItemId == itemId);

        if (existing != null)
        {
            return RedirectToAction(nameof(Edit), new { id = existing.ThresholdId });
        }

        return RedirectToAction(nameof(Create));
    }

    /// <summary>
    /// Form tạo threshold mới
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var items = await _context.InventoryItems
            .Where(i => i.IsActive)
            .OrderBy(i => i.Name)
            .ToListAsync();

        ViewBag.Items = items;
        ViewBag.Title = "Tạo cảnh báo tồn kho mới";
        return View();
    }

    /// <summary>
    /// Lưu threshold mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Guid itemId, decimal minimumThreshold, string? alertEmails, bool isEnabled)
    {
        // Kiểm tra item tồn tại
        var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.ItemId == itemId);
        if (item == null)
        {
            ModelState.AddModelError("itemId", "Sản phẩm không tồn tại");
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra đã có threshold cho item này chưa
        var existing = await _context.StockAlertThresholds
            .FirstOrDefaultAsync(t => t.ItemId == itemId);
        if (existing != null)
        {
            ModelState.AddModelError("itemId", "Sản phẩm này đã có cấu hình cảnh báo");
            return RedirectToAction(nameof(Index));
        }

        // Validation
        if (minimumThreshold < 0)
        {
            ModelState.AddModelError("minimumThreshold", "Ngưỡng không thể âm");
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra email hợp lệ
        if (!string.IsNullOrEmpty(alertEmails))
        {
            var emails = alertEmails.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToList();

            foreach (var email in emails)
            {
                if (!IsValidEmail(email))
                {
                    ModelState.AddModelError("alertEmails", $"Email không hợp lệ: {email}");
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        var threshold = new StockAlertThreshold
        {
            ThresholdId = Guid.NewGuid(),
            ItemId = itemId,
            MinimumThreshold = minimumThreshold,
            AlertEmails = alertEmails,
            IsEnabled = isEnabled,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.StockAlertThresholds.Add(threshold);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Form chỉnh sửa threshold
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var threshold = await _context.StockAlertThresholds
            .Include(t => t.InventoryItem)
            .FirstOrDefaultAsync(t => t.ThresholdId == id);

        if (threshold == null)
            return NotFound();

        ViewBag.Threshold = threshold;
        ViewBag.Title = $"Chỉnh sửa cảnh báo: {threshold.InventoryItem?.Name}";
        return View();
    }

    /// <summary>
    /// Lưu chỉnh sửa threshold
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, decimal minimumThreshold, string? alertEmails, bool isEnabled)
    {
        var threshold = await _context.StockAlertThresholds.FirstOrDefaultAsync(t => t.ThresholdId == id);
        if (threshold == null)
            return NotFound();

        // Validation
        if (minimumThreshold < 0)
        {
            ModelState.AddModelError("minimumThreshold", "Ngưỡng không thể âm");
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra email hợp lệ
        if (!string.IsNullOrEmpty(alertEmails))
        {
            var emails = alertEmails.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToList();

            foreach (var email in emails)
            {
                if (!IsValidEmail(email))
                {
                    ModelState.AddModelError("alertEmails", $"Email không hợp lệ: {email}");
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        threshold.MinimumThreshold = minimumThreshold;
        threshold.AlertEmails = alertEmails;
        threshold.IsEnabled = isEnabled;
        threshold.UpdatedAt = DateTime.Now;

        _context.StockAlertThresholds.Update(threshold);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Xóa threshold
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var threshold = await _context.StockAlertThresholds.FirstOrDefaultAsync(t => t.ThresholdId == id);
        if (threshold == null)
            return NotFound();

        _context.StockAlertThresholds.Remove(threshold);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Xem lịch sử thông báo
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> NotificationHistory(Guid? itemId, string? status, int page = 1)
    {
        var query = from n in _context.StockAlertNotifications
                    join i in _context.InventoryItems on n.ItemId equals i.ItemId
                    select new { Notification = n, Item = i };

        if (itemId.HasValue && itemId.Value != Guid.Empty)
        {
            query = from q in query where q.Notification.ItemId == itemId select q;
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = from q in query where q.Notification.NotificationStatus == status select q;
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderByDescending(q => q.Notification.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        ViewBag.Items = items;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
        ViewBag.SelectedItemId = itemId ?? Guid.Empty;
        ViewBag.SelectedStatus = status ?? "";
        ViewBag.Title = "Lịch sử thông báo cảnh báo";

        var allItems = await _context.InventoryItems
            .Where(i => i.IsActive)
            .OrderBy(i => i.Name)
            .ToListAsync();
        ViewBag.AllItems = allItems;

        return View();
    }

    /// <summary>
    /// Kiểm tra email hợp lệ
    /// </summary>
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
