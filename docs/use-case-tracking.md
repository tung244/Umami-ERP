# Theo Dõi Triển Khai Use Cases - Hệ Thống Quản Lý Nhà Hàng

## Tổng quan
File này theo dõi việc triển khai các use cases dựa trên đặc tả trong `restaurant_system_spec.md`. Mỗi use case được đánh dấu trạng thái triển khai và ghi chú implementation.

**Chú thích trạng thái:**
- ✅ **Đã triển khai**: Code hoàn chỉnh, test thành công
- 🔄 **Đang triển khai**: Code cơ bản có, cần hoàn thiện
- ❌ **Chưa triển khai**: Chưa có code
- ⏳ **Cần cải thiện**: Có code nhưng cần nâng cấp

---

## 1. POS – Quản lý bán hàng

| Mã | Chức năng | Trạng thái | Ghi chú |
|----|------------|------------|---------|
| POS-01 | Quản lý bàn ăn | ✅ | `TableController` hoàn chỉnh:<br>- **Xem danh sách bàn**: Với trạng thái, khu vực<br>- **Chi tiết bàn**: Lịch sử đơn hàng<br>- **Thêm/sửa bàn**: Form validation<br>- **Cập nhật trạng thái**: Tự động khi tạo đơn |
| POS-02 | Gọi món | ✅ | `OrderController.Create` và `AddItem`:<br>- **Tạo đơn hàng**: Chọn bàn, khách hàng<br>- **Thêm món**: Tìm kiếm, chọn số lượng<br>- **Tính tổng**: Tự động cập nhật |
| POS-02a | Trang chọn món cho khách | ✅ | `Dish/Notes.cshtml` hiển thị thực đơn theo danh mục, giỏ hàng session và điều hướng checkout; đồng bộ với `PublicOrderController` |
| POS-03 | Quản lý thứ tự món ăn | ✅ | `OrderController` + `Views/Order/Details.cshtml`:<br>- **Thay đổi số lượng**: +/- buttons với AJAX UpdateItemQuantity<br>- **Hủy món**: Delete button gọi RemoveItem<br>- **Hủy đơn**: "Hủy đơn" button với xác nhận, chỉ ở trạng thái Open/Confirmed<br>- **Cập nhật tự động**: Page reload sau thay đổi |
| POS-04 | Thông báo đến bếp | ✅ | `PublicOrderController.ConfirmOrder` phát tín hiệu qua `DishHub` (`KitchenTicketAdded`/`KitchenTicketUpdated`); `KitchenTicketController` nhận và đồng bộ real-time |
| POS-04a | Trang theo dõi bếp | ✅ | `KitchenTicketController` + `Views/KitchenTicket/Index` hiển thị món chờ nấu, lọc khu vực/trạng thái, cập nhật trạng thái qua AJAX & SignalR |
| POS-05 | Tính tiền & lập hóa đơn | ❌ | `InvoiceController` trống:<br>- **Tính tổng**: Thuế, giảm giá, phí<br>- **Tạo hóa đơn**: Lưu trữ chi tiết<br>- **Xuất hóa đơn**: PDF/thermal |
| POS-06 | Thanh toán | 🔄 | `OrderController.Checkout`:<br>- **Thanh toán tiền mặt**: Tính tiền thừa<br>- **Thẻ tín dụng**: Cần tích hợp cổng<br>- **Ví điện tử**: QR code |
| POS-07 | In hóa đơn | ❌ | Chưa có:<br>- **In thermal**: Kết nối máy in<br>- **In PDF**: Download file<br>- **Lưu trữ**: Lịch sử in |
| POS-08 | Lịch sử đơn hàng | ✅ | `OrderController.Index`:<br>- **Lọc theo trạng thái**: Pending, Paid, etc.<br>- **Tìm kiếm**: Theo số đơn, khách hàng<br>- **Chi tiết đơn**: Items, payments |
| POS-09 | Quản lý khuyến mãi | ❌ | Chưa triển khai: Áp dụng voucher khi thanh toán |
| POS-10 | Gọi món tự phục vụ | ✅ | `PublicOrderController` + `Dish/Notes.cshtml` hoàn chỉnh:<br>- **QR code**: Generate từ TableController<br>- **Luồng khách**: Chọn món, lọc danh mục, giỏ hàng session<br>- **Xác nhận đơn**: Checkout -> ConfirmOrder lưu Order/OrderItems<br>- **SignalR**: Đẩy số lượng món chờ, chờ dashboard bếp (POS-04a) |

