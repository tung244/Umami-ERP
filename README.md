# 🍱 Umami ERP

**Umami ERP** là hệ thống phần mềm quản lý nhà hàng và chuỗi F&B (Food & Beverage) toàn diện, được xây dựng trên nền tảng **ASP.NET Core 8 MVC**. Dự án cung cấp giải pháp chuyển đổi số mạnh mẽ từ khâu gọi món (POS), hiển thị bếp (Kitchen Display System), quản lý kho nguyên vật liệu, cho đến quản trị nhân sự và tài chính.

## ✨ Tính năng nổi bật

- 🛒 **Quản lý Bán hàng & Order (POS):** Hỗ trợ đa dạng loại hình phục vụ (Dine-in, Takeaway, Delivery). Quản lý phòng/bàn, sơ đồ tầng, ghép/tách bàn và xử lý thanh toán đa phương thức (Tiền mặt, Thẻ, QR, Split payment).
- 👨‍🍳 **Hệ thống Quản lý Bếp (KDS - Kitchen Display System):** Đồng bộ thời gian thực (Real-time) các phiếu gọi món xuống bếp thông qua **SignalR**, tối ưu hóa thời gian chuẩn bị và trả món.
- 📦 **Quản lý Kho & Chuỗi Cung Ứng (Inventory & Procurement):** Theo dõi xuất/nhập tồn theo thời gian thực, định lượng nguyên vật liệu (Recipe/BOM), tự động trừ kho khi món được phục vụ. Cảnh báo tồn kho tự động (Background Service) và quản lý đặt hàng nhà cung cấp (PO).
- 👥 **Quản lý Nhân sự & Chấm công (HR & Attendance):** Theo dõi ca làm việc, chấm công nhân viên (hỗ trợ QR/Biometric qua SignalR), phân quyền chi tiết (Role-based access) và tính toán bảng lương (Payroll).
- 🎁 **Khách hàng thân thiết (CRM & Loyalty):** Tích điểm, phân hạng thành viên (Silver, Gold...), quản lý voucher và các chiến dịch khuyến mãi phức tạp.
- 📊 **Tài chính & Kế toán (Finance):** Quản lý thu/chi, sổ cái công nợ (AR/AP), đối soát nhà cung cấp và hệ thống báo cáo doanh thu thời gian thực.

## 🛠️ Công nghệ & Kiến trúc (Tech Stack)

- **Backend:** C# / ASP.NET Core 8 MVC
- **Cơ sở dữ liệu:** SQL Server, Entity Framework Core 8 (Code-First)
- **Real-time Communication:** SignalR (Sử dụng cho Hub Bếp, Thanh toán, Chấm công, Báo cáo)
- **Security:** 
  - JWT (JSON Web Tokens) & Cookie Authentication linh hoạt.
  - ASP.NET Core Data Protection (DPAPI).
- **Khác:** EPPlus (Xuất báo cáo Excel), Hosted Services (Xử lý tác vụ chạy ngầm cảnh báo kho).

## 🚀 Cài đặt & Chạy dự án (Getting Started)

1. Clone dự án:
   ```bash
   git clone https://github.com/your-username/UmamiERP.git
   ```
2. Cập nhật chuỗi kết nối (`DefaultConnection`) trong `appsettings.json`.
3. Chạy Migration để tạo Database:
   ```bash
   dotnet ef database update
   ```
4. Chạy dự án:
   ```bash
   dotnet run
   ```

## 📄 License
Phân phối theo giấy phép MIT. Xem file `LICENSE` để biết thêm thông tin.