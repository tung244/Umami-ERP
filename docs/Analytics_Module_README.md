# Module Báo cáo & Phân tích (Analytics & Reporting) - Version 2.0

## Tổng quan

Module này cung cấp dashboard phân tích toàn diện cho hệ thống quản lý nhà hàng, triển khai theo đặc tả chi tiết trong `Analytics_Guide.md`.

**Ngày triển khai:** 2025-11-16  
**Framework:** ASP.NET Core MVC 8.0  
**Charting Library:** ApexCharts.js v4.4.1  
**Database:** SQL Server với Entity Framework Core

---

## Cấu trúc Module

### 1. Controller

**File:** `Controllers/ReportController.cs` (940 lines)

**Endpoints chính:**

#### A. Doanh thu & Lợi nhuận
- `GET /Report/RevenueTimeSeries` - Doanh thu theo thời gian với MoM/YoY comparison
- `GET /Report/TopDishesAnalysis` - Top món theo revenue/quantity/profit/margin
- `GET /Report/ProfitAnalysis` - Phân tích lợi nhuận (Gross & Net) với COGS/Opex breakdown

#### B. Khách hàng
- `GET /Report/CustomerMetrics` - Tổng quan metrics khách hàng
- `GET /Report/CustomerValueAnalysis` - AOV, CLV và top customers
- `GET /Report/CohortRetention` - Phân tích retention theo cohort

#### C. Kho & Nguyên liệu
- `GET /Report/InventoryStatus` - Tồn kho, DaysOfStock, low-stock alerts
- `GET /Report/PurchaseAnalysis` - Nhập kho theo supplier

#### D. Công nợ
- `GET /Report/DebtAging` - Aging report (AR/AP) theo buckets 0-30/31-60/61-90/90+

#### E. Hiệu suất & Vận hành
- `GET /Report/PeakHoursAnalysis` - Heatmap orders theo day × hour
- `GET /Report/OperationalMetrics` - Fulfillment time & table turnover
- `GET /Report/StaffPerformance` - Leaderboard nhân viên

#### F. Xu hướng & Seasonality
- `GET /Report/SeasonalityAnalysis` - Weekly pattern & monthly trend

### 2. View

**File:** `Views/Report/Dashboard.cshtml` (1000+ lines)

**Layout:**
- Header với filters (date range, branch, comparison mode)
- 4 KPI summary cards
- 6 sections chính (A-F) với 15+ charts/components
- JavaScript integration với ApexCharts.js

### 3. Entities

**Đã có:**
- `ReportsCache` - Cache cho pre-aggregated reports

**Đang sử dụng:**
- `Order`, `OrderItem`, `Customer`, `Payment`, `Refund`
- `Dish`, `Category`, `InventoryItem`, `StockTransaction`
- `Staff`, `Expense`, `AccountsReceivable`, `AccountsPayable`
- `LoyaltyAccount`, `PurchaseOrder`, `Supplier`

---

## Cách sử dụng

### 1. Truy cập Dashboard

```
URL: /Report/Dashboard
Authorization: [Authorize] - Yêu cầu đăng nhập
```

### 2. Filters

- **Date Range:** Chọn startDate & endDate
- **Branch:** Lọc theo chi nhánh (optional)
- **Compare With:** none/mom/yoy

### 3. Làm mới dữ liệu

Click nút "Làm mới" hoặc thay đổi filters sẽ tự động load lại tất cả charts.

---

## API Response Format

### Ví dụ: RevenueTimeSeries

```json
{
  "success": true,
  "period": {
    "start": "2025-10-17",
    "end": "2025-11-16"
  },
  "kpi": {
    "totalRevenue": 45000000,
    "netRevenue": 44500000,
    "totalRefunds": 500000,
    "totalOrders": 450,
    "aov": 100000
  },
  "comparison": {
    "period": "MoM",
    "previousRevenue": 40000000,
    "changePercent": 12.5
  },
  "data": [
    {
      "date": "2025-10-17",
      "revenue": 1500000,
      "orders": 15,
      "avgOrderValue": 100000
    },
    ...
  ]
}
```

### Ví dụ: TopDishesAnalysis

```json
{
  "success": true,
  "summary": {
    "totalRevenue": 30000000,
    "totalDishes": 45,
    "sortBy": "revenue"
  },
  "data": [
    {
      "dishId": "guid",
      "dishName": "Phở bò",
      "quantitySold": 120,
      "revenue": 3600000,
      "ingredientCost": 1260000,
      "profit": 2340000,
      "profitMargin": 65.0,
      "avgPrice": 30000
    },
    ...
  ]
}
```

---

## Chart Types

| Chart Type | Thư viện | Sử dụng cho |
|------------|----------|-------------|
| Area + Line | ApexCharts | Revenue time series |
| Horizontal Bar | ApexCharts | Top dishes, Purchase analysis |
| Combo (Bar + Line) | ApexCharts | Profit analysis |
| Bar | ApexCharts | Customer value, Staff performance |
| Heatmap | ApexCharts | Cohort retention, Peak hours |
| Donut | ApexCharts | Debt aging (AR/AP) |
| Line | ApexCharts | Monthly trend |

---

## Coding Conventions

### LINQ Query Syntax (REQUIRED)