---

## 2. Quản lý thực đơn (Menu Management)

| Mã | Chức năng | Trạng thái | Ghi chú |
|----|------------|------------|---------|
| MENU-01 | Quản lý danh mục món ăn | ✅ | `CategoryController` hoàn chỉnh với CRUD danh mục |
| MENU-02 | Thêm/Sửa/Xóa món ăn | ✅ | `DishController` hoàn chỉnh:<br>- **Tạo món mới**: Form với validation, upload ảnh<br>- **Chỉnh sửa món**: Update thông tin, giá, danh mục<br>- **Xóa món**: Kiểm tra ràng buộc trước khi xóa |
| MENU-02a | Hiển thị món cho khách | ✅ | `Dish/Notes.cshtml` hiển thị theo danh mục, trạng thái online, tích hợp giỏ hàng và hành động đặt món |
| MENU-03 | Quản lý giá | 🔄 | `MenuPriceHistory` entity có, cần UI quản lý lịch sử giá |
| MENU-04 | Ẩn/Hiện món | ✅ | `DishController.ToggleActive` với SignalR real-time updates |

---

## 3. Quản lý kho (Inventory Management)

| Mã | Chức năng | Trạng thái | Ghi chú |
|----|------------|------------|---------|
| INV-01 | Theo dõi tồn kho | ✅ | `InventoryController.Index`:<br>- **Danh sách mặt hàng**: Với số lượng hiện tại<br>- **Tìm kiếm**: Theo tên, SKU<br>- **Sắp xếp**: Theo mức tồn kho |
| INV-02 | Nhập kho | ✅ | `InventoryController.StockIn`:<br>- **Ghi nhận nhập**: Tăng số lượng<br>- **Tạo transaction**: Lưu lịch sử<br>- **Cập nhật giá**: Trung bình gia quyền |
| INV-03 | Xuất kho | ✅ | `InventoryController.StockOut`:<br>- **Kiểm tra tồn kho**: Không cho xuất quá<br>- **Tạo transaction**: Giảm số lượng<br>- **Lý do xuất**: Bán hàng, hỏng, etc. |
| INV-04 | Tự động trừ kho | ❌ | Chưa có:<br>- **Khi bán hàng**: Tự động trừ từ OrderItem<br>- **Rollback**: Khi hủy đơn |
| INV-05 | Cảnh báo hết hàng | ❌ | Chưa có:<br>- **Ngưỡng tối thiểu**: Cấu hình per item<br>- **Thông báo**: Email/SMS cho kho<br>- **Dashboard**: Highlight items low stock |
| INV-06 | Kiểm kê định kỳ | ❌ | Chưa có:<br>- **Form kiểm kê**: Nhập số lượng thực tế<br>- **Tạo adjustment**: Transaction type<br>- **Báo cáo chênh lệch**: Variance report |

---

## 4. Quản lý nhân viên (Staff Management)

| Mã | Chức năng | Trạng thái | Ghi chú |
|----|------------|------------|---------|
| HR-01 | Hồ sơ nhân viên | ✅ | `StaffController`:<br>- **Thông tin cá nhân**: Tên, email, phone<br>- **Thông tin công việc**: Chức vụ, lương, branch<br>- **Trạng thái**: Active/Inactive |
| HR-02 | Phân quyền | 🔄 | Cơ bản với roles:<br>- **Role-based access**: Admin, Manager, Staff<br>- **Claim-based**: BranchId, RoleId<br>- **Cần cải thiện**: Permission matrix |
| HR-03 | Lịch làm việc | ❌ | Chưa có `ShiftController`:<br>- **Tạo ca làm**: Thời gian, nhân viên<br>- **Phê duyệt**: Manager approval<br>- **Check-in/out**: QR code |
| HR-04 | Chấm công | 🔄 | `AttendanceApiController` (POST `/api/attendance/face`) ghi nhận check-in/out khuôn mặt, `AttendanceHub` phát sự kiện `AttendanceUpdated`, và các `Views/Attendance` cập nhật bằng SignalR nên có thể thử Postman trước khi triển khai phần IOT |
| HR-05 | Báo cáo công & lương | ❌ | Chưa có `PayrollController`:<br>- **Tính lương**: Base + overtime<br>- **Deductions**: Thuế, bảo hiểm<br>- **Payslip**: PDF generation |

