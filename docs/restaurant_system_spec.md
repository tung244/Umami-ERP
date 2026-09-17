# 📘 Đặc Tả Dự Án – Hệ Thống Quản Lý Nhà Hàng

## 1. Giới thiệu

### 1.1. Mục tiêu dự án
Xây dựng **hệ thống quản lý nhà hàng toàn diện**, giúp:
- Tối ưu quy trình vận hành.  
- Giảm sai sót trong phục vụ và quản lý.  
- Hỗ trợ ra quyết định qua báo cáo và phân tích dữ liệu.

### 1.2. Phạm vi
Phục vụ cho:
- Nhà hàng có **nhiều khu vực**: bàn ăn, bếp, thu ngân, kho, kế toán.  
- Các nhóm người dùng:
  - Quản lý  
  - Nhân viên phục vụ  
  - Thu ngân  
  - Bếp  
  - Nhân viên kho  
  - Kế toán  
  - Khách hàng  

---

## 2. Tổng quan hệ thống

### 2.1. Các phân hệ chính
1. POS – Quản lý bán hàng  
2. Quản lý thực đơn (Menu Management)  
3. Quản lý kho (Inventory Management)  
4. Quản lý nhân viên (Staff Management)  
5. Quản lý khách hàng (Customer Management)  
6. Phân tích & Báo cáo (Analytics & Reporting)  
7. Chức năng mở rộng: Đặt bàn, Giao hàng, Nhà cung cấp, Kế toán tài chính  

---

## 3. Đặc tả chức năng chi tiết

### 3.1. POS – Quản lý bán hàng
| Mã | Chức năng | Mô tả | Người dùng |
|----|------------|--------|-------------|
| POS-01 | Quản lý bàn ăn | Hiển thị sơ đồ bàn (số bàn, khu vực, tình trạng) | Phục vụ, Quản lý |
| POS-02 | Gọi món | Chọn bàn → chọn món → gửi đến bếp | Phục vụ |
| POS-02a | Trang chọn món cho khách | Giao diện web cho khách hàng tự chọn món qua QR code bàn (Notes.cshtml) | Khách hàng |
| POS-03 | Quản lý thứ tự món ăn | Thay đổi thứ tự hoặc hủy món chưa chế biến | Phục vụ |
| POS-04 | Thông báo đến bếp | Gửi tự động đơn hàng đến khu vực bếp | Hệ thống |
| POS-04a | Trang theo dõi bếp | Bếp theo dõi các hóa đơn cần nấu, cập nhật trạng thái real-time qua SignalR | Bếp |
| POS-05 | Tính tiền & lập hóa đơn | Tự động tính tổng, thuế, giảm giá | Thu ngân |
| POS-06 | Thanh toán | Hỗ trợ tiền mặt, thẻ, QR, ví điện tử | Thu ngân |
| POS-07 | In hóa đơn | In hóa đơn chi tiết | Thu ngân |
| POS-08 | Lịch sử đơn hàng | Xem lại hóa đơn và thanh toán | Quản lý, Thu ngân |

---

### 3.2. Quản lý thực đơn (Menu Management)
| Mã | Chức năng | Mô tả | Người dùng |
|----|------------|--------|-------------|
| MENU-01 | Quản lý danh mục món ăn | Tạo danh mục món | Quản lý |
| MENU-02 | Thêm/Sửa/Xóa món ăn | Cập nhật thông tin món (Index.cshtml - giao diện quản lý) | Quản lý |
| MENU-02a | Hiển thị món cho khách | Giao diện card view cho khách hàng chọn món (Notes.cshtml) | Khách hàng |
| MENU-03 | Quản lý giá | Lưu lịch sử thay đổi giá, thiết lập theo thời điểm | Quản lý |
| MENU-04 | Ẩn/Hiện món | Ẩn món khi hết nguyên liệu | Quản lý, Kho |

---

### 3.3. Quản lý kho (Inventory Management)
| Mã | Chức năng | Mô tả | Người dùng |
|----|------------|--------|-------------|
| INV-01 | Theo dõi tồn kho | Danh sách nguyên liệu, hạn dùng | Kho |
| INV-02 | Nhập kho | Ghi nhận nhập hàng | Kho |
| INV-03 | Xuất kho | Ghi nhận xuất kho | Kho |
| INV-04 | Tự động trừ kho | Khi có đơn bán | Hệ thống |
| INV-05 | Cảnh báo hết hàng | Báo khi dưới mức tối thiểu | Kho, Quản lý |
| INV-06 | Kiểm kê định kỳ | Đối chiếu thực tế | Kho |