```csharp
// ✅ ĐÚNG - Query syntax
var query = from o in _context.Orders.AsNoTracking()
            where o.PlacedAt >= start && o.PlacedAt < end
            select o;

// ❌ SAI - Method syntax
var query = _context.Orders.AsNoTracking()
    .Where(o => o.PlacedAt >= start && o.PlacedAt < end);
```

### ViewBag Typing

```csharp
// ✅ ĐÚNG - Typed ViewBag
ViewBag.Branches = _context.Branches.AsNoTracking().ToList(); // List<Branch>
ViewBag.TotalRevenue = 1000000; // int
ViewBag.Title = "Dashboard"; // string

// ❌ SAI - Dynamic hoặc DTO
ViewBag.Data = new { x = 1, y = 2 }; // ❌ anonymous object phức tạp
```

### Response Format

```csharp
// ✅ ĐÚNG - Consistent JSON structure
return Json(new
{
    success = true,
    summary = new { ... },
    data = items
});

// ❌ SAI - Inconsistent structure
return Json(items); // Thiếu metadata
```

---

## Performance Considerations

### Current Implementation

- **No caching**: Mỗi request đều query database trực tiếp
- **No pre-aggregation**: Tính toán real-time
- **COGS simplified**: 35-40% flat rate

### Recommended Improvements

1. **Caching Layer**
   ```csharp
   // Sử dụng ReportsCache entity
   var cached = await _context.ReportsCache
       .FirstOrDefaultAsync(r => r.ReportType == "Revenue" && r.Period == period);
   ```

2. **Pre-aggregation Tables**
   ```csharp
   // Tạo entities: DailyRevenue, DailyProfitByBranch, DishProfitDaily
   // Background job chạy mỗi đêm để tính toán
   ```

3. **Recipe-based COGS**
   ```csharp
   // Thay vì: revenue * 0.35m
   // Dùng: SUM(DishIngredient.Quantity × InventoryItem.CostPerUnit)
   ```

---

## Testing

### Manual Testing Checklist

- [ ] Revenue chart hiển thị đầy đủ ngày
- [ ] MoM/YoY comparison tính đúng
- [ ] Top dishes sắp xếp theo sortBy
- [ ] Cohort heatmap render màu đúng
- [ ] Peak hours heatmap hiển thị 7 days × 24 hours
- [ ] Debt aging buckets tính đúng
- [ ] Staff performance leaderboard đúng thứ tự
- [ ] Filters hoạt động trên tất cả charts
- [ ] Export CSV/PDF buttons (placeholder)

### Sample Data Requirements

Để test đầy đủ, cần:
- ≥ 30 ngày orders với đủ OrderStatus.Paid
- ≥ 10 dishes với OrderItems
- ≥ 5 customers với nhiều orders
- ≥ 3 branches
- Expenses data với nhiều categories
- AccountsReceivable/AccountsPayable với varied DueDate
- Staff với Orders.CreatedByStaffId

---

## Known Limitations

1. **COGS Calculation**: Dùng flat rate 35-40% thay vì tính từ recipe
2. **CLV Formula**: Simplified (totalSpent × 1.5) thay vì predictive model
3. **No Forecasting**: Chỉ hiển thị historical data
4. **No Drill-down**: Charts không có interactive drill-down
5. **Export**: CSV/PDF chỉ là placeholder

---

## Future Enhancements

### Phase 1 (High Priority)
- [ ] Recipe-based COGS calculation
- [ ] Real export functionality (CSV/PDF)
- [ ] Report caching layer
- [ ] Date range presets (Today/This week/This month/etc.)

### Phase 2 (Medium Priority)
- [ ] Pre-aggregation background jobs
- [ ] Advanced CLV prediction model
- [ ] Interactive chart drill-downs
- [ ] Custom date comparison (vs any period)

### Phase 3 (Low Priority)
- [ ] Forecasting & trend analysis
- [ ] Campaign impact tracking
- [ ] A/B testing reports
- [ ] Real-time dashboard với SignalR

---

## Troubleshooting

### Chart không hiển thị

**Nguyên nhân:** ApexCharts.js chưa load hoặc div container không đúng ID

**Giải pháp:**
```html
<!-- Kiểm tra _Layout.cshtml có ApexCharts.js -->
<script src="~/lib/apexcharts/apexcharts.min.js"></script>

<!-- Kiểm tra ID trong Dashboard.cshtml -->
<div id="revenueTimeSeriesChart"></div>
```

### API trả về empty data

**Nguyên nhân:** Date range không có orders hoặc OrderStatus filter sai

**Giải pháp:**
```csharp
// Kiểm tra filter trong controller
where o.Status == OrderStatus.Paid  // Đảm bảo enum đúng
```

### MoM/YoY comparison sai

**Nguyên nhân:** Timezone hoặc date arithmetic sai

**Giải pháp:**
```csharp
var prevStart = start.AddMonths(-1);  // Cho MoM
var prevStart = start.AddYears(-1);   // Cho YoY
// Đảm bảo dùng .Date để loại bỏ time component
```

---

## Contact & Support

**Triển khai bởi:** GitHub Copilot (Claude Sonnet 4.5)  
**Ngày:** 2025-11-16  
**Version:** 2.0

Để báo lỗi hoặc request features, vui lòng update `use-case-tracking.md` với status ⏳ và ghi chú chi tiết.
