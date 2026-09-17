# 📊 Hướng dẫn Module Phân tích & Báo cáo (Analytics & Reporting)

## A. Doanh thu & Lợi nhuận (Revenue & Profit)

### 1. Tổng quan doanh thu theo thời gian (REP-01)

**Loại biểu đồ:** Line chart (time series) + area under curve, có tùy chọn smoothing; kèm KPI cards (TotalRevenue, OrdersCount, AOV).

**Insight:** xu hướng doanh thu, so sánh kỳ hiện tại với kỳ trước (MoM) và cùng kỳ năm trước (YoY).

**Data source + logic:**  
- Bảng Orders (status = Paid): order_date, total_amount, discount, tax, payments.  
- Bảng DailyRevenue (pre-aggregate): date, branch_id, total_revenue, orders_count, refunds, gross_revenue.  
- Query: `SUM(total_amount) GROUP BY date`.  
- So sánh kỳ: join với date range dịch chuyển (shifted date ranges).

**Công thức:**  
`NetRevenue = SUM(Order.TotalPaid) - SUM(RefundAmount)`

**Filters:** branch, POS/online, payment method, customer tier, category.

**Performance:** sử dụng pre-aggregated `DailyRevenue`, cập nhật ban đêm + incremental cập nhật khi có đơn mới trong ngày.

---

### 2. Lợi nhuận (REP-04)

**Loại biểu đồ:** Combo chart (bar = revenue, line = profit margin) hoặc stacked chart (Revenue / COGS / Opex).

**Insight:** theo dõi Gross Profit và Net Profit, margin theo ngày/tháng; phát hiện món/nhóm gây lỗ.

**Data source + logic:**  
- `COGS` = ∑ (OrderDetail.Quantity × DishRecipeIngredient.Quantity × Ingredient.CostPerUnit).  
- Nếu có nhiều lô nhập: chọn phương pháp Weighted Average Cost hoặc FIFO tùy theo yêu cầu kế toán.  
- `Profit = Revenue - COGS - OperationalCosts`  
- `OperationalCosts` lấy từ bảng `Expenses`, phân bổ theo rule (fixed% hoặc mapping category).

**Pre-aggregation:** `DailyProfitByBranch`, `DishProfitDaily`.

**UX:** Tooltip hiển thị breakdown (Revenue, COGS, GrossProfit, Opex).

---

### 3. Top món theo doanh thu & lợi nhuận (REP-02)

**Loại biểu đồ:** Horizontal bar chart (Top 10) + bảng chi tiết.

**Insight:** món bán chạy (volume), món mang lại doanh thu nhiều, món có margin cao/thấp.

**Data source + logic:**  
- `OrderDetail` JOIN `Dish` GROUP BY DishId  
- Tính: `SUM(quantity)`, `SUM(quantity * price)` → `revenue_by_dish`.  
- `Lợi nhuận theo món = revenue_by_dish - ingredient_cost_calculated (theo recipe)`.

**Notes:**  
- 2 phân loại: theo quantity và theo profit.  
- Cho phép drill-down theo cửa hàng, giờ, hoặc chiến dịch.

---

## B. Báo cáo Khách hàng

### 1. Tổng số khách / đăng ký / active users

**Loại:** KPI cards + time-series.  
**Metrics:** NewRegistrations, ActiveCustomers (trong kỳ), ReturningRate = #returning / #unique_customers.  
**Logic:** từ bảng `Customers` và `Orders` (đếm distinct customer_id).

---

### 2. Trung bình chi tiêu mỗi khách (AOV, ARPU) & Top customers

**Loại:** Table (Top 10) + Bar chart (Top N).  
**Logic:**  
- `CustomerTotalSpent = SUM(Order.TotalPaid)`  
- `OrderCount = COUNT(Order)`  
- `AvgPerOrder = CustomerTotalSpent / OrderCount`  
- `CLV = AOV × purchase_frequency × expected_lifetime (configurable)`

**Filters/UX:** lọc theo customer tier (Bronze/Silver/Gold), branch, timeframe.

---

### 3. Cohort & Retention

**Loại:** Cohort heatmap (matrix) + Retention line chart.  
**Mục đích:** đo tỉ lệ quay lại theo cohort (theo tháng đăng ký).  
**Logic:** nhóm theo `registration_date`, tính % khách trong cohort quay lại mua hàng trong các kỳ tiếp theo.

---

## C. Kho & Nguyên liệu (Inventory & Purchases)

### 1. Tồn kho hiện tại & Turnover

**Loại:** Table + sparkline cho từng item.  
**Metrics:** OnHandQty, DaysOfStock = OnHandQty / AverageDailyUsage.  
**Logic:**  
- OnHand lấy từ `Inventory`.  
- Usage tính từ `OrderDetail` thông qua `Dish → Recipe → Ingredient`.

---

### 2. Nhập kho & chi phí nhập

**Loại:** Bar chart (chi phí nhập theo tháng) + bảng nhà cung cấp.  
**Data:** `ImportInvoice` / `PurchaseOrder` → `SUM(total)` GROUP BY supplier, item.

---

### 3. Cảnh báo low-stock & wastage

**Loại:** Alert list + bảng chi tiết.  
**Logic:**  
- Low stock: `OnHandQty <= ReorderPoint`.  
- Wastage: tính từ `StockOut` có reason = spoilage.

---

## D. Công nợ & Nhà cung cấp

**Loại:** Aging report (stacked bars 0-30/31-60/61-90/90+), bảng chi tiết có trạng thái và hành động.  
**Logic:**  
- Từ `SupplierInvoice`: DueDate, PaidAmount → Outstanding = Total - PaidAmount.  
- `DaysOverdue = DATEDIFF(day, DueDate, GETDATE())`.  
**Features:** cảnh báo quá hạn, liên kết đến lịch sử giao dịch.

---

## E. Hiệu suất & Vận hành

### 1. Orders theo giờ — Peak hours

**Loại:** Heatmap (day-of-week × hour-of-day) hoặc area chart orders-per-hour.  
**Logic:**  
`GROUP BY DATEPART(hour, OrderDate), DATEPART(weekday, OrderDate)`  
→ COUNT(orders), SUM(revenue).  
**Output:** danh sách khung giờ cao điểm (VD: 11:30–13:00, 18:00–20:00) theo chi nhánh.

---

### 2. Table turnover, Fulfillment time, Service time

**Loại:** Boxplot / histogram (phân phối thời gian) + KPI cards.  
**Logic:**  
- `FulfillmentTime = TimePrepared - TimeOrderCreated`.  
- `TableTurnover = #covers / open-hours`.

---

### 3. Staff performance

**Loại:** Leaderboard (Orders handled, average ticket size, avg fulfillment time).  
**Logic:**  
- Dựa theo `Order.StaffId` và `OrderDetail` để tính tổng đơn, tổng giá trị và thời gian trung bình xử lý.

---

## F. Phân tích xu hướng (Seasonality & Campaign Impact)

**Loại:** Decomposition chart (Trend + Seasonality) + YoY comparison.  
**Logic:**  
- Áp dụng time-series decomposition (weekly, monthly) để phát hiện seasonality.  
- Tính `percent lift` sau khuyến mãi bằng cách so sánh với baseline period.  
- Cho phép xem theo chiến dịch hoặc nhóm món.