using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLiKhoHang.Hubs;
using QuanLiKhoHang.Extensions;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller phục vụ bếp theo dõi và cập nhật trạng thái món đang chế biến
    /// </summary>
    public class KitchenTicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DishHub> _hubContext;
        private const int PageSize = 10;

        public KitchenTicketController(ApplicationDbContext context, IHubContext<DishHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? sectionId, string? status, string? search, string? sort, int page = 1)
        {
            var parsedStatus = TryParseStatus(status);
            var query = BuildTicketQuery(sectionId, parsedStatus, trackChanges: true);
            
            // Áp dụng tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = from oi in query
                        where (oi.Dish != null && oi.Dish.Name.Contains(search))
                           || (oi.Order != null && oi.Order.OrderNumber.Contains(search))
                           || (oi.Order != null && oi.Order.Customer != null && oi.Order.Customer.FullName.Contains(search))
                           || (oi.SpecialInstructions != null && oi.SpecialInstructions.Contains(search))
                        select oi;
            }
            
            // Áp dụng sắp xếp
            query = sort switch
            {
                "dish" => from oi in query orderby oi.Dish != null ? oi.Dish.Name : "" select oi,
                "dish_desc" => from oi in query orderby oi.Dish != null ? oi.Dish.Name : "" descending select oi,
                "time_asc" => from oi in query orderby (oi.Order != null ? oi.Order.PlacedAt : oi.CreatedAt) select oi,
                _ => from oi in query orderby (oi.Order != null ? oi.Order.PlacedAt : oi.CreatedAt) descending select oi
            };

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            
            var sections = await (from ks in _context.KitchenSections.AsNoTracking()
                                  orderby ks.Name
                                  select ks).ToListAsync();

            var sectionOptions = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = string.Empty,
                    Text = "Tất cả khu vực",
                    Selected = !sectionId.HasValue
                }
            };

            sectionOptions.AddRange(sections.Select(ks => new SelectListItem
            {
                Value = ks.KitchenSectionId.ToString(),
                Text = ks.Name,
                Selected = sectionId.HasValue && sectionId.Value == ks.KitchenSectionId
            }));

            var statusSelected = parsedStatus?.ToString() ?? string.Empty;
            var statusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Đang chờ / Đang nấu", Selected = string.IsNullOrEmpty(statusSelected) },
                new SelectListItem { Value = OrderItemStatus.Pending.ToString(), Text = "Chờ xử lý", Selected = statusSelected == OrderItemStatus.Pending.ToString() },
                new SelectListItem { Value = OrderItemStatus.SentToKitchen.ToString(), Text = "Đã gửi bếp", Selected = statusSelected == OrderItemStatus.SentToKitchen.ToString() },
                new SelectListItem { Value = OrderItemStatus.Preparing.ToString(), Text = "Đang nấu", Selected = statusSelected == OrderItemStatus.Preparing.ToString() },
                new SelectListItem { Value = OrderItemStatus.Ready.ToString(), Text = "Hoàn tất", Selected = statusSelected == OrderItemStatus.Ready.ToString() },
                new SelectListItem { Value = OrderItemStatus.Served.ToString(), Text = "Đã phục vụ", Selected = statusSelected == OrderItemStatus.Served.ToString() }
            };

            ViewBag.Items = items;
            ViewBag.SelectedSection = sectionId;
            ViewBag.StatusFilter = parsedStatus?.ToString() ?? string.Empty;
            ViewBag.SectionOptions = sectionOptions;
            ViewBag.StatusOptions = statusOptions;
            ViewBag.Title = "Bếp - Danh sách món cần chế biến";
            ViewBag.SearchTerm = search ?? "";
            ViewBag.SortOrder = sort ?? "default";
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.TotalItems = totalItems;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets(Guid? sectionId, string? status)
        {
            var parsedStatus = TryParseStatus(status);
            var query = BuildTicketQuery(sectionId, parsedStatus, trackChanges: false);
            var items = await query.ToListAsync();

            Console.WriteLine($"[Kitchen] GetTickets called - SectionId: {sectionId}, Status: {status}, Found: {items.Count} items");

            var tickets = items.Select(oi => new
            {
                orderItemId = oi.OrderItemId,
                orderId = oi.OrderId,
                orderNumber = oi.Order != null ? oi.Order.OrderNumber : string.Empty,
                tableName = oi.Order != null ? (oi.Order.Table != null ? oi.Order.Table.Name : "Mang về") : "",
                customerName = oi.Order != null ? (oi.Order.Customer != null ? oi.Order.Customer.FullName : "Khách vãng lai") : string.Empty,
                dishName = oi.Dish != null ? oi.Dish.Name : string.Empty,
                quantity = oi.Quantity,
                status = oi.Status.ToString(),
                sectionId = oi.KitchenSectionId,
                sectionName = oi.KitchenSection != null ? oi.KitchenSection.Name : string.Empty,
                placedAt = oi.Order != null ? oi.Order.PlacedAt.ToString("O") : oi.CreatedAt.ToString("O"),
                notes = oi.SpecialInstructions ?? string.Empty
            }).ToList();

            return Json(new { success = true, tickets });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItemStatus(Guid orderItemId, OrderItemStatus status)
        {
            if (!IsTargetStatusAllowed(status))
            {
                return Json(new { success = false, message = "Trạng thái không hợp lệ." });
            }

            var item = await _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o!.OrderItems)
                .Include(oi => oi.Dish)
                    .ThenInclude(d => d!.Ingredients)
                        .ThenInclude(di => di.InventoryItem)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);

            if (item == null)
            {
                return Json(new { success = false, message = "Không tìm thấy món cần cập nhật." });
            }

            if (!IsTransitionValid(item.Status, status))
            {
                return Json(new { success = false, message = "Không thể chuyển trạng thái theo yêu cầu." });
            }

            var now = DateTime.Now;
            if (item.Status != status)
            {
                // Nếu chuyển sang Preparing, check nguyên liệu TRƯỚC khi update status
                if (status == OrderItemStatus.Preparing && item.RequestedAt == default)
                {
                    // Trừ nguyên liệu từ kho khi bắt đầu nấu
                    var deductionResult = await DeductIngredientsFromInventory(item);
                    if (!deductionResult.Success)
                    {
                        return Json(new { success = false, message = deductionResult.Message });
                    }
                }

                // Nếu kiểm tra xong OK, mới update status
                item.Status = status;
                item.UpdatedAt = now;

                if (status == OrderItemStatus.Preparing && item.RequestedAt == default)
                {
                    item.RequestedAt = now;
                }

                if (status == OrderItemStatus.Ready)
                {
                    item.ServedAt = null;
                }

                if (status == OrderItemStatus.Served)
                {
                    item.ServedAt = now;
                }

                if (item.Order != null)
                {
                    UpdateOrderStatus(item.Order, now);
                }

                await _context.SaveChangesAsync();

                var pendingQuantity = await GetPendingQuantityForDish(item.DishId);
                await _hubContext.Clients.All.SendAsync("ReceiveDishPreparationUpdate", item.DishId, pendingQuantity);
                await _hubContext.Clients.All.SendAsync("KitchenTicketUpdated", new { orderId = item.OrderId });

                // Nếu tất cả items của order hoàn tất, update table status
                if (item.Order != null && item.Order.TableId.HasValue && item.Order.OrderItems != null)
                {
                    var allItemsReady = item.Order.OrderItems.All(oi => 
                        oi.Status == OrderItemStatus.Ready || oi.Status == OrderItemStatus.Served);
                    
                    if (allItemsReady)
                    {
                        // Emit signal để table dashboard cập nhật trạng thái
                        await _hubContext.SendTableStatusUpdated(
                            new { 
                                tableId = item.Order.TableId.Value, 
                                newStatus = "Chờ phục vụ"
                            });
                    }
                }
            }

            return Json(new { success = true });
        }

        private IQueryable<OrderItem> BuildTicketQuery(Guid? sectionId, OrderItemStatus? status, bool trackChanges)
        {
            var baseQuery = _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Table)
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Customer)
                .Include(oi => oi.Dish)
                .Include(oi => oi.KitchenSection)
                .AsQueryable();

            if (!trackChanges)
            {
                baseQuery = baseQuery.AsNoTracking();
            }

            var query =
                from oi in baseQuery
                select oi;

            if (sectionId.HasValue)
            {
                query =
                    from oi in query
                    where oi.KitchenSectionId == sectionId.Value
                    select oi;
            }

            if (status.HasValue)
            {
                query =
                    from oi in query
                    where oi.Status == status.Value
                    select oi;
            }
            else
            {
                // Mặc định: chỉ hiển thị các món đã được gửi bếp hoặc đang nấu
                query =
                    from oi in query
                    where oi.Status == OrderItemStatus.SentToKitchen
                       || oi.Status == OrderItemStatus.Preparing
                    select oi;
            }

            query =
                from oi in query
                orderby oi.Order != null ? oi.Order.PlacedAt : oi.CreatedAt
                select oi;

            return query;
        }

        private static OrderItemStatus? TryParseStatus(string? status)
        {
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse(status, true, out OrderItemStatus parsed))
            {
                return parsed;
            }

            return null;
        }

        private static bool IsTargetStatusAllowed(OrderItemStatus status)
        {
            return status == OrderItemStatus.SentToKitchen
                || status == OrderItemStatus.Preparing
                || status == OrderItemStatus.Ready
                || status == OrderItemStatus.Served;
        }

        private static bool IsTransitionValid(OrderItemStatus current, OrderItemStatus next)
        {
            if (current == next)
            {
                return true;
            }

            return current switch
            {
                OrderItemStatus.Pending => next == OrderItemStatus.SentToKitchen
                    || next == OrderItemStatus.Preparing
                    || next == OrderItemStatus.Ready,
                OrderItemStatus.SentToKitchen => next == OrderItemStatus.Preparing
                    || next == OrderItemStatus.Ready,
                OrderItemStatus.Preparing => next == OrderItemStatus.Ready,
                OrderItemStatus.Ready => next == OrderItemStatus.Served,
                _ => false
            };
        }

        private void UpdateOrderStatus(Order order, DateTime updatedAt)
        {
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                return;
            }

            var statuses = order.OrderItems.Select(oi => oi.Status).ToList();

            if (statuses.All(s => s == OrderItemStatus.Served))
            {
                order.Status = OrderStatus.Served;
                order.UpdatedAt = updatedAt;
                return;
            }

            if (statuses.All(s => s == OrderItemStatus.Ready || s == OrderItemStatus.Served))
            {
                order.Status = OrderStatus.Ready;
                order.UpdatedAt = updatedAt;
                return;
            }

            if (statuses.Any(s => s == OrderItemStatus.Preparing || s == OrderItemStatus.SentToKitchen))
            {
                if (order.Status != OrderStatus.InPreparation)
                {
                    order.Status = OrderStatus.InPreparation;
                    order.UpdatedAt = updatedAt;
                }
                return;
            }

            if (statuses.All(s => s == OrderItemStatus.Pending))
            {
                if (order.Status != OrderStatus.Confirmed)
                {
                    order.Status = OrderStatus.Confirmed;
                    order.UpdatedAt = updatedAt;
                }
            }
        }

        private async Task<decimal> GetPendingQuantityForDish(Guid dishId)
        {
            return await (from oi in _context.OrderItems.AsNoTracking()
                          where oi.DishId == dishId
                                && (oi.Status == OrderItemStatus.Pending
                                    || oi.Status == OrderItemStatus.SentToKitchen
                                    || oi.Status == OrderItemStatus.Preparing)
                          select oi.Quantity).SumAsync();
        }

        /// <summary>
        /// Trừ nguyên liệu từ kho dựa trên công thức nấu ăn (DishIngredient)
        /// Khi số lượng không đủ, cho phép trừ vào âm nhưng ghi chú lại trong StockTransaction
        /// </summary>
        private async Task<(bool Success, string Message)> DeductIngredientsFromInventory(OrderItem item)
        {
            if (item.Dish == null || item.Dish.Ingredients == null || !item.Dish.Ingredients.Any())
            {
                return (true, "Không có nguyên liệu cần trừ");
            }

            var now = DateTime.Now;

            // Kiểm tra xem có đủ nguyên liệu không trước khi deduct
            var missingIngredients = new List<string>();
            
            foreach (var dishIngredient in item.Dish.Ingredients.Where(di => !di.IsOptional && di.InventoryItem != null))
            {
                var quantityNeeded = dishIngredient.QuantityPerPortion * item.Quantity;

                if (quantityNeeded <= 0)
                    continue;

                var inventoryItem = dishIngredient.InventoryItem;
                if (inventoryItem == null)
                    continue;

                // Kiểm tra có đủ không
                if (inventoryItem.CurrentQuantity < quantityNeeded)
                {
                    missingIngredients.Add(
                        $"{inventoryItem.Name}: cần {quantityNeeded} {inventoryItem.Unit}, chỉ còn {inventoryItem.CurrentQuantity} {inventoryItem.Unit}");
                }
            }

            // Nếu không đủ bất kỳ nguyên liệu nào, reject
            if (missingIngredients.Any())
            {
                var message = "⚠️ KHÔNG ĐỦ NGUYÊN LIỆU! Chi tiết:\n" + string.Join("\n", missingIngredients);
                return (false, message);
            }

            // Nếu đủ, mới deduct
            foreach (var dishIngredient in item.Dish.Ingredients.Where(di => !di.IsOptional && di.InventoryItem != null))
            {
                var quantityNeeded = dishIngredient.QuantityPerPortion * item.Quantity;

                if (quantityNeeded <= 0)
                    continue;

                var inventoryItem = dishIngredient.InventoryItem;
                if (inventoryItem == null)
                    continue;

                // Tạo ghi chép StockTransaction (âm) để trừ từ kho
                var transaction = new StockTransaction
                {
                    StockTransactionId = Guid.NewGuid(),
                    ItemId = inventoryItem.ItemId,
                    TransactionType = StockTransactionType.SaleOut,
                    ReferenceId = item.OrderItemId, // Liên kết tới OrderItem
                    Quantity = -quantityNeeded, // Âm để biểu thị xuất kho
                    UnitCost = inventoryItem.CostPerUnit,
                    TotalCost = inventoryItem.CostPerUnit * quantityNeeded,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Note = $"Tự động trừ từ kho khi nấu món: {item.Dish.Name} (x{item.Quantity})"
                };

                _context.StockTransactions.Add(transaction);

                // Cập nhật CurrentQuantity trong InventoryItems
                inventoryItem.CurrentQuantity -= quantityNeeded;
                inventoryItem.UpdatedAt = now;
                _context.InventoryItems.Update(inventoryItem);
            }

            try
            {
                await _context.SaveChangesAsync();
                return (true, "✓ Đã trừ nguyên liệu từ kho thành công");
            }
            catch (Exception ex)
            {
                return (false, $"❌ Lỗi khi trừ nguyên liệu: {ex.Message}");
            }
        }
    }
}