---

### 3.4. Quản lý nhân viên (Staff Management)
| Mã | Chức năng | Mô tả | Người dùng |
|----|------------|--------|-------------|
| HR-01 | Hồ sơ nhân viên | Lưu thông tin, lương, chức vụ | Nhân sự |
| HR-02 | Phân quyền | Cấp quyền theo vai trò | Quản lý |
| HR-03 | Lịch làm việc | Đăng ký, phê duyệt ca làm | Nhân viên, Quản lý |
| HR-04 | Chấm công | Ghi nhận bằng QR, thẻ, vân tay | Nhân sự |
| HR-05 | Báo cáo công & lương | Tổng hợp và tính lương | Nhân sự |

---

### 3.5. Quản lý khách hàng (Customer Management)
| Mã | Chức năng | Mô tả | Người dùng |
|----|------------|--------|-------------|
| CUST-01 | Hồ sơ khách hàng | Lưu thông tin & lịch sử giao dịch | Quản lý |
| CUST-02 | Chương trình thân thiết | Tích điểm, xếp hạng | Quản lý |
| CUST-03 | Giảm giá & Voucher | Tạo mã khuyến mãi | Marketing |
| CUST-04 | Lịch sử mua hàng | Xem hóa đơn, món yêu thích | Quản lý, Khách hàng |

---

### 10. Báo cáo & Phân tích
| Mã | Báo cáo | Mô tả | Người dùng |
|----|----------|--------|-------------|
| REP-01 | Doanh thu theo thời gian | Báo cáo ngày/tháng/năm | Quản lý |
| REP-02 | Doanh thu theo món | Món bán chạy/chậm | Quản lý |
| REP-03 | Doanh thu theo khách hàng | Top khách hàng chi tiêu cao | Quản lý |
| REP-04 | Lợi nhuận & chi phí | Lợi nhuận gộp, chi phí nguyên liệu | Kế toán |
| REP-05 | Công nợ | Công nợ khách hàng & nhà cung cấp | Kế toán |
| REP-06 | Hiệu quả hoạt động | Tỷ lệ hủy đơn, thời gian phục vụ | Quản lý |

---

### 3.7. Chức năng mở rộng
#### 3.7.1. Đặt bàn & Giao hàng
| Mã | Chức năng | Mô tả | Người dùng |
|----|------------|--------|-------------|
| BOOK-01 | Đặt bàn trực tuyến | Đặt qua web/app, nhận SMS/Email | Khách hàng |
| BOOK-02 | Theo dõi bàn | Danh sách bàn đặt/chờ/phục vụ | Phục vụ |
| DEL-01 | Đặt món online | Qua web/app, thanh toán trước/COD | Khách hàng |
| DEL-02 | Quản lý giao hàng | Theo dõi trạng thái đơn | Quản lý, Giao hàng |

#### 3.7.2. Quản lý nhà cung cấp
| SUP-01 | Quản lý thông tin | Lưu tên, liên hệ, sản phẩm | Quản lý |
| SUP-02 | Theo dõi công nợ | Ghi nhận nhập hàng & nợ | Kế toán |
| SUP-03 | Lịch sử giao dịch | Đơn nhập từng nhà cung cấp | Kho, Kế toán |

#### 3.7.3. Kế toán tài chính
| Mã | Chức năng | Mô tả | Người dùng |
|----|------------|--------|-------------|
| ACC-01 | Quản lý thu chi | Ghi nhận phiếu thu, chi | Kế toán |
| ACC-02 | Quản lý công nợ | Theo dõi phải thu, phải trả | Kế toán |
| ACC-03 | Báo cáo tài chính | Tổng hợp dòng tiền, lợi nhuận | Kế toán, Quản lý |

---