---

## 5. Quản lý khách hàng (Customer Management)

| Mã | Chức năng | Trạng thái | Ghi chú |
|----|------------|------------|---------|
| CUST-01 | Hồ sơ khách hàng | ✅ | `CustomerController`:<br>- **Thông tin cá nhân**: Tên, phone, email<br>- **Lịch sử giao dịch**: Orders, payments<br>- **Tier**: Loyalty level |
| CUST-02 | Chương trình thân thiết | 🔄 | `LoyaltyController`:<br>- **Tích điểm**: Từ orders<br>- **Xếp hạng**: Bronze/Silver/Gold<br>- **Quyền lợi**: Discount per tier |
| CUST-03 | Giảm giá & Voucher | 🔄 | `VoucherController`:<br>- **Tạo voucher**: Code, discount, expiry<br>- **Áp dụng**: Trong order<br>- **Tracking**: Usage count |
| CUST-04 | Lịch sử mua hàng | ✅ | Tích hợp trong `CustomerController`:<br>- **Order history**: Chi tiết đơn hàng<br>- **Favorite dishes**: Analytics<br>- **Spending analysis**: Total spent |

---

## 6. Phân tích & Báo cáo (Analytics & Reporting) - VERSION 2.0

**Cập nhật ngày:** 2025-11-16  
**Triển khai theo:** `Analytics_Guide.md` - Advanced Analytics Module

### 6.1. Doanh thu & Lợi nhuận

| Mã | Báo cáo | Trạng thái | Ghi chú |
|----|----------|------------|---------|
| REP-01 | Doanh thu theo thời gian (v2) | ✅ | `ReportController.RevenueTimeSeries`:<br>- **Area + Line Chart**: Doanh thu & Số đơn theo ngày<br>- **KPI Cards**: Total Revenue, Net Revenue (after refunds), AOV<br>- **MoM/YoY Comparison**: So sánh với tháng/năm trước<br>- **Lọc**: Thời gian, chi nhánh, loại so sánh<br>- **Điền đầy ngày**: Hiển thị cả ngày 0 doanh thu |
| REP-02 | Top món ăn (v2) | ✅ | `ReportController.TopDishesAnalysis`:<br>- **Horizontal Bar Chart**: Doanh thu vs Lợi nhuận<br>- **Sắp xếp đa chiều**: Theo revenue/quantity/profit/margin<br>- **Phân tích COGS**: 35% simplified (cần recipe-based)<br>- **Metrics**: Quantity sold, Revenue, Profit, Profit margin %<br>- **Lọc**: Thời gian, danh mục, sortBy |
| REP-04 | Lợi nhuận & Chi phí (v2) | ✅ | `ReportController.ProfitAnalysis`:<br>- **Combo Chart**: Bars (Revenue/COGS/Opex) + Line (Margin %)<br>- **Group By**: Theo ngày hoặc tháng<br>- **Metrics**: Gross Profit, Net Profit, Gross/Net Margin<br>- **Expense Breakdown**: Top 5 categories chi phí<br>- **Lọc**: Thời gian, chi nhánh, groupBy |

### 6.2. Báo cáo Khách hàng

