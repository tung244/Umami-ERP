# 🎟️ Hướng dẫn Quản lý Voucher & Khuyến mãi

## ✅ Tổng quan

Hệ thống Voucher cho phép:
- Tạo và quản lý các chương trình khuyến mãi
- Áp dụng voucher khi thanh toán QR
- 3 loại giảm giá: Phần trăm (%), Số tiền cố định (VND), Tặng món miễn phí
- Theo dõi lịch sử sử dụng voucher

---

## 🎯 Các loại Voucher

### 1. **Percentage (Phần trăm)**
- Giảm theo % giá trị đơn hàng
- Ví dụ: Giảm 20% cho đơn hàng từ 200,000 VND

### 2. **FixedAmount (Số tiền cố định)**
- Giảm một số tiền nhất định
- Ví dụ: Giảm 50,000 VND cho đơn hàng từ 100,000 VND

### 3. **FreeItem (Tặng món miễn phí)**
- Tặng món ăn/đồ uống miễn phí
- Logic xử lý riêng (tùy customize)

---

## 🚀 Quản lý Voucher

### **Truy cập trang quản lý:**
```
http://localhost:5256/Voucher/Index
```

### **1. Xem danh sách voucher**

**Tính năng:**
- ✅ Hiển thị tất cả voucher
- ✅ Tìm kiếm theo mã hoặc mô tả
- ✅ Sắp xếp theo: Mã, Loại, Giá trị, Thời gian
- ✅ Phân trang (10 items/trang)
- ✅ Highlight voucher đang có hiệu lực

---

### **2. Tạo voucher mới**

**URL:** `/Voucher/Create`

**Thông tin cần nhập:**
- **Mã voucher** (bắt buộc, unique): VD: `SUMMER2024`, `WELCOME50K`
- **Mô tả**: Mô tả chương trình khuyến mãi
- **Loại giảm giá**:
  - `Percentage`: Giảm %
  - `FixedAmount`: Giảm số tiền cố định
  - `FreeItem`: Tặng món
- **Giá trị giảm**:
  - Nếu `Percentage`: Nhập số % (0-100)
  - Nếu `FixedAmount`: Nhập số tiền VND
- **Hiệu lực từ** (bắt buộc): Ngày bắt đầu
- **Hiệu lực đến** (bắt buộc): Ngày kết thúc
- **Giới hạn sử dụng/khách** (tùy chọn): Số lần 1 khách được dùng
- **Tổng giới hạn sử dụng** (tùy chọn): Tổng số lần dùng
- **Áp dụng cho** (tùy chọn): Loại món/danh mục
- **Đơn hàng tối thiểu** (tùy chọn): Số tiền tối thiểu để dùng voucher
- **Kích hoạt**: Bật/tắt voucher

**Ví dụ:**
```
Mã: SUMMER20
Mô tả: Giảm 20% mùa hè
Loại: Percentage
Giá trị: 20
Hiệu lực: 01/06/2024 - 31/08/2024
Đơn tối thiểu: 100,000 VND
```

---

### **3. Chỉnh sửa voucher**

**URL:** `/Voucher/Edit/{id}`

- Có thể sửa tất cả thông tin
- ⚠️ **Lưu ý**: Không nên sửa voucher đã được sử dụng nhiều

---

### **4. Xóa voucher**

- Xóa voucher không còn dùng
- ⚠️ **Cảnh báo**: Xóa vĩnh viễn, không thể khôi phục!

---

## 💳 Áp dụng Voucher khi Thanh toán

### **Quy trình:**

1. **Khách hàng thanh toán đơn hàng**
   - Vào trang chi tiết đơn hàng
   - Nhấn **"Thanh toán QR"**

2. **Modal thanh toán hiện ra**
   - Thấy section **"Áp dụng Voucher"** ở đầu modal
   - Màu cam nổi bật với icon 🎟️

3. **Nhập mã voucher**
   - Gõ mã voucher vào ô input
   - Nhấn **"Áp dụng"**

4. **Hệ thống validate**
   - Check voucher có tồn tại không
   - Check còn hiệu lực không
   - Check đơn hàng đủ điều kiện không (minimum amount)
   - Tính toán số tiền giảm

5. **Kết quả:**
   - ✅ **Thành công**: 
     - Hiển thị thông báo xanh
     - Hiện số tiền gốc gạch ngang
     - Hiện số tiền giảm
     - Hiện số tiền cuối cùng
     - Button đổi thành **"Hủy"** để remove voucher
   - ❌ **Thất bại**:
     - Hiển thị lỗi màu đỏ
     - Giữ nguyên số tiền gốc

6. **Xác nhận thanh toán**
   - Sau khi quét QR
   - Nhấn **"Xác nhận đã thanh toán"**
   - Voucher được áp dụng vào Payment
   - Notes ghi lại: "Voucher: XXX (Giảm YYY VND)"

---

## 🎨 Giao diện Voucher trong Thanh toán

