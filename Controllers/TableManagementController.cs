using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Controllers
{
    public class TableManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 20;

        public TableManagementController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? statusFilter, int page = 1)
        {
            var branchIdClaim = User.FindFirst("BranchId")?.Value;
            if (string.IsNullOrEmpty(branchIdClaim) || !Guid.TryParse(branchIdClaim, out var branchId))
                return Unauthorized();

            var query = from t in _context.RestaurantTables.AsNoTracking()
                        where t.BranchId == branchId
                        select t;

            if (!string.IsNullOrEmpty(search))
                query = from t in query
                        where t.Name.Contains(search) || t.Code.Contains(search)
                        select t;

            if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse<TableStatus>(statusFilter, out var status))
                query = from t in query
                        where t.Status == status
                        select t;

            query = from t in query orderby t.Code select t;

            var totalItems = await query.CountAsync();
            var tables = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.Tables = tables;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.StatusFilter = statusFilter ?? "";
            ViewBag.Title = "Quản lý bàn";
            ViewBag.TableStatuses = Enum.GetValues(typeof(TableStatus))
                .Cast<TableStatus>()
                .Select(s => new { Value = s.ToString(), Text = GetTableStatusText(s) })
                .ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid tableId)
        {
            var branchIdClaim = User.FindFirst("BranchId")?.Value;
            if (string.IsNullOrEmpty(branchIdClaim) || !Guid.TryParse(branchIdClaim, out var branchId))
                return Unauthorized();

            var table = await _context.RestaurantTables
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TableId == tableId && t.BranchId == branchId);

            if (table == null)
                return NotFound();

            // Lấy order gần nhất của bàn (bao gồm cả Served để hiển thị nút thanh toán)
            var currentOrder = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Where(o => o.TableId == tableId && 
                       (o.Status == OrderStatus.Open || 
                        o.Status == OrderStatus.Confirmed ||
                        o.Status == OrderStatus.InPreparation ||
                        o.Status == OrderStatus.Ready ||
                        o.Status == OrderStatus.Served))  // ✅ Thêm Served
                .OrderByDescending(o => o.PlacedAt)
                .FirstOrDefaultAsync();

            ViewBag.Table = table;
            ViewBag.CurrentOrder = currentOrder;
            ViewBag.Title = $"Chi tiết bàn {table.Name}";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTableStatus(Guid tableId, string status)
        {
            var branchIdClaim = User.FindFirst("BranchId")?.Value;
            if (string.IsNullOrEmpty(branchIdClaim) || !Guid.TryParse(branchIdClaim, out var branchId))
                return Unauthorized();

            if (!Enum.TryParse<TableStatus>(status, out var newStatus))
                return BadRequest(new { error = "Invalid status" });

            var table = await _context.RestaurantTables
                .FirstOrDefaultAsync(t => t.TableId == tableId && t.BranchId == branchId);

            if (table == null)
                return NotFound();

            table.Status = newStatus;
            table.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, newStatus = newStatus.ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsServed(Guid tableId)
        {
            var branchIdClaim = User.FindFirst("BranchId")?.Value;
            if (string.IsNullOrEmpty(branchIdClaim) || !Guid.TryParse(branchIdClaim, out var branchId))
                return Unauthorized();

            var table = await _context.RestaurantTables
                .FirstOrDefaultAsync(t => t.TableId == tableId && t.BranchId == branchId);

            if (table == null)
                return NotFound();

            // Cập nhật trạng thái bàn thành chờ thanh toán
            table.Status = TableStatus.Occupied;
            table.UpdatedAt = DateTime.UtcNow;

            // Cập nhật trạng thái order MỚI NHẤT sang Served
            var activeOrder = await _context.Orders
                .Where(o => o.TableId == tableId && 
                       (o.Status == OrderStatus.Open || 
                        o.Status == OrderStatus.Confirmed ||
                        o.Status == OrderStatus.InPreparation ||
                        o.Status == OrderStatus.Ready))
                .OrderByDescending(o => o.PlacedAt)  // ✅ Lấy đơn mới nhất
                .FirstOrDefaultAsync();

            if (activeOrder != null)
            {
                activeOrder.Status = OrderStatus.Served; // ✅ Đổi từ Ready thành Served
                activeOrder.ServedAt = DateTime.UtcNow;
                activeOrder.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoToCheckout(Guid tableId)
        {
            var branchIdClaim = User.FindFirst("BranchId")?.Value;
            if (string.IsNullOrEmpty(branchIdClaim) || !Guid.TryParse(branchIdClaim, out var branchId))
                return Unauthorized();

            var table = await _context.RestaurantTables
                .FirstOrDefaultAsync(t => t.TableId == tableId && t.BranchId == branchId);

            if (table == null)
                return NotFound();

            // Tìm đơn hàng MỚI NHẤT đang active của bàn này
            var activeOrder = await _context.Orders
                .Where(o => o.TableId == tableId && 
                       (o.Status == OrderStatus.Served || 
                        o.Status == OrderStatus.Ready))
                .OrderByDescending(o => o.PlacedAt)  // ✅ Lấy đơn mới nhất
                .FirstOrDefaultAsync();

            if (activeOrder == null)
                return NotFound("Không tìm thấy đơn hàng cần thanh toán");

            // Redirect đến trang chi tiết đơn hàng để thanh toán QR
            return RedirectToAction("Details", "Order", new { id = activeOrder.OrderId });
        }

        private string GetTableStatusText(TableStatus status)
        {
            return status switch
            {
                TableStatus.Available => "Trống",
                TableStatus.Occupied => "Có khách",
                TableStatus.Reserved => "Đã đặt trước",
                TableStatus.OutOfService => "Ngừng sử dụng",
                _ => "Không xác định"
            };
        }
    }
}