| Mã | Báo cáo | Trạng thái | Ghi chú |
|----|----------|------------|---------|
| CUST-RPT-01 | Customer Metrics | ✅ | `ReportController.CustomerMetrics`:<br>- **KPI Cards**: Total/New/Active/Returning customers<br>- **Retention Rate**: Tỷ lệ khách quay lại trong kỳ<br>- **Lọc**: Thời gian |
| CUST-RPT-02 | AOV & CLV Analysis | ✅ | `ReportController.CustomerValueAnalysis`:<br>- **Bar Chart**: CLV & Total Spent của top 10 khách<br>- **Metrics**: AOV, CLV (simplified: totalSpent × 1.5)<br>- **Lọc**: Thời gian, tier, limit<br>- **Chi tiết**: Order count, First/Last order date |
| CUST-RPT-03 | Cohort Retention | ✅ | `ReportController.CohortRetention`:<br>- **Heatmap Chart**: Retention % theo cohort tháng đăng ký<br>- **Matrix**: M0 đến M6+ retention rates<br>- **Color Scale**: Đỏ (0-20%), Cam (21-50%), Xanh (51-100%)<br>- **Tham số**: Số tháng phân tích |

### 6.3. Kho & Nguyên liệu

| Mã | Báo cáo | Trạng thái | Ghi chú |
|----|----------|------------|---------|
| INV-RPT-01 | Inventory Status | ✅ | `ReportController.InventoryStatus`:<br>- **Table**: Top low-stock items<br>- **Metrics**: Days of Stock, Avg Daily Usage, Stock Value<br>- **Cảnh báo**: needsReorder flag<br>- **Summary**: Total items, Low stock count, Total value |
| INV-RPT-02 | Purchase Analysis | ✅ | `ReportController.PurchaseAnalysis`:<br>- **Horizontal Bar Chart**: Nhập kho theo nhà cung cấp<br>- **Metrics**: Total amount, Order count, Avg order value<br>- **Timeline**: By month<br>- **Lọc**: Thời gian |

### 6.4. Công nợ

| Mã | Báo cáo | Trạng thái | Ghi chú |
|----|----------|------------|---------|
| DEBT-RPT-01 | Debt Aging Report | ✅ | `ReportController.DebtAging`:<br>- **Donut Charts**: AR & AP phân theo buckets<br>- **Aging Buckets**: Current, 0-30, 31-60, 61-90, 90+ days<br>- **Color Code**: Xanh (current) → Đỏ (90+)<br>- **Metrics**: Total debt, Count, Overdue amount |

### 6.5. Hiệu suất & Vận hành

| Mã | Báo cáo | Trạng thái | Ghi chú |
|----|----------|------------|---------|
| PERF-RPT-01 | Peak Hours Heatmap | ✅ | `ReportController.PeakHoursAnalysis`:<br>- **Heatmap Chart**: Đơn hàng theo Day-of-Week × Hour<br>- **Color Scale**: Xám (0-5) → Xanh đậm (31+)<br>- **Top 5 Peak Hours**: Xếp hạng giờ cao điểm<br>- **Lọc**: Thời gian, chi nhánh |
| PERF-RPT-02 | Operational Metrics | ✅ | `ReportController.OperationalMetrics`:<br>- **Fulfillment Time**: Avg & Median (phút)<br>- **Table Turnover**: Top 5 bàn theo số đơn<br>- **Distribution**: 100 first fulfillment times<br>- **Lọc**: Thời gian, chi nhánh |
| PERF-RPT-03 | Staff Performance | ✅ | `ReportController.StaffPerformance`:<br>- **Bar Chart**: Order count & Revenue per staff<br>- **Leaderboard**: Top 10 nhân viên<br>- **Metrics**: Order count, Total revenue, Avg ticket size<br>- **Lọc**: Thời gian, limit |

### 6.6. Xu hướng & Seasonality

| Mã | Báo cáo | Trạng thái | Ghi chú |
|----|----------|------------|---------|
| TREND-RPT-01 | Seasonality Analysis | ✅ | `ReportController.SeasonalityAnalysis`:<br>- **Weekly Pattern**: Bar chart doanh thu TB theo ngày trong tuần<br>- **Monthly Trend**: Line chart xu hướng theo tháng<br>- **Metrics**: Avg revenue, Order count per period<br>- **Lọc**: Thời gian (khuyến nghị 12 tháng) |