### **Trước khi áp dụng:**
```
┌──────────────────────────────────────┐
│ 🎟️ Áp dụng Voucher                  │
│ ┌──────────────────┬──────────────┐ │
│ │ Nhập mã voucher  │  [Áp dụng]   │ │
│ └──────────────────┴──────────────┘ │
└──────────────────────────────────────┘

Đơn hàng: ORD-001
Số tiền: 150,000 VND
```

### **Sau khi áp dụng thành công:**
```
┌──────────────────────────────────────┐
│ 🎟️ Áp dụng Voucher                  │
│ ┌──────────────────┬──────────────┐ │
│ │ SUMMER20         │    [Hủy]     │ │
│ └──────────────────┴──────────────┘ │
│ ✅ Áp dụng voucher thành công!       │
│ Giảm 20%                             │
└──────────────────────────────────────┘

Đơn hàng: ORD-001
~~Tổng tiền: 150,000 VND~~
🏷️ Giảm giá: -30,000 VND
Số tiền: 120,000 VND
```

---

## 🔧 API Endpoint

### **Validate Voucher**

**Endpoint:** `POST /Voucher/ValidateVoucher`

**Request:**
```json
{
  "code": "SUMMER20",
  "orderAmount": 150000
}
```

**Response Success:**
```json
{
  "success": true,
  "message": "Áp dụng voucher thành công!",
  "voucherCode": "SUMMER20",
  "discountType": "Percentage",
  "discountValue": 20,
  "discountAmount": 30000,
  "discountInfo": "Giảm 20%",
  "finalAmount": 120000
}
```

**Response Error:**
```json
{
  "success": false,
  "message": "Voucher chỉ có hiệu lực từ 01/06/2024 đến 31/08/2024!"
}
```

---

## 📊 Database Schema

### Table: **Vouchers**

| Column | Type | Description |
|--------|------|-------------|
| `VoucherId` | GUID (PK) | ID voucher |
| `Code` | nvarchar(64) (Unique) | Mã voucher |
| `Description` | nvarchar(max) | Mô tả |
| `DiscountType` | int (Enum) | Loại giảm giá |
| `DiscountValue` | decimal(18,2) | Giá trị giảm |
| `ValidFrom` | DateTime | Hiệu lực từ |
| `ValidTo` | DateTime | Hiệu lực đến |
| `UsageLimitPerCustomer` | int? | Giới hạn/khách |
| `TotalUsageLimit` | int? | Tổng giới hạn |
| `AppliedTo` | nvarchar(128) | Áp dụng cho |
| `MinimumOrderAmount` | decimal(18,2)? | Đơn tối thiểu |
| `IsActive` | bit | Kích hoạt |
| `CreatedAt` | DateTime | Ngày tạo |
| `UpdatedAt` | DateTime? | Ngày cập nhật |

### Indexes:
- `IX_Voucher_Code` (Unique)
- `IX_Voucher_ValidityPeriod` (ValidFrom, ValidTo)

---

## 🧪 Testing

### **Test Case 1: Áp dụng voucher phần trăm**

**Điều kiện:**
- Voucher: `TEST20` (Giảm 20%)
- Đơn hàng: 100,000 VND
- Đơn tối thiểu: 50,000 VND

**Kết quả mong đợi:**
- Giảm: 20,000 VND
- Thanh toán: 80,000 VND
- Payment.Notes: "Thanh toán qua QR Code - Voucher: TEST20 (Giảm 20,000 VND)"

---

### **Test Case 2: Voucher số tiền cố định**

**Điều kiện:**
- Voucher: `SAVE50K` (Giảm 50,000 VND)
- Đơn hàng: 150,000 VND

**Kết quả mong đợi:**
- Giảm: 50,000 VND
- Thanh toán: 100,000 VND

---

### **Test Case 3: Voucher hết hạn**

**Điều kiện:**
- Voucher: `OLD20` (Hết hạn 31/12/2023)
- Hôm nay: 05/11/2024

**Kết quả mong đợi:**
- ❌ Lỗi: "Voucher chỉ có hiệu lực từ XX đến 31/12/2023!"
- Không áp dụng

---

### **Test Case 4: Đơn hàng không đủ điều kiện**

**Điều kiện:**
- Voucher: `VIP100K` (Đơn tối thiểu 200,000 VND)
- Đơn hàng: 100,000 VND

**Kết quả mong đợi:**
- ❌ Lỗi: "Đơn hàng tối thiểu phải từ 200,000 VND!"
- Không áp dụng

---

### **Test Case 5: Voucher không tồn tại**

**Điều kiện:**
- Voucher: `FAKE123`
- Code không có trong DB

**Kết quả mong đợi:**
- ❌ Lỗi: "Mã voucher không tồn tại hoặc đã bị vô hiệu hóa!"

---

### **Test Case 6: Hủy voucher và thanh toán lại**

**Quy trình:**
1. Áp dụng voucher thành công
2. Nhấn **"Hủy"**
3. Số tiền trở về ban đầu
4. Áp dụng voucher khác
5. Thanh toán

**Kết quả mong đợi:**
- Voucher cuối cùng được áp dụng vào Payment

---

## 📝 Best Practices

### **Khi tạo voucher:**