## 4. Mô hình người dùng & quyền truy cập
| Vai trò | Quyền chính |
|----------|--------------|
| Quản lý | Toàn quyền hệ thống, xem báo cáo |
| Thu ngân | POS, thanh toán, in hóa đơn |
| Phục vụ | Quản lý bàn, gọi món |
| Bếp | Nhận đơn, cập nhật trạng thái, theo dõi hóa đơn cần nấu (POS-04a) |
| Kho | Nhập – xuất – tồn nguyên liệu |
| Kế toán | Thu chi, công nợ, báo cáo tài chính |
| Nhân sự | Lịch làm việc, chấm công |
| Khách hàng | Đặt bàn, đặt món, xem lịch sử |

---

## 5. Yêu cầu phi chức năng
| Nhóm | Yêu cầu |
|-------|----------|
| Hiệu năng | POS xử lý <1s khi thêm món, <3s khi thanh toán |
| Bảo mật | Phân quyền chi tiết, mã hóa thông tin khách hàng |
| Khả năng mở rộng | Hỗ trợ multi-branch |
| Khả dụng | Uptime 99%, backup định kỳ |
| Giao diện | Thân thiện, tối ưu cảm ứng |
| Tích hợp | API với hệ thống kế toán/giao hàng |

---

## 6. Mô hình dữ liệu khái quát
```plaintext
Customer(CustomerId, Name, Phone, Tier, Points, ...)
Staff(StaffId, Name, Role, Salary, WorkShift, ...)
Table(TableId, Area, Status, ...)
Order(OrderId, TableId, StaffId, OrderDate, TotalAmount, Status, ...)
OrderDetail(OrderId, DishId, Quantity, Price, ...)
Dish(DishId, Name, CategoryId, Price, Image, ...)
Category(CategoryId, Name, Description)
Inventory(ItemId, Name, Quantity, Unit, MinQuantity, ...)
Supplier(SupplierId, Name, Contact, ...)
ImportInvoice(InvoiceId, SupplierId, Date, TotalAmount, ...)
Accounting(TransId, Type, Amount, Description, Date, ...)
```

---

## 7. Luồng nghiệp vụ chính
**Luồng bán hàng:**
1. Phục vụ chọn bàn → gọi món → gửi đơn đến bếp.  
2. Bếp chế biến → báo hoàn tất → phục vụ giao món.  
3. Khách yêu cầu thanh toán → thu ngân tính tiền → in hóa đơn.  
4. Hệ thống tự động cập nhật tồn kho, doanh thu, điểm khách hàng.

**Luồng đặt món tự phục vụ:**
1. Khách quét QR bàn → truy cập trang Notes.cshtml → chọn món → thêm vào giỏ hàng.  
2. Khách xác nhận đơn hàng → hệ thống tạo Order và OrderDetail.  
3. SignalR gửi thông báo real-time đến trang bếp (POS-04a).  
4. Bếp theo dõi và cập nhật trạng thái món → click hoàn thành để xóa khỏi danh sách.  
5. Phục vụ giao món → thu ngân tính tiền.

**Luồng quản lý thực đơn:**
1. Quản lý truy cập Index.cshtml → CRUD món ăn, quản lý danh mục.  
2. Thay đổi trạng thái món → SignalR cập nhật real-time đến Notes.cshtml.  
3. Khách hàng chỉ thấy món đang hoạt động trên trang chọn món.

---

## 9. Tích hợp SignalR & Real-time Updates
| Chức năng | Trigger | Receiver | Action |
|------------|---------|----------|--------|
| Cập nhật trạng thái món | Quản lý thay đổi Active/Inactive (Index.cshtml) | Notes.cshtml | Ẩn/hiện món real-time |
| Thông báo đặt hàng | Khách đặt món (Notes.cshtml) | Trang bếp (POS-04a) | Thêm đơn vào danh sách nấu |
| Cập nhật số lượng cần chuẩn bị | Đặt hàng mới | Notes.cshtml | Hiển thị cảnh báo nguyên liệu |
| Hoàn thành món | Bếp click hoàn thành | Trang bếp (POS-04a) | Xóa đơn khỏi danh sách |

---
- Báo cáo doanh thu (ngày/tháng/năm)  
- Báo cáo món bán chạy  
- Báo cáo khách hàng thân thiết  
- Báo cáo công nợ  
- Báo cáo tồn kho và cảnh báo  
- Báo cáo nhân sự  