### 📊 Dashboard Tổng hợp

**File:** `Views/Report/Dashboard.cshtml` (Version 2.0)

**Cấu trúc:**
- **4 KPI Cards**: Total Revenue, Total Orders, Net Profit, Active Customers
- **Section A**: 3 charts doanh thu & lợi nhuận
- **Section B**: 3 charts/components khách hàng (Metrics, CLV, Cohort)
- **Section C**: 2 charts kho & nhập hàng
- **Section D**: 2 donut charts công nợ (AR/AP)
- **Section E**: 3 components hiệu suất (Heatmap, Metrics, Staff)
- **Section F**: 2 charts xu hướng (Weekly, Monthly)

**Tính năng:**
- ✅ ApexCharts.js integration
- ✅ Date range filter với MoM/YoY comparison
- ✅ Branch filter
- ✅ Real-time chart updates
- ✅ Export CSV/PDF placeholders
- ✅ Responsive layout với Bootstrap classes
- ✅ LINQ query syntax trong controller
- ✅ ViewBag data passing

**Cần cải thiện:**
- ⏳ Recipe-based COGS calculation (thay vì 35% flat)
- ⏳ Pre-aggregation tables (DailyRevenue, DishProfitDaily)
- ⏳ Caching với ReportsCache entity
- ⏳ Advanced smoothing/forecasting
- ⏳ Campaign impact tracking

---

## 7. Chức năng mở rộng

### 7.1. Đặt bàn & Giao hàng
| Mã | Chức năng | Trạng thái | Ghi chú |
|----|------------|------------|---------|
| BOOK-01 | Đặt bàn trực tuyến | ❌ | Chưa có API/web cho khách |
| BOOK-02 | Theo dõi bàn | 🔄 | Tích hợp với `TableController` |
| DEL-01 | Đặt món online | ❌ | Chưa có web/app cho khách |
| DEL-02 | Quản lý giao hàng | ❌ | Chưa có `DeliveryController` |

### 7.2. Quản lý nhà cung cấp
| SUP-01 | Quản lý thông tin | ❌ | Chưa có `SupplierController` |
| SUP-02 | Theo dõi công nợ | ❌ | Liên kết với accounting |
| SUP-03 | Lịch sử giao dịch | ❌ | Cần import invoice tracking |

### 7.3. Kế toán tài chính
| Mã | Chức năng | Trạng thái | Ghi chú |
|----|------------|------------|---------|
| ACC-01 | Quản lý thu chi | ❌ | Chưa có `AccountingController` |
| ACC-02 | Quản lý công nợ | ❌ | Cần debt tracking |
| ACC-03 | Báo cáo tài chính | ❌ | Cần financial reports |

---

## 8. Chức năng chung

| Chức năng | Trạng thái | Ghi chú |
|------------|------------|---------|
| Authentication | 🔄 | Cookie auth cơ bản, cần cải thiện |
| Authorization | 🔄 | Role-based, cần policy-based |
| Real-time updates | ✅ | SignalR implemented cho dish updates, Notes view cho đầu bếp |
| API endpoints | ❌ | Chưa có REST API |
| Mobile app | ❌ | Chưa có |
| Multi-branch | ❌ | Chưa hỗ trợ |
| Backup/Restore | ❌ | Chưa có |
| Audit logging | ❌ | Chưa có |

---

## 9. Ưu tiên triển khai tiếp theo

### **Cao nhất (Core POS)**
1. Hoàn thiện POS-03: Logic thay đổi thứ tự/hủy món
2. Triển khai POS-05 & POS-07: Invoice management và in hóa đơn
3. POS-04: Thông báo real-time đến bếp từ POS
4. ✅ **POS-10**: Gọi món tự phục vụ qua QR code (đã hoàn thành)

### **Cao (Operations)**
4. INV-04: Tự động trừ kho khi bán
5. INV-05: Cảnh báo hết hàng
6. HR-03 & HR-04: Shift và Attendance management