✅ **NÊN:**
- Đặt mã ngắn gọn, dễ nhớ: `SUMMER20`, `WELCOME50`
- Mô tả rõ ràng điều kiện
- Set thời gian hiệu lực hợp lý
- Test voucher trước khi public
- Đặt giới hạn sử dụng để tránh lạm dụng

❌ **KHÔNG NÊN:**
- Mã quá dài hoặc phức tạp
- Không set thời gian kết thúc
- Giảm giá quá cao (>80%)
- Không có điều kiện đơn tối thiểu

---

### **Khi áp dụng voucher:**

✅ **NÊN:**
- Kiểm tra kỹ thông tin trước khi xác nhận
- Thông báo rõ cho khách số tiền được giảm
- Ghi chú voucher trong payment để theo dõi

❌ **KHÔNG NÊN:**
- Cho phép stack nhiều voucher (nếu không cho phép)
- Bỏ qua validation
- Không ghi log sử dụng voucher

---

## 🔥 Ví dụ thực tế

### **Voucher Chào mừng khách mới**
```
Mã: WELCOME10
Mô tả: Giảm 10% cho khách hàng mới
Loại: Percentage
Giá trị: 10
Đơn tối thiểu: 50,000 VND
Hiệu lực: 01/01/2024 - 31/12/2024
Giới hạn/khách: 1
```

### **Voucher Khuyến mãi cuối tuần**
```
Mã: WEEKEND30K
Mô tả: Giảm 30,000 VND cuối tuần
Loại: FixedAmount
Giá trị: 30000
Đơn tối thiểu: 100,000 VND
Hiệu lực: Thứ 7 & Chủ nhật
Tổng giới hạn: 100
```

### **Voucher Sinh nhật**
```
Mã: BDAY50
Mô tả: Giảm 50% đơn hàng sinh nhật
Loại: Percentage
Giá trị: 50
Đơn tối thiểu: 200,000 VND
Giới hạn/khách: 1
```

---

## 🐛 Troubleshooting

### **Vấn đề: Voucher không áp dụng được**

**Nguyên nhân:**
- Voucher hết hạn
- Đơn hàng không đủ điều kiện
- Voucher bị vô hiệu hóa (`IsActive = false`)

**Giải pháp:**
```sql
-- Check voucher
SELECT * FROM Vouchers WHERE Code = 'SUMMER20';

-- Check hiệu lực
SELECT Code, ValidFrom, ValidTo, IsActive, MinimumOrderAmount
FROM Vouchers 
WHERE Code = 'SUMMER20';
```

---

### **Vấn đề: Giảm giá sai số tiền**

**Nguyên nhân:**
- Logic tính toán sai
- Frontend gửi sai `discountAmount`

**Giải pháp:**
- Kiểm tra Console log trong browser (F12)
- Kiểm tra Server log
- Validate lại ở API

---

## 📈 Reports

### **Thống kê voucher được sử dụng nhiều nhất**
```sql
SELECT 
    v.Code,
    v.Description,
    COUNT(*) as UsageCount,
    SUM(CAST(SUBSTRING(p.Notes, 
        CHARINDEX('Giảm', p.Notes) + 5, 
        CHARINDEX('VND', p.Notes) - CHARINDEX('Giảm', p.Notes) - 6) 
        AS DECIMAL)) as TotalDiscount
FROM Payments p
INNER JOIN Vouchers v ON p.Notes LIKE '%Voucher: ' + v.Code + '%'
GROUP BY v.Code, v.Description
ORDER BY UsageCount DESC;
```

### **Doanh thu theo voucher**
```sql
SELECT 
    YEAR(p.PaymentDate) as Year,
    MONTH(p.PaymentDate) as Month,
    COUNT(CASE WHEN p.Notes LIKE '%Voucher:%' THEN 1 END) as OrdersWithVoucher,
    COUNT(*) as TotalOrders,
    SUM(CASE WHEN p.Notes LIKE '%Voucher:%' THEN p.Amount ELSE 0 END) as RevenueWithVoucher,
    SUM(p.Amount) as TotalRevenue
FROM Payments p
GROUP BY YEAR(p.PaymentDate), MONTH(p.PaymentDate)
ORDER BY Year DESC, Month DESC;
```

---

## ✅ Tóm tắt

| Chức năng | URL | Mô tả |
|-----------|-----|-------|
| Danh sách | `/Voucher/Index` | Xem tất cả voucher |
| Tạo mới | `/Voucher/Create` | Thêm voucher mới |
| Chi tiết | `/Voucher/Details/{id}` | Xem chi tiết |
| Chỉnh sửa | `/Voucher/Edit/{id}` | Sửa voucher |
| Xóa | `/Voucher/Delete` (POST) | Xóa voucher |
| Validate | `/Voucher/ValidateVoucher` (POST API) | Kiểm tra & tính giảm giá |

**Áp dụng voucher:** Trong modal thanh toán QR ở trang `/Order/Details/{id}`

---

**✅ HỆ THỐNG VOUCHER ĐÃ SẴN SÀNG!**

Truy cập: `http://localhost:5256/Voucher/Index` để quản lý voucher
