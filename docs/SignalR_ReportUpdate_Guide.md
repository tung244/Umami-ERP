# Hướng dẫn sử dụng ReportUpdateService với SignalR

## Tổng quan
ReportUpdateService cho phép gửi thông báo realtime đến các trang báo cáo khi có thay đổi dữ liệu trong hệ thống.

## Cách sử dụng

### 1. Inject service vào controller

```csharp
using QuanLiKhoHang.Services;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IReportUpdateService _reportUpdateService;

    public OrderController(
        ApplicationDbContext context,
        IReportUpdateService reportUpdateService)
    {
        _context = context;
        _reportUpdateService = reportUpdateService;
    }
}
```

### 2. Gọi service khi có thay đổi dữ liệu

#### Khi tạo Order mới hoặc Payment
```csharp
[HttpPost]
public async Task<IActionResult> Create(Order order)
{
    _context.Orders.Add(order);
    await _context.SaveChangesAsync();
    
    // Thông báo cập nhật Revenue report
    await _reportUpdateService.NotifyRevenueUpdate("Có đơn hàng mới");
    
    // Thông báo cập nhật Trends report (peak hours)
    await _reportUpdateService.NotifyTrendsUpdate("Có đơn hàng mới");
    
    return Ok();
}
```

#### Khi Customer mới hoặc Tier change
```csharp
[HttpPost]
public async Task<IActionResult> UpdateTier(Guid customerId, Tier newTier)
{
    var customer = await _context.Customers.FindAsync(customerId);
    customer.Tier = newTier;
    await _context.SaveChangesAsync();
    
    // Thông báo cập nhật Customer report
    await _reportUpdateService.NotifyCustomerUpdate($"Khách hàng {customer.FullName} lên hạng {newTier}");
    
    return Ok();
}
```

#### Khi có Purchase Order mới
```csharp
[HttpPost]
public async Task<IActionResult> CreatePurchaseOrder(PurchaseOrder po)
{
    _context.PurchaseOrders.Add(po);
    await _context.SaveChangesAsync();
    
    // Thông báo cập nhật Inventory report
    await _reportUpdateService.NotifyInventoryUpdate("Có đơn nhập hàng mới");
    
    return Ok();
}
```

#### Cập nhật tất cả báo cáo cùng lúc
```csharp
[HttpPost]
public async Task<IActionResult> CompleteOrder(Guid orderId)
{
    var order = await _context.Orders.FindAsync(orderId);
    order.Status = OrderStatus.Completed;
    await _context.SaveChangesAsync();
    
    // Thông báo cập nhật tất cả báo cáo
    await _reportUpdateService.NotifyAllReportsUpdate("Đơn hàng hoàn thành");
    
    return Ok();
}
```

## Các method có sẵn

| Method | Mô tả | Khi nào gọi |
|--------|-------|-------------|
| `NotifyRevenueUpdate()` | Cập nhật báo cáo doanh thu | Order mới, Payment, Order status change |
| `NotifyCustomerUpdate()` | Cập nhật báo cáo khách hàng | Customer mới, Tier change, Loyalty points |
| `NotifyInventoryUpdate()` | Cập nhật báo cáo kho | Purchase Order, Stock Transaction |
| `NotifyTrendsUpdate()` | Cập nhật phân tích xu hướng | Order mới (peak hours), Reservation |
| `NotifyAllReportsUpdate()` | Cập nhật tất cả báo cáo | Thay đổi lớn ảnh hưởng nhiều báo cáo |

## Các trang đã tích hợp SignalR

1. **Revenue.cshtml** - Báo cáo kinh doanh
   - Lắng nghe: `RevenueUpdated`, `AllReportsUpdated`
   - Auto-refresh: KPI cards, charts (revenue, orders, top dishes, payment methods, categories)

2. **Customer.cshtml** - Báo cáo khách hàng
   - Lắng nghe: `CustomerUpdated`, `AllReportsUpdated`
   - Auto-refresh: Customer metrics, top customers, tier distribution

3. **Inventory.cshtml** - Báo cáo kho
   - Lắng nghe: `InventoryUpdated`, `AllReportsUpdated`
   - Auto-refresh: Top purchased items, purchase analysis

4. **Trends.cshtml** - Phân tích xu hướng
   - Lắng nghe: `TrendsUpdated`, `AllReportsUpdated`
   - Auto-refresh: Peak hours heatmap, operational metrics, weekly/monthly trends

## Ví dụ triển khai trong OrderController

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Services;

namespace QuanLiKhoHang.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportUpdateService _reportUpdateService;

        public OrderController(
            ApplicationDbContext context,
            IReportUpdateService reportUpdateService)
        {
            _context = context;
            _reportUpdateService = reportUpdateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            try
            {
                order.OrderId = Guid.NewGuid();
                order.PlacedAt = DateTime.Now;
                order.Status = OrderStatus.Pending;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Gửi thông báo cập nhật báo cáo
                await _reportUpdateService.NotifyRevenueUpdate($"Đơn hàng mới #{order.OrderId}");
                await _reportUpdateService.NotifyTrendsUpdate("Cập nhật peak hours");

                return Json(new { success = true, orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid orderId, OrderStatus status)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    return NotFound();

                order.Status = status;
                if (status == OrderStatus.Completed)
                {
                    order.ServedAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                // Gửi thông báo
                await _reportUpdateService.NotifyRevenueUpdate($"Đơn hàng #{orderId} đã {status}");
                if (status == OrderStatus.Completed)
                {
                    await _reportUpdateService.NotifyTrendsUpdate("Cập nhật operational metrics");
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
```

## Lưu ý

1. **Performance**: Service sử dụng `Clients.All`, có thể tối ưu bằng cách sử dụng groups nếu cần
2. **Error handling**: Đã có retry logic (5s) nếu connection bị lỗi
3. **Cleanup**: Client tự động disconnect khi rời trang
4. **Message**: Có thể truyền message tùy chỉnh hoặc để mặc định

## Testing

Để test SignalR realtime:
1. Mở trang báo cáo (ví dụ: Revenue)
2. Mở Console (F12) để xem logs
3. Thực hiện thao tác tạo Order/Payment từ tab khác
4. Kiểm tra Console: `"Revenue updated: ..."` và chart tự động refresh

## Troubleshooting

- **Không nhận được updates**: Kiểm tra Console có lỗi SignalR connection
- **Charts không refresh**: Kiểm tra event listener đã đăng ký đúng event name
- **Connection failed**: Kiểm tra `/reportHub` endpoint đã được map trong Program.cs
