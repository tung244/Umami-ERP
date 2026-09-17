using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using QuanLiKhoHang.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using QuanLiKhoHang.Hubs;
using QuanLiKhoHang.Services;

namespace QuanLiKhoHang.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DishHub> _hubContext;
        private readonly LoyaltyService _loyaltyService;
        private readonly IReportUpdateService _reportUpdateService;
        private const int PageSize = 10;

        public OrderController(
            ApplicationDbContext context, 
            IHubContext<DishHub> hubContext, 
            LoyaltyService loyaltyService,
            IReportUpdateService reportUpdateService)
        {
            _context = context;
            _hubContext = hubContext;
            _loyaltyService = loyaltyService;
            _reportUpdateService = reportUpdateService;
        }

        // GET: Order
        [HttpGet]
        public async Task<IActionResult> Index(string? status, string? search, int page = 1)
        {
            var query = from o in _context.Orders.AsNoTracking()
                       .Include(o => o.Table)
                       .Include(o => o.Customer)
                       .Include(o => o.CreatedByStaff)
                       .Include(o => o.OrderItems)
                       select o;

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, out var orderStatus))
            {
                query = from o in query where o.Status == orderStatus select o;
            }
            else
            {
                // Mặc định hiển thị đơn hàng đang xử lý và đơn hàng đã hủy
                query = from o in query
                       where o.Status == OrderStatus.Open ||
                             o.Status == OrderStatus.Confirmed ||
                             o.Status == OrderStatus.InPreparation ||
                             o.Status == OrderStatus.Ready ||
                             o.Status == OrderStatus.Cancelled
                       select o;
            }

            // Tìm kiếm theo số đơn hàng hoặc tên khách hàng
            if (!string.IsNullOrEmpty(search))
            {
                query = from o in query
                       where o.OrderNumber.Contains(search) ||
                             (o.Customer != null && o.Customer.FullName.Contains(search))
                       select o;
            }

            // Sắp xếp theo thời gian đặt gần nhất
            query = from o in query orderby o.PlacedAt descending select o;

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            // Keep backwards compatibility and support views expecting either ViewBag.Items or ViewBag.Orders
            ViewBag.Items = items;
            ViewBag.Orders = items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            ViewBag.SearchTerm = search ?? "";
            ViewBag.StatusFilter = status ?? "";
            ViewBag.Title = "Quản lý đơn hàng";

            return View();
        }

        // GET: Order/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách bàn trống
            var availableTables = await (from t in _context.RestaurantTables.AsNoTracking()
                                       where t.Status == TableStatus.Available
                                       orderby t.Code
                                       select t).ToListAsync();

            // Lấy danh sách khách hàng (lấy toàn bộ rồi sort client-side vì FullName là computed property)
            var customers = await (from c in _context.Customers.AsNoTracking()
                                  select c).ToListAsync();
            customers = customers.OrderBy(c => c.FullName).ToList();

            // Lấy danh sách món ăn có sẵn
            var availableDishes = await (from d in _context.Dishes.AsNoTracking()
                                       .Include(d => d.Category)
                                       where d.Active
                                       select d).ToListAsync();

            availableDishes = availableDishes.OrderBy(d => d.Category?.Name).ThenBy(d => d.Name).ToList();

            // Gán vào ViewBag với tên phù hợp với Create.cshtml
            ViewBag.Tables = availableTables;
            ViewBag.Customers = customers;
            ViewBag.Dishes = availableDishes;
            ViewBag.Title = "Tạo đơn hàng mới";

            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload data for form
                var availableTables = await (from t in _context.RestaurantTables.AsNoTracking()
                                           where t.Status == TableStatus.Available
                                           orderby t.Code
                                           select t).ToListAsync();

                var availableDishes = await (from d in _context.Dishes.AsNoTracking()
                                           .Include(d => d.Category)
                                           where d.Active && d.IsAvailableOnline
                                           select d).ToListAsync();

                availableDishes = availableDishes.OrderBy(d => d.Category?.Name).ThenBy(d => d.Name).ToList();

                ViewBag.AvailableTables = availableTables;
                ViewBag.AvailableDishes = availableDishes;
                ViewBag.Title = "Tạo đơn hàng mới";
                return View(model);
            }

            // Tạo đơn hàng mới
            // Lấy BranchId từ bàn nếu có (bàn có thể thuộc chi nhánh), nếu không lấy branch hiện tại
            Guid branchId = GetCurrentBranchId();
            if (model.TableId.HasValue)
            {
                var tableForBranch = await _context.RestaurantTables.FindAsync(model.TableId.Value);
                if (tableForBranch != null)
                {
                    branchId = tableForBranch.BranchId;
                }
            }

            // Lấy StaffId của nhân viên đăng nhập
            var staffId = await GetCurrentStaffIdAsync();
            if (staffId == Guid.Empty)
            {
                return BadRequest("Không thể lấy thông tin nhân viên");
            }

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = GenerateOrderNumber(),
                BranchId = branchId,
                TableId = model.TableId,
                CustomerId = model.CustomerId,
                CreatedByStaffId = staffId,
                OrderType = OrderType.DineIn,
                OrderSource = OrderSource.Pos,
                PlacedAt = DateTime.Now,
                Status = OrderStatus.Open,
                SubTotal = 0,
                DiscountAmount = 0,
                TaxAmount = 0,
                ServiceChargeAmount = 0,
                RoundingAdjustment = 0,
                TotalAmount = 0,
                PaidAmount = 0,
                BalanceDue = 0,
                PaymentStatus = PaymentStatus.Pending,
                IsVoided = false,
                CreatedAt = DateTime.Now
            };

            // Thêm items vào đơn hàng
            if (model.Items != null && model.Items.Any())
            {
                foreach (var item in model.Items)
                {
                    var dish = await _context.Dishes.FindAsync(item.DishId);
                    if (dish != null)
                    {
                        var currentPrice = await GetCurrentDishPrice(item.DishId);
                        var orderItem = new OrderItem
                        {
                            OrderItemId = Guid.NewGuid(),
                            OrderId = order.OrderId,
                            DishId = item.DishId,
                            MenuPriceId = currentPrice.MenuPriceId,
                            Quantity = item.Quantity,
                            UnitPrice = currentPrice.Price,
                            LineTotal = currentPrice.Price * item.Quantity,
                            Status = OrderItemStatus.Pending,
                            RequestedAt = DateTime.Now,
                            IsDiscounted = false,
                            DiscountAmount = 0,
                            SpecialInstructions = item.SpecialInstructions,
                            CreatedAt = DateTime.Now
                        };

                        order.SubTotal += orderItem.LineTotal;
                        order.OrderItems.Add(orderItem);
                    }
                }
            }

            // Tính tổng tiền
            order.TotalAmount = order.SubTotal - order.DiscountAmount + order.TaxAmount + order.ServiceChargeAmount + order.RoundingAdjustment;
            order.BalanceDue = order.TotalAmount;

            // Cập nhật trạng thái bàn
            if (order.TableId.HasValue)
            {
                var table = await _context.RestaurantTables.FindAsync(order.TableId.Value);
                if (table != null)
                {
                    table.Status = TableStatus.Occupied;
                    table.UpdatedAt = DateTime.Now;
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Thông báo cập nhật báo cáo realtime
            await _reportUpdateService.NotifyRevenueUpdate($"Đơn hàng mới {order.OrderNumber}");
            await _reportUpdateService.NotifyTrendsUpdate("Cập nhật peak hours");

            // Send real-time updates to kitchen for each dish in the order
            foreach (var item in order.OrderItems)
            {
                var pendingQuantity = await _context.OrderItems
                    .Where(oi => oi.DishId == item.DishId && oi.Status == OrderItemStatus.Pending)
                    .SumAsync(oi => oi.Quantity);
                await _hubContext.Clients.All.SendAsync("ReceiveDishPreparationUpdate", item.DishId, pendingQuantity);
            }

            // Thông báo đơn hàng mới cho KitchenTicket
            var kitchenTicketData = new
            {
                orderId = order.OrderId,
                orderNumber = order.OrderNumber,
                tableNumber = order.Table?.Code ?? "Mang về",
                itemCount = order.OrderItems.Count,
                createdAt = DateTime.Now.ToString("HH:mm:ss")
            };
            await _hubContext.Clients.All.SendAsync("KitchenTicketAdded", kitchenTicketData);

            TempData["SuccessMessage"] = $"Đơn hàng {order.OrderNumber} đã được tạo thành công!";
            return RedirectToAction("Details", new { id = order.OrderId });
        }

        // GET: Order/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _context.Orders.AsNoTracking()
                .Include(o => o.Table)
                .Include(o => o.Customer)
                .Include(o => o.CreatedByStaff)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // Load active vouchers for dropdown
            var now = DateTime.Now;
            var activeVouchers = await _context.Vouchers
                .Where(v => v.IsActive && v.ValidFrom <= now && v.ValidTo > now)
                .OrderBy(v => v.Code)
                .Select(v => new
                {
                    v.Code,
                    v.Description,
                    v.DiscountType,
                    v.DiscountValue,
                    v.MinimumOrderAmount
                })
                .ToListAsync();

            // Lấy các quyền lợi (benefits) của khách hàng nếu có
            var customerBenefits = new List<dynamic>();
            if (order.CustomerId.HasValue)
            {
                var customerId = order.CustomerId.Value;
                Console.WriteLine($"\n[DEBUG BENEFIT] ===== Starting benefit lookup for customer: {customerId} =====");

                // Step 1: Kiểm tra LoyaltyAccount
                var loyaltyAccounts = await _context.LoyaltyAccounts.AsNoTracking()
                    .Where(la => la.CustomerId == customerId)
                    .ToListAsync();
                Console.WriteLine($"[DEBUG BENEFIT] Step 1: Found {loyaltyAccounts.Count} LoyaltyAccounts for customer");
                foreach (var la in loyaltyAccounts)
                {
                    Console.WriteLine($"  - LoyaltyAccountId: {la.LoyaltyAccountId}, Tier: {la.Tier}");
                }

                // Step 2: Kiểm tra TierConfigs
                var tierConfigs = await _context.TierConfigs.AsNoTracking()
                    .Where(tc => tc.IsActive)
                    .ToListAsync();
                Console.WriteLine($"[DEBUG BENEFIT] Step 2: Found {tierConfigs.Count} active TierConfigs");
                foreach (var tc in tierConfigs)
                {
                    Console.WriteLine($"  - TierConfigId: {tc.TierConfigId}, Tier: {tc.Tier}, IsActive: {tc.IsActive}");
                }

                // Step 3: Kiểm tra TierBenefits
                var tierBenefits = await _context.TierBenefits.AsNoTracking()
                    .Where(tb => tb.IsActive)
                    .ToListAsync();
                Console.WriteLine($"[DEBUG BENEFIT] Step 3: Found {tierBenefits.Count} active TierBenefits");
                foreach (var tb in tierBenefits)
                {
                    Console.WriteLine($"  - TierBenefitId: {tb.TierBenefitId}, TierConfigId: {tb.TierConfigId}, BenefitId: {tb.BenefitId}, IsActive: {tb.IsActive}");
                }

                // Step 4: Kiểm tra Benefits
                var benefits = await _context.Benefits.AsNoTracking()
                    .Where(b => b.IsActive)
                    .ToListAsync();
                Console.WriteLine($"[DEBUG BENEFIT] Step 4: Found {benefits.Count} active Benefits");
                foreach (var b in benefits)
                {
                    Console.WriteLine($"  - BenefitId: {b.BenefitId}, Name: {b.BenefitName}, Type: {b.BenefitType}, IsActive: {b.IsActive}");
                }

                // Step 5: Thử cách chính - join từ LoyaltyAccount
                var customerBenefitIds = await (
                    from la in _context.LoyaltyAccounts.AsNoTracking()
                    where la.CustomerId == customerId
                    join tc in _context.TierConfigs.AsNoTracking() on la.Tier equals tc.Tier
                    join tb in _context.TierBenefits.AsNoTracking() on tc.TierConfigId equals tb.TierConfigId
                    join b in _context.Benefits.AsNoTracking() on tb.BenefitId equals b.BenefitId
                    select b.BenefitId
                ).Distinct().ToListAsync();

                Console.WriteLine($"[DEBUG BENEFIT] Step 5: Found {customerBenefitIds.Count} benefit IDs via main query");
                foreach (var benefitId in customerBenefitIds)
                {
                    Console.WriteLine($"  - Benefit ID: {benefitId}");
                }

                if (customerBenefitIds.Any())
                {
                    customerBenefits = (await _context.Benefits
                        .Where(b => b.IsActive && customerBenefitIds.Contains(b.BenefitId))
                        .AsNoTracking()
                        .ToListAsync())
                        .Select(b => (dynamic)new {
                            Code = b.BenefitType,
                            Description = b.BenefitName,
                            DiscountType = b.DiscountPercent.HasValue ? VoucherDiscountType.Percentage : VoucherDiscountType.FixedAmount,
                            DiscountValue = b.DiscountPercent ?? b.DiscountAmount ?? 0,
                            IsBenefit = true
                        }).ToList();

                    Console.WriteLine($"[DEBUG BENEFIT] Step 6: Transformed into {customerBenefits.Count} benefit objects");
                    foreach (var benefit in customerBenefits)
                    {
                        Console.WriteLine($"  - Code: {benefit.Code}, Desc: {benefit.Description}, Value: {benefit.DiscountValue}");
                    }
                }
                else
                {
                    Console.WriteLine($"[DEBUG BENEFIT] No benefits found for customer {customerId}");
                }

                Console.WriteLine($"[DEBUG BENEFIT] ===== End benefit lookup =====\n");
            }

            ViewBag.Order = order;
            ViewBag.Title = $"Chi tiết đơn hàng - {order.OrderNumber}";
            ViewBag.ActiveVouchers = activeVouchers;
            ViewBag.CustomerBenefits = customerBenefits; // Truyền danh sách quyền lợi riêng

            return View();
        }

        // GET: Order/AddItem
        [HttpGet]
        public async Task<IActionResult> AddItem(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Lấy danh sách món ăn hiện có
            var dishes = await (from d in _context.Dishes
                               where d.Active == true
                               orderby d.Category!.Name, d.Name
                               select d)
                .Include(d => d.Category)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Order = order;
            ViewBag.Dishes = dishes;
            ViewBag.Title = $"Thêm món vào đơn {order.OrderNumber}";
            return View();
        }

        // POST: Order/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Guid orderId, Guid dishId, decimal quantity, string? specialInstructions)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(dishId);
                if (dish == null)
                {
                    TempData["ErrorMessage"] = "Món ăn không tồn tại!";
                    return RedirectToAction("Details", new { id = orderId });
                }

                // Get current price
                var currentPrice = await GetCurrentDishPrice(dishId);

                // Kiểm tra món đã có trong đơn chưa
                var existingItem = await _context.OrderItems
                    .FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.DishId == dishId && oi.Status == OrderItemStatus.Pending);

                if (existingItem != null)
                {
                    // Cập nhật số lượng
                    existingItem.Quantity += (int)quantity;
                    existingItem.LineTotal = existingItem.UnitPrice * existingItem.Quantity;
                    existingItem.UpdatedAt = DateTime.Now;
                }
                else
                {
                    // Thêm món mới
                    var orderItem = new OrderItem
                    {
                        OrderItemId = Guid.NewGuid(),
                        OrderId = orderId,
                        DishId = dishId,
                        MenuPriceId = currentPrice.MenuPriceId,
                        Quantity = (int)quantity,
                        UnitPrice = currentPrice.Price,
                        LineTotal = currentPrice.Price * quantity,
                        Status = OrderItemStatus.Pending,
                        RequestedAt = DateTime.Now,
                        IsDiscounted = false,
                        DiscountAmount = 0,
                        SpecialInstructions = specialInstructions,
                        CreatedAt = DateTime.Now
                    };

                    _context.OrderItems.Add(orderItem);
                }

                // Save order items first
                await _context.SaveChangesAsync();

                // Then reload order fresh to get current totals
                var order = await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return NotFound();
                }

                // Cập nhật tổng tiền của đơn
                var totalLineAmount = await _context.OrderItems
                    .Where(oi => oi.OrderId == orderId && oi.Status == OrderItemStatus.Pending)
                    .SumAsync(oi => oi.LineTotal);

                // Attach order entity to context and update
                _context.Orders.Attach(order);
                order.SubTotal = totalLineAmount;
                order.TotalAmount = order.SubTotal - order.DiscountAmount + order.TaxAmount + order.ServiceChargeAmount + order.RoundingAdjustment;
                order.BalanceDue = order.TotalAmount - order.PaidAmount;
                order.UpdatedAt = DateTime.Now;
                _context.Orders.Update(order);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã thêm {dish.Name} vào đơn hàng!";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Đơn hàng đã được thay đổi bởi người khác. Vui lòng tải lại trang.";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Lỗi khi thêm món. Vui lòng thử lại.";
                return RedirectToAction("Details", new { id = orderId });
            }
        }

        // POST: Order/RemoveItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(Guid orderId, Guid orderItemId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng!" });
                }

                var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);
                if (orderItem == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy món ăn trong đơn hàng!" });
                }

                // Xóa món
                _context.OrderItems.Remove(orderItem);

                // Cập nhật tổng tiền
                order.SubTotal = order.OrderItems.Where(oi => oi.OrderItemId != orderItemId).Sum(oi => oi.LineTotal);
                order.TotalAmount = order.SubTotal - order.DiscountAmount + order.TaxAmount + order.ServiceChargeAmount + order.RoundingAdjustment;
                order.BalanceDue = order.TotalAmount - order.PaidAmount;
                order.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã xóa món khỏi đơn hàng!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi khi xóa món. Vui lòng thử lại." });
            }
        }

        // POST: Order/UpdateItemQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItemQuantity(Guid orderId, Guid orderItemId, int quantity)
        {
            try
            {
                if (quantity < 1)
                {
                    return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });
                }

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng!" });
                }

                var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);
                if (orderItem == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy món ăn trong đơn hàng!" });
                }

                // Cập nhật số lượng
                orderItem.Quantity = quantity;
                orderItem.LineTotal = orderItem.UnitPrice * quantity;
                orderItem.UpdatedAt = DateTime.Now;

                // Cập nhật tổng tiền
                order.SubTotal = order.OrderItems.Sum(oi => oi.LineTotal);
                order.TotalAmount = order.SubTotal - order.DiscountAmount + order.TaxAmount + order.ServiceChargeAmount + order.RoundingAdjustment;
                order.BalanceDue = order.TotalAmount - order.PaidAmount;
                order.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã cập nhật số lượng!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi khi cập nhật số lượng. Vui lòng thử lại." });
            }
        }        // POST: Order/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng!" });
                }

                // Chỉ cho phép hủy đơn hàng ở trạng thái "Mới tạo" hoặc "Đã xác nhận"
                if (order.Status != OrderStatus.Open && order.Status != OrderStatus.Confirmed)
                {
                    return Json(new { success = false, message = "Chỉ có thể hủy đơn hàng ở trạng thái 'Mới tạo' hoặc 'Đã xác nhận'!" });
                }

                // Đánh dấu đơn hàng là đã hủy thay vì xóa
                order.IsVoided = true;
                order.VoidReason = "Người dùng hủy đơn hàng";
                order.Status = OrderStatus.Cancelled;
                order.UpdatedAt = DateTime.Now;

                // Cập nhật trạng thái bàn nếu có
                if (order.TableId.HasValue)
                {
                    var table = await _context.RestaurantTables.FindAsync(order.TableId.Value);
                    if (table != null)
                    {
                        table.Status = TableStatus.Available;
                        table.UpdatedAt = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã hủy đơn hàng thành công!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi khi hủy đơn hàng. Vui lòng thử lại." });
            }
        }

        // POST: Order/ApplyVoucher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyVoucher(Guid orderId, string voucherCode)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v =>
                v.Code == voucherCode &&
                v.IsActive &&
                v.ValidFrom <= DateTime.Now &&
                v.ValidTo >= DateTime.Now);

            if (voucher == null)
            {
                TempData["ErrorMessage"] = "Mã voucher không hợp lệ hoặc đã hết hạn!";
                return RedirectToAction("Details", new { id = orderId });
            }

            // Kiểm tra điều kiện áp dụng
            if (order.SubTotal < voucher.MinimumOrderAmount)
            {
                TempData["ErrorMessage"] = $"Đơn hàng tối thiểu phải {voucher.MinimumOrderAmount:N0} VND để áp dụng voucher!";
                return RedirectToAction("Details", new { id = orderId });
            }

            // Tính giảm giá
            decimal discountAmount = 0;
            switch (voucher.DiscountType)
            {
                case VoucherDiscountType.Percentage:
                    discountAmount = order.SubTotal * (voucher.DiscountValue / 100);
                    break;
                case VoucherDiscountType.FixedAmount:
                    discountAmount = voucher.DiscountValue;
                    break;
                case VoucherDiscountType.FreeItem:
                    // Logic phức tạp hơn, tạm thời không implement
                    break;
            }

            order.DiscountAmount = discountAmount;
            order.TotalAmount = order.SubTotal - order.DiscountAmount + order.TaxAmount + order.ServiceChargeAmount + order.RoundingAdjustment;
            order.BalanceDue = order.TotalAmount - order.PaidAmount;
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã áp dụng voucher {voucher.Code}, giảm {discountAmount:N0} VND!";
            return RedirectToAction("Details", new { id = orderId });
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Guid orderId, decimal paidAmount, PaymentMethod paymentMethod)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Tính giảm giá tự động từ quyền lợi tier nếu có customer
            decimal autoDiscount = 0m;
            if (order.CustomerId.HasValue)
            {
                autoDiscount = await _loyaltyService.CalculateAutoDiscountAsync(order.CustomerId.Value, order.BalanceDue);
            }

            // Số tiền cần thanh toán sau giảm giá
            decimal amountAfterDiscount = order.BalanceDue - autoDiscount;

            if (paidAmount < amountAfterDiscount)
            {
                TempData["ErrorMessage"] = "Số tiền thanh toán không đủ!";
                return RedirectToAction("Details", new { id = orderId });
            }

            var staffId = await GetCurrentStaffIdAsync();
            if (staffId == Guid.Empty)
            {
                return BadRequest("Không thể lấy thông tin nhân viên");
            }

            // Tạo thanh toán
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                OrderId = orderId,
                Amount = paidAmount,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now,
                ProcessedByStaffId = staffId,
                Notes = autoDiscount > 0 ? $"Thanh toán tại POS - Giảm giá từ quyền lợi: {autoDiscount:N0} VND" : "Thanh toán tại POS",
                CreatedAt = DateTime.Now
            };

            // Cập nhật đơn hàng
            order.PaidAmount += paidAmount;
            order.BalanceDue = order.TotalAmount - order.PaidAmount;
            order.PaymentStatus = order.BalanceDue <= 0 ? PaymentStatus.Approved : PaymentStatus.Pending;
            order.Status = OrderStatus.Paid;
            order.ServedAt = DateTime.Now;
            order.ClosedByStaffId = staffId;
            order.UpdatedAt = DateTime.Now;

            // Cập nhật trạng thái bàn
            if (order.TableId.HasValue)
            {
                var table = await _context.RestaurantTables.FindAsync(order.TableId.Value);
                if (table != null)
                {
                    table.Status = TableStatus.Available;
                    table.UpdatedAt = DateTime.Now;
                }
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Thông báo cập nhật báo cáo realtime (payment mới)
            await _reportUpdateService.NotifyRevenueUpdate($"Thanh toán mới cho đơn {order.OrderNumber}");

            // Xử lý tích điểm nếu thanh toán thành công
            if (order.PaymentStatus == PaymentStatus.Approved && order.CustomerId.HasValue)
            {
                await _loyaltyService.ProcessPaymentLoyaltyAsync(orderId, paidAmount);
                // Thông báo cập nhật customer report (có thể có tier change)
                await _reportUpdateService.NotifyCustomerUpdate("Cập nhật loyalty points");
            }

            var discountMessage = autoDiscount > 0 ? $" (Áp dụng giảm giá: {autoDiscount:N0} VND)" : "";
            TempData["SuccessMessage"] = $"Đơn hàng {order.OrderNumber} đã được thanh toán thành công!{discountMessage}";
            return RedirectToAction("Details", new { id = orderId });
        }

        private string GenerateOrderNumber()
        {
            var today = DateTime.Now;
            var datePart = today.ToString("yyyyMMdd");
            var sequence = (_context.Orders.Count(o => o.CreatedAt.Date == today.Date) + 1).ToString("D4");
            return $"ORD-{datePart}-{sequence}";
        }

        private Guid GetCurrentBranchId()
        {
            // Lấy từ claims hoặc config, tạm thời hardcode
            return Guid.Parse("00000000-0000-0000-0000-000000000001");
        }

        private async Task<(Guid MenuPriceId, decimal Price)> GetCurrentDishPrice(Guid dishId)
        {
            var currentPrice = await (from mp in _context.MenuPriceHistory.AsNoTracking()
                                    where mp.DishId == dishId &&
                                          mp.EffectiveFrom <= DateTime.Now &&
                                          (mp.EffectiveTo == null || mp.EffectiveTo >= DateTime.Now)
                                    orderby mp.EffectiveFrom descending
                                    select new { mp.MenuPriceId, mp.Price })
                                   .FirstOrDefaultAsync();

            return currentPrice != null
                ? (currentPrice.MenuPriceId, currentPrice.Price)
                : (Guid.Empty, 0m);
        }

        private async Task<Guid> GetCurrentStaffIdAsync()
        {
            var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(staffIdClaim, out var staffId))
            {
                return staffId;
            }
            
            // Fallback: get the first admin staff from the database
            var adminStaff = await _context.Staff.FirstOrDefaultAsync();
            return adminStaff?.StaffId ?? Guid.Empty;
        }

        // POST: Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(Guid orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            Console.WriteLine($"[ORDER] UpdateStatus called - OrderId: {orderId}, OldStatus: {order.Status}, NewStatus: {newStatus}");

            order.Status = newStatus;
            order.UpdatedAt = DateTime.Now;

            // Cập nhật status của OrderItems tương ứng
            if (newStatus == OrderStatus.InPreparation)
            {
                // Chuyển các item Pending sang SentToKitchen
                var itemsToUpdate = order.OrderItems.Where(oi => oi.Status == OrderItemStatus.Pending).ToList();
                Console.WriteLine($"[ORDER] InPreparation: Updating {itemsToUpdate.Count} items to SentToKitchen");
                
                foreach (var item in itemsToUpdate)
                {
                    Console.WriteLine($"  - Item {item.OrderItemId} ({item.Dish?.Name}): {item.Status} → SentToKitchen");
                    item.Status = OrderItemStatus.SentToKitchen;
                    item.UpdatedAt = DateTime.Now;
                }
            }
            else if (newStatus == OrderStatus.Ready)
            {
                // Chuyển các item Preparing sang Ready
                var itemsToUpdate = order.OrderItems.Where(oi => oi.Status == OrderItemStatus.Preparing).ToList();
                Console.WriteLine($"[ORDER] Ready: Updating {itemsToUpdate.Count} items to Ready");
                
                foreach (var item in itemsToUpdate)
                {
                    item.Status = OrderItemStatus.Ready;
                    item.UpdatedAt = DateTime.Now;
                }
            }
            else if (newStatus == OrderStatus.Served)
            {
                // Nếu chuyển sang Served, cập nhật thời gian phục vụ
                order.ServedAt = DateTime.Now;
                
                // Chuyển các item Ready sang Served
                var itemsToUpdate = order.OrderItems.Where(oi => oi.Status == OrderItemStatus.Ready).ToList();
                Console.WriteLine($"[ORDER] Served: Updating {itemsToUpdate.Count} items to Served");
                
                foreach (var item in itemsToUpdate)
                {
                    item.Status = OrderItemStatus.Served;
                    item.UpdatedAt = DateTime.Now;
                }
            }
            else if (newStatus == OrderStatus.Cancelled)
            {
                // Hủy toàn bộ items
                var itemsToUpdate = order.OrderItems.Where(oi => oi.Status != OrderItemStatus.Served && oi.Status != OrderItemStatus.Cancelled).ToList();
                Console.WriteLine($"[ORDER] Cancelled: Cancelling {itemsToUpdate.Count} items");
                
                foreach (var item in itemsToUpdate)
                {
                    item.Status = OrderItemStatus.Cancelled;
                    item.UpdatedAt = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine($"[ORDER] Status updated successfully");

            // Thông báo cập nhật báo cáo realtime
            await _reportUpdateService.NotifyRevenueUpdate($"Đơn hàng {order.OrderNumber} đã {GetStatusDisplayName(newStatus)}");
            if (newStatus == OrderStatus.Served)
            {
                await _reportUpdateService.NotifyTrendsUpdate("Cập nhật operational metrics");
            }

            TempData["SuccessMessage"] = $"Đã cập nhật trạng thái đơn hàng thành {GetStatusDisplayName(newStatus)}";
            return RedirectToAction("Details", new { id = orderId });
        }

        // GET: Order/GenerateQRPayment
        [HttpGet]
        public async Task<IActionResult> GenerateQRPayment(Guid orderId, decimal? amount = null)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Sử dụng amount truyền vào (đã giảm voucher) hoặc BalanceDue gốc
            var paymentAmount = amount ?? order.BalanceDue;
            
            Console.WriteLine($"[GenerateQRPayment] OrderId: {orderId}, BalanceDue: {order.BalanceDue}, Amount with voucher: {paymentAmount}");

            // Tạo QR code sử dụng VietQR API với số tiền đã giảm
            var qrData = new
            {
                orderId = order.OrderId,
                orderNumber = order.OrderNumber,
                amount = paymentAmount,
                bankCode = "970422", // MB Bank (có thể thay đổi)
                accountNumber = "0828959442", // Số tài khoản nhà hàng
                accountName = "TungDZ Restaurant",
                description = $"Thanh toan don {order.OrderNumber}",
                // qrUrl = $"https://img.vietqr.io/image/970422-0828959442-compact2.jpg?amount={paymentAmount}&addInfo={Uri.EscapeDataString($"Thanh toan don {order.OrderNumber}")}&accountName={Uri.EscapeDataString("TungDZ Restaurant")}"
                qrUrl = $"https://img.vietqr.io/image/970422-0828959442-compact2.jpg?amount=10000&addInfo={Uri.EscapeDataString($"Thanh toan don {order.OrderNumber}")}&accountName={Uri.EscapeDataString("TungDZ Restaurant")}"
            };

            return Json(qrData);
        }

        // POST: Order/CheckoutQR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutQR(Guid orderId, string? voucherCode, decimal discountAmount = 0)
        {
            try
            {
                Console.WriteLine($"[CheckoutQR] Starting payment for order: {orderId}");
                Console.WriteLine($"[CheckoutQR] Voucher code: {voucherCode}, Discount: {discountAmount}");

                var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    Console.WriteLine($"[CheckoutQR] Order not found: {orderId}");
                    return NotFound();
                }

                Console.WriteLine($"[CheckoutQR] Order found: {order.OrderNumber}, Balance: {order.BalanceDue}");

                // Calculate final amount after discount
                var finalAmount = order.BalanceDue - discountAmount;
                if (finalAmount < 0) finalAmount = 0;

                Console.WriteLine($"[CheckoutQR] Original: {order.BalanceDue}, Discount: {discountAmount}, Final: {finalAmount}");

                // Tạo thanh toán
                var staffId = await GetCurrentStaffIdAsync();
                Console.WriteLine($"[CheckoutQR] Current StaffId: {staffId} (IsEmpty: {staffId == Guid.Empty})");

                var paymentNotes = "Thanh toán qua QR Code";
                if (!string.IsNullOrEmpty(voucherCode))
                {
                    paymentNotes += $" - Voucher: {voucherCode} (Giảm {discountAmount:N0} VND)";
                }
                


                var payment = new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    OrderId = orderId,
                    Amount = finalAmount,
                    PaymentMethod = PaymentMethod.Qr,
                    PaymentDate = DateTime.Now,
                    Status = PaymentStatus.Approved,
                    ProcessedByStaffId = staffId != Guid.Empty ? (Guid?)staffId : null,
                };

                // Cập nhật đơn hàng
                order.PaidAmount += finalAmount;
                order.BalanceDue = 0;
                order.PaymentStatus = PaymentStatus.Approved;
                order.Status = OrderStatus.Paid;
                order.ServedAt = order.ServedAt ?? DateTime.Now;

                // Chỉ set ClosedByStaffId nếu có StaffId hợp lệ (tránh FK constraint)
                if (staffId != Guid.Empty)
                {
                    order.ClosedByStaffId = staffId;
                }
                // Nếu không có staffId, để null (nếu field nullable) hoặc không set

                order.UpdatedAt = DateTime.Now;

                // Cập nhật trạng thái bàn
                if (order.TableId.HasValue)
                {
                    var table = await _context.RestaurantTables.FindAsync(order.TableId.Value);
                    if (table != null)
                    {
                        table.Status = TableStatus.Available;
                        table.UpdatedAt = DateTime.Now;
                    }
                }

                _context.Payments.Add(payment);
                Console.WriteLine($"[CheckoutQR] Payment created: {payment.PaymentId}, Amount: {payment.Amount}");

                await _context.SaveChangesAsync();
                Console.WriteLine($"[CheckoutQR] Database saved successfully!");
                Console.WriteLine($"[CheckoutQR] Order status: {order.Status}, Table status: {(order.TableId.HasValue ? "Updated to Available" : "No table")}");

                // Thông báo cập nhật báo cáo realtime
                await _reportUpdateService.NotifyRevenueUpdate($"Thanh toán QR cho đơn {order.OrderNumber}");

                // Xử lý tích điểm nếu thanh toán thành công
                if (order.PaymentStatus == PaymentStatus.Approved && order.CustomerId.HasValue)
                {
                    await _loyaltyService.ProcessPaymentLoyaltyAsync(orderId, finalAmount);
                    await _reportUpdateService.NotifyCustomerUpdate("Cập nhật loyalty points");
                }

                return Json(new
                {
                    success = true,
                    message = "Thanh toán thành công!",
                    orderId = order.OrderId,
                    orderNumber = order.OrderNumber
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CheckoutQR] ERROR: {ex.Message}");
                Console.WriteLine($"[CheckoutQR] Stack trace: {ex.StackTrace}");
                return Json(new
                {
                    success = false,
                    message = $"Lỗi thanh toán: {ex.Message}"
                });
            }
        }
        

        [HttpGet]
        public async Task<IActionResult> CheckPaymentStatus(Guid orderId)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Json(new { paymentStatus = order.PaymentStatus.ToString() });
        }

        private string GetStatusDisplayName(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Open => "Mới tạo",
                OrderStatus.Confirmed => "Đã xác nhận",
                OrderStatus.InPreparation => "Đang chuẩn bị",
                OrderStatus.Ready => "Sẵn sàng",
                OrderStatus.Served => "Đã phục vụ",
                OrderStatus.Paid => "Đã thanh toán",
                OrderStatus.Cancelled => "Đã hủy",
                OrderStatus.Refunded => "Đã hoàn tiền",
                _ => status.ToString()
            };
        }

        // API: Lấy giá hiện tại của các món ăn
        [HttpGet]
        public async Task<IActionResult> GetDishPrices([FromQuery] string dishIds)
        {
            if (string.IsNullOrEmpty(dishIds))
                return Json(new { });

            var ids = dishIds.Split(',')
                .Where(id => Guid.TryParse(id.Trim(), out _))
                .Select(id => Guid.Parse(id.Trim()))
                .ToList();

            if (!ids.Any())
                return Json(new { });

            var prices = await (from d in _context.Dishes.AsNoTracking()
                               where ids.Contains(d.DishId)
                               select new { d.DishId, d.DefaultServingPrice })
                         .ToDictionaryAsync(x => x.DishId.ToString(), x => x.DefaultServingPrice);

            return Json(prices);
        }
    }

    public class OrderCreateViewModel
    {
        public Guid? TableId { get; set; }
        public Guid? CustomerId { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }

    public class OrderItemViewModel
    {
        public Guid DishId { get; set; }
        public decimal Quantity { get; set; } = 1;
        public string? SpecialInstructions { get; set; }
    }
}