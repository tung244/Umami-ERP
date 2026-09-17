using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using QuanLiKhoHang.Hubs;
using QuanLiKhoHang.Extensions;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace QuanLiKhoHang.Controllers
{
    /// <summary>
    /// Controller cho khách hàng gọi món tự phục vụ qua QR code
    /// </summary>
    public class PublicOrderController : Controller
    {
        private const string CartSessionKey = "PUBLIC_ORDER_CART";
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DishHub> _hubContext;

        public PublicOrderController(ApplicationDbContext context, IHubContext<DishHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Hiển thị giao diện gọi món cho bàn cụ thể
        /// Route: /PublicOrder/Table/{tableId}
        /// </summary>
        [HttpGet("PublicOrder/Table/{tableId}")]
        public IActionResult Index(Guid tableId)
        {
            return RedirectToAction("Notes", "Dish", new { tableId });
        }

        /// <summary>
        /// Thêm món vào giỏ hàng theo session khách
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid dishId, int quantity = 1)
        {
            if (quantity <= 0)
                return Json(new { success = false, message = "Số lượng không hợp lệ" });

            var dishExists = await _context.Dishes
                .AsNoTracking()
                .AnyAsync(d => d.DishId == dishId && d.Active);

            if (!dishExists)
                return Json(new { success = false, message = "Món ăn không còn khả dụng" });

            var cart = GetCartFromSession();
            cart[dishId] = cart.ContainsKey(dishId) ? cart[dishId] + quantity : quantity;
            SaveCartToSession(cart);

            return Json(new { success = true, cartCount = cart.Sum(x => x.Value) });
        }

        /// <summary>
        /// Xóa món khỏi giỏ hàng
        /// </summary>
        [HttpPost]
        public IActionResult RemoveFromCart(Guid dishId)
        {
            var cart = GetCartFromSession();

            if (cart.ContainsKey(dishId))
            {
                cart.Remove(dishId);
                SaveCartToSession(cart);
            }

            return Json(new { success = true, cartCount = cart.Sum(x => x.Value) });
        }

        /// <summary>
        /// Cập nhật số lượng món trong giỏ hàng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(Guid dishId, int quantity)
        {
            var cart = GetCartFromSession();

            if (quantity <= 0)
            {
                cart.Remove(dishId);
                SaveCartToSession(cart);
                return Json(new { success = true, cartCount = cart.Sum(x => x.Value) });
            }

            var dishExists = await _context.Dishes
                .AsNoTracking()
                .AnyAsync(d => d.DishId == dishId && d.Active);

            if (!dishExists)
            {
                cart.Remove(dishId);
                SaveCartToSession(cart);
                return Json(new { success = false, cartCount = cart.Sum(x => x.Value), message = "Món ăn không còn khả dụng" });
            }

            cart[dishId] = quantity;
            SaveCartToSession(cart);

            return Json(new { success = true, cartCount = cart.Sum(x => x.Value) });
        }

        /// <summary>
        /// Lấy thông tin giỏ hàng để hiển thị bên client
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CartSummary()
        {
            var cart = GetCartFromSession();
            if (!cart.Any())
            {
                return Json(new { success = true, items = Array.Empty<object>(), total = 0m });
            }

            var dishIds = cart.Keys.ToList();
            var dishes = await _context.Dishes
                .Where(d => dishIds.Contains(d.DishId))
                .Select(d => new { d.DishId, d.Name, d.DefaultServingPrice })
                .ToListAsync();

            var items = dishes
                .Select(d => new
                {
                    dishId = d.DishId,
                    name = d.Name,
                    quantity = cart.ContainsKey(d.DishId) ? cart[d.DishId] : 0,
                    unitPrice = d.DefaultServingPrice,
                    total = d.DefaultServingPrice * (cart.ContainsKey(d.DishId) ? cart[d.DishId] : 0)
                })
                .Where(x => x.quantity > 0)
                .ToList();

            var totalAmount = items.Sum(x => x.total);

            return Json(new { success = true, items, total = totalAmount });
        }

        /// <summary>
        /// Trang checkout xác nhận đơn hàng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Checkout(Guid tableId)
        {
            var table = await _context.RestaurantTables
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TableId == tableId && t.Status != TableStatus.OutOfService);

            if (table == null)
                return NotFound();

            var cart = GetCartFromSession();
            if (!cart.Any())
                return RedirectToAction("Index", new { tableId });

            var dishIds = cart.Keys.ToList();
            var dishes = await _context.Dishes
                .Where(d => dishIds.Contains(d.DishId))
                .ToDictionaryAsync(d => d.DishId);

            var cartItems = cart
                .Where(kvp => dishes.ContainsKey(kvp.Key))
                .Select(kvp => new
                {
                    Dish = dishes[kvp.Key],
                    Quantity = kvp.Value,
                    Total = dishes[kvp.Key].DefaultServingPrice * kvp.Value
                })
                .ToList();

            var totalAmount = cartItems.Sum(x => x.Total);

            ViewBag.Table = table;
            ViewBag.CartItems = cartItems;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.Title = "Xác nhận đơn hàng";

            return View();
        }

        /// <summary>
        /// Xác nhận và tạo đơn hàng public
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(Guid tableId, string customerPhone, string? customerName)
        {
            var table = await _context.RestaurantTables
                .FirstOrDefaultAsync(t => t.TableId == tableId && t.Status != TableStatus.OutOfService);

            if (table == null)
                return Json(new { success = false, message = "Bàn không hợp lệ" });

            var cart = GetCartFromSession();
            if (!cart.Any())
                return Json(new { success = false, message = "Giỏ hàng trống" });

            if (string.IsNullOrWhiteSpace(customerPhone))
                return Json(new { success = false, message = "Vui lòng nhập số điện thoại" });

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == customerPhone);

            if (customer == null)
            {
                customer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    FirstName = customerName ?? $"Khách {customerPhone}",
                    PhoneNumber = customerPhone,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                _context.Customers.Add(customer);
            }

            var staffId = await GetDefaultStaffId(table.BranchId);
            if (staffId == Guid.Empty)
                return Json(new { success = false, message = "Không tìm thấy nhân viên phụ trách chi nhánh." });

            var now = DateTime.Now;
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                OrderNumber = GenerateOrderNumber(),
                BranchId = table.BranchId,
                TableId = tableId,
                CustomerId = customer.CustomerId,
                CreatedByStaffId = staffId,
                OrderType = OrderType.DineIn,
                OrderSource = OrderSource.Web,
                PlacedAt = now,
                Status = OrderStatus.Open,
                SubTotal = 0m,
                DiscountAmount = 0m,
                TaxAmount = 0m,
                ServiceChargeAmount = 0m,
                RoundingAdjustment = 0m,
                TotalAmount = 0m,
                PaidAmount = 0m,
                BalanceDue = 0m,
                PaymentStatus = PaymentStatus.Pending,
                IsVoided = false,
                CreatedAt = now
            };

            var dishIds = cart.Keys.ToList();
            var dishes = await _context.Dishes
                .Where(d => dishIds.Contains(d.DishId))
                .Include(d => d.Category)
                .ToDictionaryAsync(d => d.DishId);

            foreach (var kvp in cart)
            {
                if (!dishes.ContainsKey(kvp.Key))
                    continue;

                var dish = dishes[kvp.Key];
                var quantity = kvp.Value;
                var (menuPriceId, unitPrice) = await GetCurrentDishPrice(dish.DishId, dish.DefaultServingPrice);

                if (menuPriceId == Guid.Empty)
                    return Json(new { success = false, message = $"Món {dish.Name} chưa có giá bán." });

                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    OrderId = orderId,
                    DishId = dish.DishId,
                    MenuPriceId = menuPriceId,
                    KitchenSectionId = dish.KitchenSectionId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    LineTotal = unitPrice * quantity,
                    Status = OrderItemStatus.Pending,
                    RequestedAt = now,
                    IsDiscounted = false,
                    DiscountAmount = 0m,
                    CreatedAt = now
                };

                order.OrderItems.Add(orderItem);
            }

            if (!order.OrderItems.Any())
                return Json(new { success = false, message = "Không thể tạo đơn vì các món đã bị ẩn." });

            order.SubTotal = order.OrderItems.Sum(oi => oi.LineTotal);
            order.TotalAmount = order.SubTotal;
            order.BalanceDue = order.TotalAmount;

            if (table.Status == TableStatus.Available)
            {
                table.Status = TableStatus.Occupied;
                table.UpdatedAt = now;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartSessionKey);

            foreach (var item in order.OrderItems)
            {
                var pendingQuantity = await GetPendingQuantityForDish(item.DishId);
                await _hubContext.SendDishPreparationUpdate(item.DishId, pendingQuantity);
            }

            await _hubContext.SendKitchenTicketAdded(new
            {
                orderId = order.OrderId,
                orderNumber = order.OrderNumber,
                tableName = table.Name,
                placedAt = order.PlacedAt.ToString("O"),
                items = order.OrderItems.Select(oi => new
                {
                    orderItemId = oi.OrderItemId,
                    dishId = oi.DishId,
                    dishName = dishes.ContainsKey(oi.DishId) ? dishes[oi.DishId].Name : string.Empty,
                    quantity = oi.Quantity,
                    status = oi.Status.ToString()
                })
            });

            // Broadcast trạng thái bàn đã thay đổi
            await _hubContext.SendTableStatusChanged(table.TableId, table.Status.ToString());

            return Json(new
            {
                success = true,
                message = $"Đơn hàng #{order.OrderNumber} đã được tạo thành công!",
                orderId = order.OrderId
            });
        }

        /// <summary>
        /// Lấy giỏ hàng từ session
        /// </summary>
        private Dictionary<Guid, int> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
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

        /// <summary>
        /// Lưu giỏ hàng vào session
        /// </summary>
        private void SaveCartToSession(Dictionary<Guid, int> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        /// <summary>
        /// Sinh số đơn hàng định dạng cố định
        /// </summary>
        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private async Task<decimal> GetPendingQuantityForDish(Guid dishId)
        {
            return await _context.OrderItems
                .Where(oi => oi.DishId == dishId && (oi.Status == OrderItemStatus.Pending || oi.Status == OrderItemStatus.Preparing))
                .SumAsync(oi => oi.Quantity);
        }

        private async Task<(Guid MenuPriceId, decimal Price)> GetCurrentDishPrice(Guid dishId, decimal fallbackPrice)
        {
            var currentPrice = await (from mp in _context.MenuPriceHistory.AsNoTracking()
                                      where mp.DishId == dishId &&
                                            mp.EffectiveFrom <= DateTime.Now &&
                                            (mp.EffectiveTo == null || mp.EffectiveTo >= DateTime.Now)
                                      orderby mp.EffectiveFrom descending
                                      select new { mp.MenuPriceId, mp.Price })
                                     .FirstOrDefaultAsync();

            if (currentPrice != null)
                return (currentPrice.MenuPriceId, currentPrice.Price);

            var newPrice = new MenuPriceHistory
            {
                MenuPriceId = Guid.NewGuid(),
                DishId = dishId,
                Price = fallbackPrice,
                EffectiveFrom = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.MenuPriceHistory.Add(newPrice);
            return (newPrice.MenuPriceId, newPrice.Price);
        }

        private async Task<Guid> GetDefaultStaffId(Guid branchId)
        {
            var staffId = await (from s in _context.Staff.AsNoTracking()
                                 where s.BranchId == branchId && s.IsActive
                                 orderby s.CreatedAt
                                 select s.StaffId)
                                .FirstOrDefaultAsync();

            if (staffId != Guid.Empty)
                return staffId;

            return await (from s in _context.Staff.AsNoTracking()
                          where s.IsActive
                          orderby s.CreatedAt
                          select s.StaffId)
                          .FirstOrDefaultAsync();
        }
    }
}