### **Trung bình (Reports & Extensions)**
7. ✅ **REP-01 to REP-06**: Báo cáo cơ bản (đã hoàn thành)
8. BOOK-01 & DEL-01: Online ordering
9. SUP-01 to SUP-03: Supplier management

### **Thấp (Advanced)**
10. ACC-01 to ACC-03: Accounting
11. API development
12. Mobile app

---

## 10. Ghi chú kỹ thuật

- **Framework**: ASP.NET Core MVC 8.0, EF Core, SQL Server
- **Real-time**: SignalR implemented for dish updates
- **UI**: Bootstrap 5, responsive design
- **Authentication**: Cookie-based with claims
- **Testing**: Chưa có unit tests
- **Deployment**: Chưa có CI/CD

---

## 11. Cập nhật quy tắc phát triển

| Ngày | Loại cập nhật | Chi tiết |
|------|---------------|----------|
| 30/10/2025 | Quy tắc CSS | Thêm rule nghiêm cấm thêm CSS mới:<br>- **Không được thêm CSS tùy chỉnh**: Chỉ sử dụng class có sẵn trong `wwwroot/css/style.css`<br>- **Class có sẵn**: `text-primary-*`, `bg-primary-*`, `btn-primary`, `card`, `alert-*`, etc.<br>- **Mục đích**: Đảm bảo tính nhất quán UI và tránh duplicate styling |
| 30/10/2025 | Sửa GenerateQR.cshtml | Xóa CDN Bootstrap/FontAwesome, xóa CSS tùy chỉnh, chuyển sang dùng layout chung và class có sẵn |
| 30/10/2025 | Sửa Notes.cshtml | Xóa CDN Bootstrap/FontAwesome/SignalR, xóa CSS tùy chỉnh, chuyển sang dùng layout chung và class có sẵn |
| 30/10/2025 | Cập nhật đặc tả | Thêm 3 trang chính: Notes.cshtml (khách chọn món), Index.cshtml (quản lý CRUD), trang bếp theo dõi hóa đơn |
| 12/11/2025 | Triển khai Analytics | **Module Phân tích & Báo cáo hoàn chỉnh (REP-01 đến REP-06)**:<br>- **ReportController**: 6 endpoints API (RevenueByDate, TopDishes, TopCustomers, ProfitSummary, DebtSummary, PerformanceMetrics)<br>- **Dashboard View**: ApexCharts integration với 6 loại biểu đồ (Line, Bar, Donut, Combo, Stacked Bar, Area)<br>- **Entities**: Thêm ReportsCache vào FinanceEntities<br>- **Features**: Real-time filtering, summary cards, export CSV/PDF placeholders<br>- **LINQ Queries**: Optimized aggregation cho doanh thu, lợi nhuận, công nợ, hiệu suất |

---

**File này được cập nhật tự động khi triển khai use cases mới. Cập nhật lần cuối: 16/11/2025 - Hoàn thiện cập nhật kho & Excel templates**

---

## 12. Cập nhật gần đây (16/11/2025)

- ✅ Inventory UI: Tô màu cột "Loại" trong `Views/Inventory/Details.cshtml` theo `StockTransactionType` (PurchaseIn=bg-success, SaleOut=bg-danger, AdjustmentIn=bg-info, AdjustmentOut=bg-secondary, TransferIn=bg-primary, TransferOut=bg-dark, Waste=bg-danger).
- ✅ Chính sách ngừng hoạt động (Inventory): Khi `IsActive=false` chỉ cập nhật trạng thái; không tự tạo giao dịch write-off, không thay đổi `CurrentQuantity` (đã refactor `InventoryController.Edit`).
- ✅ EPPlus: Hạ phiên bản xuống v7.4.2 và thiết lập license `NonCommercial` ở startup để tránh lỗi LicenseNotSetException khi xuất template.
- ✅ Mẫu Excel: Giữ `InventoryImportTemplate.xlsx` ổn định; thêm `SupplierImportTemplate.xlsx` với cột cơ bản (Name, PrimaryContactName, Phone, Email, Address, PaymentTerms, Currency, IsActive) và đường dẫn tải từ sidebar.