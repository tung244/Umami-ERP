using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models;

public static class DataSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        // Seed Branches
        var branch1 = new Branch
        {
            BranchId = Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
            Name = "Chi nhánh Trung tâm",
            Code = "CN001",
            Address = "123 Đường Lê Lợi, Quận 1, TP.HCM",
            Phone = "028-1234-5678",
            TimeZone = "SE Asia Standard Time",
            DefaultCurrency = "VND",
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var branch2 = new Branch
        {
            BranchId = Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
            Name = "Chi nhánh Phú Nhuận",
            Code = "CN002",
            Address = "456 Đường Nguyễn Kiệm, Quận Phú Nhuận, TP.HCM",
            Phone = "028-8765-4321",
            TimeZone = "SE Asia Standard Time",
            DefaultCurrency = "VND",
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Branch>().HasData(branch1, branch2);

        // Seed Roles
        var adminRole = new Role
        {
            RoleId = Guid.Parse("660e8400-e29b-41d4-a716-446655440001"),
            Name = "Administrator",
            Description = "Quyền quản trị viên hệ thống",
            DefaultPermissions = "all",
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var managerRole = new Role
        {
            RoleId = Guid.Parse("660e8400-e29b-41d4-a716-446655440002"),
            Name = "Manager",
            Description = "Quyền quản lý chi nhánh",
            DefaultPermissions = "branch_management,staff_management,inventory_management",
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var staffRole = new Role
        {
            RoleId = Guid.Parse("660e8400-e29b-41d4-a716-446655440003"),
            Name = "Staff",
            Description = "Quyền nhân viên phục vụ",
            DefaultPermissions = "order_management,customer_service",
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Role>().HasData(adminRole, managerRole, staffRole);
        
        // Seed Permissions
        var permissions = new List<Permission>
        {
            // Dashboard
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440001"), Name = "Dashboard.View", Description = "Xem dashboard" },

            // Menu Management
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440010"), Name = "Menu.Category.View", Description = "Xem danh mục món ăn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440011"), Name = "Menu.Category.Create", Description = "Tạo danh mục" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440012"), Name = "Menu.Category.Edit", Description = "Sửa danh mục" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440013"), Name = "Menu.Category.Delete", Description = "Xóa danh mục" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440020"), Name = "Menu.Dish.View", Description = "Xem món ăn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440021"), Name = "Menu.Dish.Create", Description = "Tạo món ăn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440022"), Name = "Menu.Dish.Edit", Description = "Sửa món ăn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440023"), Name = "Menu.Dish.Delete", Description = "Xóa món ăn" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440030"), Name = "Menu.PriceHistory.View", Description = "Xem lịch sử giá" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440031"), Name = "Menu.PriceHistory.Create", Description = "Tạo lịch sử giá" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440040"), Name = "Menu.DishIngredient.View", Description = "Xem nguyên liệu" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440041"), Name = "Menu.DishIngredient.Create", Description = "Tạo nguyên liệu" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440042"), Name = "Menu.DishIngredient.Edit", Description = "Sửa nguyên liệu" },

            // Inventory Management
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440050"), Name = "Inventory.View", Description = "Xem tồn kho" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440051"), Name = "Inventory.Import", Description = "Nhập kho" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440052"), Name = "Inventory.Export", Description = "Xuất kho" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440053"), Name = "Inventory.StockTaking", Description = "Kiểm kê kho" },

            // Staff Management
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440060"), Name = "Staff.View", Description = "Xem danh sách nhân viên" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440061"), Name = "Staff.Create", Description = "Tạo nhân viên" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440062"), Name = "Staff.Edit", Description = "Sửa thông tin nhân viên" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440063"), Name = "Staff.Delete", Description = "Xóa nhân viên" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440070"), Name = "Staff.Payroll.View", Description = "Xem bảng lương" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440071"), Name = "Staff.Payroll.Manage", Description = "Quản lý bảng lương" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440080"), Name = "Staff.RolesAndPermissions.Manage", Description = "Quản lý phân quyền" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440090"), Name = "Staff.Shift.View", Description = "Xem lịch làm việc" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440091"), Name = "Staff.Shift.Create", Description = "Tạo ca làm việc" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440092"), Name = "Staff.Shift.Edit", Description = "Sửa ca làm việc" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440100"), Name = "Staff.Attendance.View", Description = "Xem chấm công" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440101"), Name = "Staff.Attendance.Manage", Description = "Quản lý chấm công" },

            // Customer Management
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440110"), Name = "Customer.View", Description = "Xem danh sách khách hàng" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440111"), Name = "Customer.Create", Description = "Tạo khách hàng" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440112"), Name = "Customer.Edit", Description = "Sửa khách hàng" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440113"), Name = "Customer.Delete", Description = "Xóa khách hàng" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440120"), Name = "Customer.Loyalty.Manage", Description = "Quản lý chương trình thân thiết" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440121"), Name = "Customer.Voucher.Manage", Description = "Quản lý voucher" },

            // POS - Sales
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440130"), Name = "POS.Table.View", Description = "Xem danh sách bàn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440131"), Name = "POS.Table.Manage", Description = "Quản lý bàn ăn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440132"), Name = "POS.TableManagement.View", Description = "Xem dashboard quản lý bàn" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440140"), Name = "POS.Order.Create", Description = "Tạo đơn hàng" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440141"), Name = "POS.Order.View", Description = "Xem đơn hàng" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440142"), Name = "POS.Order.Edit", Description = "Sửa đơn hàng" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440143"), Name = "POS.Order.Delete", Description = "Xóa đơn hàng" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440150"), Name = "POS.SalesReport.View", Description = "Xem báo cáo bán hàng" },

            // Reservation & Delivery
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440160"), Name = "Reservation.View", Description = "Xem đặt bàn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440161"), Name = "Reservation.Create", Description = "Tạo đặt bàn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440162"), Name = "Reservation.Edit", Description = "Sửa đặt bàn" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440163"), Name = "Reservation.Delete", Description = "Xóa đặt bàn" },

            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440170"), Name = "OnlineOrder.Manage", Description = "Quản lý đặt món online" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440171"), Name = "Delivery.Manage", Description = "Quản lý giao hàng" },

            // Supplier Management
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440180"), Name = "Supplier.View", Description = "Xem nhà cung cấp" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440181"), Name = "Supplier.Manage", Description = "Quản lý nhà cung cấp" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440182"), Name = "Supplier.Debt.Manage", Description = "Quản lý công nợ" },

            // Reports & Analytics
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440190"), Name = "Report.Revenue.View", Description = "Xem báo cáo doanh thu" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440191"), Name = "Report.Finance.View", Description = "Xem báo cáo tài chính" },
            new Permission { PermissionId = Guid.Parse("770e8400-e29b-41d4-a716-446655440192"), Name = "Report.Analytics.View", Description = "Xem phân tích hoạt động" }
        };

        modelBuilder.Entity<Permission>().HasData(permissions);

        // Seed Staff
        var adminStaff = new Staff
        {
            StaffId = Guid.Parse("880e8400-e29b-41d4-a716-446655440001"),
            EmployeeNumber = "ADM001",
            FirstName = "Nguyễn",
            LastName = "Văn Admin",
            Email = "admin@quanlikhoahang.vn",
            PhoneNumber = "0901234567",
            Password = "admin123", // Thêm password
            RoleId = adminRole.RoleId,
            BranchId = branch1.BranchId,
            HireDate = DateTime.Now.AddYears(-2),
            EmploymentType = EmploymentType.FullTime,
            BaseSalary = 15000000,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var managerStaff = new Staff
        {
            StaffId = Guid.Parse("880e8400-e29b-41d4-a716-446655440002"),
            EmployeeNumber = "MGR001",
            FirstName = "Trần",
            LastName = "Thị Manager",
            Email = "manager@quanlikhoahang.vn",
            PhoneNumber = "0902345678",
            Password = "manager123", // Thêm password
            RoleId = managerRole.RoleId,
            BranchId = branch1.BranchId,
            HireDate = DateTime.Now.AddYears(-1),
            EmploymentType = EmploymentType.FullTime,
            BaseSalary = 12000000,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Staff>().HasData(adminStaff, managerStaff);

        // Seed Categories
        var mainDishCategory = new Category
        {
            CategoryId = Guid.Parse("990e8400-e29b-41d4-a716-446655440001"),
            Name = "Món chính",
            Description = "Các món ăn chính",
            DisplayOrder = 1,
            Active = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var drinkCategory = new Category
        {
            CategoryId = Guid.Parse("990e8400-e29b-41d4-a716-446655440002"),
            Name = "Đồ uống",
            Description = "Các loại đồ uống",
            DisplayOrder = 2,
            Active = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Category>().HasData(mainDishCategory, drinkCategory);

        // Seed Dishes
        var phoBo = new Dish
        {
            DishId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440001"),
            Name = "Phở bò",
            Description = "Phở bò truyền thống với thịt bò tươi ngon",
            CategoryId = mainDishCategory.CategoryId,
            DefaultServingPrice = 45000,
            Active = true,
            IsAvailableOnline = true,
            PreparationTimeMinutes = 15,
            IsComposite = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null,
            ImageUrl = "https://fohlafood.vn/cdn/shop/articles/bi-quyet-nau-phi-bo-ngon-tuyet-dinh.jpg?v=1712213789"
        };

        var caPheSua = new Dish
        {
            DishId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440002"),
            Name = "Cà phê sữa đá",
            Description = "Cà phê sữa đá đậm đà",
            CategoryId = drinkCategory.CategoryId,
            DefaultServingPrice = 25000,
            Active = true,
            IsAvailableOnline = true,
            PreparationTimeMinutes = 5,
            IsComposite = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = null,
            ImageUrl = "https://lyoncoffee.com.vn/wp-content/uploads/cong-thuc-pha-ca-phe-sua-da-sai-gon.jpg"
        };

        modelBuilder.Entity<Dish>().HasData(phoBo, caPheSua);

        // Seed Menu Price History
        var phoBoPrice = new MenuPriceHistory
        {
            MenuPriceId = Guid.Parse("bb0e8400-e29b-41d4-a716-446655440001"),
            DishId = phoBo.DishId,
            Price = 45000,
            EffectiveFrom = DateTime.Now.AddMonths(-6),
            EffectiveTo = null,
            ChangedByStaffId = adminStaff.StaffId,
            Reason = "Giá khởi tạo",
            CreatedAt = DateTime.Now.AddMonths(-6),
            UpdatedAt = null
        };

        var caPhePrice = new MenuPriceHistory
        {
            MenuPriceId = Guid.Parse("bb0e8400-e29b-41d4-a716-446655440002"),
            DishId = caPheSua.DishId,
            Price = 25000,
            EffectiveFrom = DateTime.Now.AddMonths(-6),
            EffectiveTo = null,
            ChangedByStaffId = adminStaff.StaffId,
            Reason = "Giá khởi tạo",
            CreatedAt = DateTime.Now.AddMonths(-6),
            UpdatedAt = null
        };

        modelBuilder.Entity<MenuPriceHistory>().HasData(phoBoPrice, caPhePrice);

        // Seed Suppliers
        var supplier1 = new Supplier
        {
            SupplierId = Guid.Parse("bb0e8400-e29b-41d4-a716-446655440009"),
            Name = "Công ty TNHH Thực phẩm ABC",
            PrimaryContactName = "Nguyễn Văn B",
            Email = "contact@thucphamabc.vn",
            Phone = "028-1111-2222",
            Address = "789 Đường Cách Mạng Tháng 8, Quận 3, TP.HCM",
            PaymentTerms = "Net 30",
            Currency = "VND",
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Supplier>().HasData(supplier1);

        // Seed Storage Locations
        var storageLocation1 = new StorageLocation
        {
            StorageLocationId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440001"),
            Name = "Kho lạnh A1",
            BranchId = branch1.BranchId,
            Type = StorageLocationType.Refrigerator,
            Condition = StorageCondition.Cold,
            Description = "Kho lạnh để bảo quản thịt, cá",
            Address = "Khu A, Tầng 1",
            Capacity = 1000m,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var storageLocation2 = new StorageLocation
        {
            StorageLocationId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440002"),
            Name = "Kho khô B2",
            BranchId = branch1.BranchId,
            Type = StorageLocationType.DryStorage,
            Condition = StorageCondition.Dry,
            Description = "Kho khô để bảo quản cà phê, đường, muối",
            Address = "Khu B, Tầng 2",
            Capacity = 2000m,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<StorageLocation>().HasData(storageLocation1, storageLocation2);

        // Seed Inventory Items
        var thitBo = new InventoryItem
        {
            ItemId = Guid.Parse("cc0e8400-e29b-41d4-a716-446655440001"),
            Sku = "THITBO001",
            Name = "Thịt bò",
            Unit = "kg",
            CurrentQuantity = 50,
            ReorderLevel = 10,
            ReorderQuantity = 25,
            CostPerUnit = 250000,
            StorageCondition = StorageCondition.Cold,
            SupplierId = supplier1.SupplierId,
            StorageLocationId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440001"),
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var caPheHat = new InventoryItem
        {
            ItemId = Guid.Parse("cc0e8400-e29b-41d4-a716-446655440002"),
            Sku = "CAFE001",
            Name = "Hạt cà phê",
            Unit = "kg",
            CurrentQuantity = 20,
            ReorderLevel = 5,
            ReorderQuantity = 10,
            CostPerUnit = 150000,
            StorageCondition = StorageCondition.Dry,
            SupplierId = supplier1.SupplierId,
            StorageLocationId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440002"),
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<InventoryItem>().HasData(thitBo, caPheHat);

        // Seed Dish Ingredients
        var phoBoThitBo = new DishIngredient
        {
            DishIngredientId = Guid.Parse("dd0e8400-e29b-41d4-a716-446655440001"),
            DishId = phoBo.DishId,
            InventoryItemId = thitBo.ItemId,
            QuantityPerPortion = 0.3m,
            UnitMultiplier = 1m,
            IsOptional = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<DishIngredient>().HasData(phoBoThitBo);

        // Seed Customers
        var customer1 = new Customer
        {
            CustomerId = Guid.Parse("ee0e8400-e29b-41d4-a716-446655440001"),
            FirstName = "Nguyễn",
            LastName = "Văn A",
            Email = "nguyenvana@gmail.com",
            PhoneNumber = "0912345678",
            DateOfBirth = new DateOnly(1990, 1, 1),
            PreferredBranchId = branch1.BranchId,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Customer>().HasData(customer1);

        // Seed Restaurant Tables
        var table1 = new RestaurantTable
        {
            TableId = Guid.Parse("ff0e8400-e29b-41d4-a716-446655440001"),
            BranchId = branch1.BranchId,
            Code = "T01",
            Name = "Bàn số 1",
            Seats = 4,
            Status = TableStatus.Available,
            IsOutdoor = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var table2 = new RestaurantTable
        {
            TableId = Guid.Parse("ff0e8400-e29b-41d4-a716-446655440002"),
            BranchId = branch1.BranchId,
            Code = "T02",
            Name = "Bàn số 2",
            Seats = 6,
            Status = TableStatus.Available,
            IsOutdoor = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<RestaurantTable>().HasData(table1, table2);

        // Seed Kitchen Sections
        var kitchenSection1 = new KitchenSection
        {
            KitchenSectionId = Guid.Parse("110e8400-e29b-41d4-a716-446655440001"),
            Name = "Bếp chính",
            Description = "Bếp nấu các món chính",
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<KitchenSection>().HasData(kitchenSection1);

        // Seed Shifts
        var morningShift = new Shift
        {
            ShiftId = Guid.Parse("220e8400-e29b-41d4-a716-446655440001"),
            BranchId = branch1.BranchId,
            Name = "Ca sáng",
            StartTime = new TimeSpan(6, 0, 0),
            EndTime = new TimeSpan(14, 0, 0),
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        var eveningShift = new Shift
        {
            ShiftId = Guid.Parse("220e8400-e29b-41d4-a716-446655440002"),
            BranchId = branch1.BranchId,
            Name = "Ca tối",
            StartTime = new TimeSpan(14, 0, 0),
            EndTime = new TimeSpan(22, 0, 0),
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Shift>().HasData(morningShift, eveningShift);

        // Seed Orders
        var order1 = new Order
        {
            OrderId = Guid.Parse("330e8400-e29b-41d4-a716-446655440001"),
            OrderNumber = "ORD001",
            BranchId = branch1.BranchId,
            TableId = table1.TableId,
            CustomerId = customer1.CustomerId,
            OrderType = OrderType.DineIn,
            OrderSource = OrderSource.Pos,
            Status = OrderStatus.Paid,
            SubTotal = 70000,
            TaxAmount = 7000,
            DiscountAmount = 0,
            ServiceChargeAmount = 0,
            RoundingAdjustment = 0,
            TotalAmount = 77000,
            PaidAmount = 77000,
            BalanceDue = 0,
            PaymentStatus = PaymentStatus.Approved,
            PlacedAt = DateTime.Now.AddHours(-2),
            CreatedByStaffId = managerStaff.StaffId,
            Notes = "Khách hàng thân thiết",
            CreatedAt = DateTime.Now.AddHours(-2),
            UpdatedAt = DateTime.Now.AddHours(-1)
        };

        modelBuilder.Entity<Order>().HasData(order1);

        // Seed Order Items
        var orderItem1 = new OrderItem
        {
            OrderItemId = Guid.Parse("440e8400-e29b-41d4-a716-446655440001"),
            OrderId = order1.OrderId,
            DishId = phoBo.DishId,
            MenuPriceId = phoBoPrice.MenuPriceId,
            Quantity = 1,
            UnitPrice = 45000,
            LineTotal = 45000,
            KitchenSectionId = kitchenSection1.KitchenSectionId,
            Status = OrderItemStatus.Served,
            RequestedAt = DateTime.Now.AddHours(-2),
            IsDiscounted = false,
            DiscountAmount = 0,
            SpecialInstructions = "Ít cay",
            CreatedAt = DateTime.Now.AddHours(-2),
            UpdatedAt = DateTime.Now.AddHours(-1)
        };

        var orderItem2 = new OrderItem
        {
            OrderItemId = Guid.Parse("440e8400-e29b-41d4-a716-446655440002"),
            OrderId = order1.OrderId,
            DishId = caPheSua.DishId,
            MenuPriceId = caPhePrice.MenuPriceId,
            Quantity = 1,
            UnitPrice = 25000,
            LineTotal = 25000,
            Status = OrderItemStatus.Served,
            RequestedAt = DateTime.Now.AddHours(-2),
            IsDiscounted = false,
            DiscountAmount = 0,
            CreatedAt = DateTime.Now.AddHours(-2),
            UpdatedAt = DateTime.Now.AddHours(-1)
        };

        modelBuilder.Entity<OrderItem>().HasData(orderItem1, orderItem2);

        // Seed Payments
        var payment1 = new Payment
        {
            PaymentId = Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
            OrderId = order1.OrderId,
            PaymentMethod = PaymentMethod.Cash,
            Amount = 77000,
            PaymentDate = DateTime.Now.AddHours(-1),
            ProcessedByStaffId = managerStaff.StaffId,
            Status = PaymentStatus.Approved,
            TransactionReference = "PAY001",
            Notes = "Thanh toán bằng tiền mặt",
            CreatedAt = DateTime.Now.AddHours(-1),
            UpdatedAt = null
        };

        modelBuilder.Entity<Payment>().HasData(payment1);

        // Seed Kitchen Tickets
        var kitchenTicket1 = new KitchenTicket
        {
            KitchenTicketId = Guid.Parse("660e8400-e29b-41d4-a716-446655440004"),
            OrderId = order1.OrderId,
            OrderItemId = orderItem1.OrderItemId,
            TicketNumber = "KT001",
            KitchenSectionId = kitchenSection1.KitchenSectionId,
            SentAt = DateTime.Now.AddHours(-2),
            ReceivedAt = DateTime.Now.AddHours(-1),
            Status = KitchenTicketStatus.Completed,
            PrintedByStaffId = managerStaff.StaffId,
            Notes = "Món chính",
            CreatedAt = DateTime.Now.AddHours(-2),
            UpdatedAt = DateTime.Now.AddHours(-1)
        };

        modelBuilder.Entity<KitchenTicket>().HasData(kitchenTicket1);

        // Seed Loyalty Account
        var loyaltyAccount1 = new LoyaltyAccount
        {
            LoyaltyAccountId = Guid.Parse("770e8400-e29b-41d4-a716-446655440005"),
            CustomerId = customer1.CustomerId,
            PointsBalance = 77,
            TotalEarnedPoints = 77,
            TotalRedeemedPoints = 0,
            Tier = LoyaltyTier.Silver,
            TierEffectiveFrom = DateTime.Now.AddMonths(-6),
            CreatedAt = DateTime.Now.AddHours(-1),
            UpdatedAt = null
        };

        modelBuilder.Entity<LoyaltyAccount>().HasData(loyaltyAccount1);

        // Seed Loyalty Transaction
        var loyaltyTransaction1 = new LoyaltyTransaction
        {
            LoyaltyTransactionId = Guid.Parse("880e8400-e29b-41d4-a716-446655440006"),
            LoyaltyAccountId = loyaltyAccount1.LoyaltyAccountId,
            OrderId = order1.OrderId,
            Type = LoyaltyTransactionType.Earn,
            Points = 77,
            Reason = "Tích điểm từ đơn hàng ORD001",
            CreatedAt = DateTime.Now.AddHours(-1),
            UpdatedAt = null
        };

        modelBuilder.Entity<LoyaltyTransaction>().HasData(loyaltyTransaction1);

        // Seed Stock Transactions
        var stockTransaction1 = new StockTransaction
        {
            StockTransactionId = Guid.Parse("990e8400-e29b-41d4-a716-446655440007"),
            ItemId = thitBo.ItemId,
            TransactionType = StockTransactionType.SaleOut,
            ReferenceId = order1.OrderId,
            Quantity = 0.3m,
            UnitCost = 250000,
            TotalCost = 75000,
            LotNumber = "LOT001",
            CreatedByStaffId = managerStaff.StaffId,
            Note = "Xuất kho cho đơn hàng ORD001",
            CreatedAt = DateTime.Now.AddHours(-1),
            UpdatedAt = null
        };

        modelBuilder.Entity<StockTransaction>().HasData(stockTransaction1);

        // Seed Attendance
        var attendance1 = new Attendance
        {
            AttendanceId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440008"),
            StaffId = managerStaff.StaffId,
            ClockInAt = DateTime.Now.AddHours(-8),
            ClockOutAt = DateTime.Now.AddHours(-1),
            ClockType = ClockType.Manual,
            Status = AttendanceStatus.Approved,
            ApprovedByStaffId = adminStaff.StaffId,
            ShiftId = morningShift.ShiftId,
            CreatedAt = DateTime.Now.AddHours(-8),
            UpdatedAt = DateTime.Now.AddHours(-1)
        };

        modelBuilder.Entity<Attendance>().HasData(attendance1);

        // Seed Purchase Order
        var purchaseOrder1 = new PurchaseOrder
        {
            PurchaseOrderId = Guid.Parse("cc0e8400-e29b-41d4-a716-446655440010"),
            PONumber = "PO001",
            SupplierId = supplier1.SupplierId,
            BranchId = branch1.BranchId,
            CreatedByStaffId = managerStaff.StaffId,
            OrderedAt = DateTime.Now.AddDays(-7),
            ExpectedDeliveryDate = DateTime.Now.AddDays(-5),
            Status = PurchaseOrderStatus.Completed,
            TotalAmount = 10000000,
            Currency = "VND",
            Notes = "Đơn hàng nhập hàng tháng 10",
            CreatedAt = DateTime.Now.AddDays(-7),
            UpdatedAt = DateTime.Now.AddDays(-5)
        };

        modelBuilder.Entity<PurchaseOrder>().HasData(purchaseOrder1);

        // Seed Purchase Order Lines
        var poLine1 = new PurchaseOrderLine
        {
            POLineId = Guid.Parse("dd0e8400-e29b-41d4-a716-446655440011"),
            PurchaseOrderId = purchaseOrder1.PurchaseOrderId,
            ItemId = thitBo.ItemId,
            QuantityOrdered = 25,
            QuantityReceived = 25,
            UnitCost = 240000,
            LineTotal = 6000000,
            CreatedAt = DateTime.Now.AddDays(-7),
            UpdatedAt = DateTime.Now.AddDays(-5)
        };

        var poLine2 = new PurchaseOrderLine
        {
            POLineId = Guid.Parse("dd0e8400-e29b-41d4-a716-446655440012"),
            PurchaseOrderId = purchaseOrder1.PurchaseOrderId,
            ItemId = caPheHat.ItemId,
            QuantityOrdered = 10,
            QuantityReceived = 10,
            UnitCost = 140000,
            LineTotal = 1400000,
            CreatedAt = DateTime.Now.AddDays(-7),
            UpdatedAt = DateTime.Now.AddDays(-5)
        };

        modelBuilder.Entity<PurchaseOrderLine>().HasData(poLine1, poLine2);

        // Seed Supplier Invoice
        var supplierInvoice1 = new SupplierInvoice
        {
            SupplierInvoiceId = Guid.Parse("ee0e8400-e29b-41d4-a716-446655440013"),
            SupplierId = supplier1.SupplierId,
            InvoiceNumber = "INV001",
            LinkedPOId = purchaseOrder1.PurchaseOrderId,
            InvoiceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(25)),
            TotalAmount = 7400000,
            BalanceDue = 0,
            Status = "Paid",
            CreatedAt = DateTime.Now.AddDays(-5),
            UpdatedAt = DateTime.Now.AddDays(-3)
        };

        modelBuilder.Entity<SupplierInvoice>().HasData(supplierInvoice1);

        // Seed Voucher
        var voucher1 = new Voucher
        {
            VoucherId = Guid.Parse("ff0e8400-e29b-41d4-a716-446655440014"),
            Code = "WELCOME10",
            Description = "Giảm 10% cho đơn hàng đầu tiên",
            DiscountType = VoucherDiscountType.Percentage,
            DiscountValue = 10,
            MinimumOrderAmount = 100000,
            ValidFrom = DateTime.Now,
            ValidTo = DateTime.Now.AddMonths(6),
            TotalUsageLimit = 100,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<Voucher>().HasData(voucher1);

        // Seed StaffShifts
        var staffShift1 = new StaffShift
        {
            StaffShiftId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440001"),
            StaffId = adminStaff.StaffId,
            ShiftId = morningShift.ShiftId,
            WorkDate = DateTime.Now.Date,
            Status = ShiftStatus.Approved,
            ApprovedAt = DateTime.Now.AddMinutes(-30),
            ApprovedByStaffId = adminStaff.StaffId,
            Notes = "Ca làm việc chính",
            CreatedAt = DateTime.Now.AddHours(-2),
            UpdatedAt = null
        };

        var staffShift2 = new StaffShift
        {
            StaffShiftId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440002"),
            StaffId = managerStaff.StaffId,
            ShiftId = eveningShift.ShiftId,
            WorkDate = DateTime.Now.Date,
            Status = ShiftStatus.Scheduled,
            Notes = "Ca làm việc phụ",
            CreatedAt = DateTime.Now.AddHours(-1),
            UpdatedAt = null
        };

        var staffShift3 = new StaffShift
        {
            StaffShiftId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440003"),
            StaffId = adminStaff.StaffId,
            ShiftId = morningShift.ShiftId,
            WorkDate = DateTime.Now.Date.AddDays(1),
            Status = ShiftStatus.Scheduled,
            Notes = "Ca làm việc ngày mai",
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        modelBuilder.Entity<StaffShift>().HasData(staffShift1, staffShift2, staffShift3);

        // Seed Sample Orders
        var sampleOrder1 = new Order
        {
            OrderId = Guid.Parse("cc0e8400-e29b-41d4-a716-446655440001"),
            OrderNumber = "ORD-20251020-001",
            BranchId = branch1.BranchId,
            TableId = table1.TableId,
            CustomerId = customer1.CustomerId,
            CreatedByStaffId = adminStaff.StaffId,
            OrderType = OrderType.DineIn,
            OrderSource = OrderSource.Pos,
            PlacedAt = DateTime.Now.AddHours(-2),
            Status = OrderStatus.InPreparation,
            SubTotal = 150000,
            DiscountAmount = 0,
            TaxAmount = 15000,
            ServiceChargeAmount = 7500,
            RoundingAdjustment = 0,
            TotalAmount = 172500,
            PaidAmount = 0,
            BalanceDue = 172500,
            PaymentStatus = PaymentStatus.Pending,
            IsVoided = false,
            CreatedAt = DateTime.Now.AddHours(-2),
            UpdatedAt = null
        };

        var sampleOrder2 = new Order
        {
            OrderId = Guid.Parse("cc0e8400-e29b-41d4-a716-446655440002"),
            OrderNumber = "ORD-20251020-002",
            BranchId = branch1.BranchId,
            TableId = null, // Take away
            CustomerId = null,
            CreatedByStaffId = adminStaff.StaffId,
            OrderType = OrderType.TakeAway,
            OrderSource = OrderSource.Pos,
            PlacedAt = DateTime.Now.AddHours(-1),
            ServedAt = DateTime.Now.AddMinutes(-30),
            Status = OrderStatus.Ready,
            SubTotal = 85000,
            DiscountAmount = 0,
            TaxAmount = 8500,
            ServiceChargeAmount = 0, // No service charge for takeaway
            RoundingAdjustment = 0,
            TotalAmount = 93500,
            PaidAmount = 93500,
            BalanceDue = 0,
            PaymentStatus = PaymentStatus.Approved,
            IsVoided = false,
            CreatedAt = DateTime.Now.AddHours(-1),
            UpdatedAt = DateTime.Now.AddMinutes(-30),
            ClosedByStaffId = adminStaff.StaffId
        };

        var sampleOrder3 = new Order
        {
            OrderId = Guid.Parse("cc0e8400-e29b-41d4-a716-446655440003"),
            OrderNumber = "ORD-20251019-001",
            BranchId = branch1.BranchId,
            TableId = table1.TableId,
            CustomerId = customer1.CustomerId,
            CreatedByStaffId = adminStaff.StaffId,
            OrderType = OrderType.DineIn,
            OrderSource = OrderSource.Pos,
            PlacedAt = DateTime.Now.AddDays(-1).AddHours(19),
            ServedAt = DateTime.Now.AddDays(-1).AddHours(20),
            Status = OrderStatus.Paid,
            SubTotal = 200000,
            DiscountAmount = 20000, // 10% discount
            TaxAmount = 18000,
            ServiceChargeAmount = 9000,
            RoundingAdjustment = 0,
            TotalAmount = 207000,
            PaidAmount = 207000,
            BalanceDue = 0,
            PaymentStatus = PaymentStatus.Approved,
            IsVoided = false,
            CreatedAt = DateTime.Now.AddDays(-1).AddHours(19),
            UpdatedAt = DateTime.Now.AddDays(-1).AddHours(20),
            ClosedByStaffId = adminStaff.StaffId
        };

        modelBuilder.Entity<Order>().HasData(sampleOrder1, sampleOrder2, sampleOrder3);

        // Seed Role-Permission Mappings
        var rolePermissions = new List<RolePermission>();

        // Admin Role - All permissions
        var adminPermissionIds = permissions.Select(p => p.PermissionId).ToList();
        foreach (var permId in adminPermissionIds)
        {
            rolePermissions.Add(new RolePermission
            {
                RolePermissionId = Guid.NewGuid(),
                RoleId = adminRole.RoleId,
                PermissionId = permId
            });
        }

        // Manager Role - Most permissions except some admin-only ones
        var managerPermissionNames = new[]
        {
            "Dashboard.View",
            "Menu.Category.View",
            "Menu.Category.Create",
            "Menu.Category.Edit",
            "Menu.Dish.View",
            "Menu.Dish.Create",
            "Menu.Dish.Edit",
            "Menu.PriceHistory.View",
            "Menu.PriceHistory.Create",
            "Menu.DishIngredient.View",
            "Menu.DishIngredient.Create",
            "Menu.DishIngredient.Edit",
            "Inventory.View",
            "Inventory.Import",
            "Inventory.Export",
            "Inventory.StockTaking",
            "Staff.View",
            "Staff.Create",
            "Staff.Edit",
            "Staff.Payroll.View",
            "Staff.Payroll.Manage",
            "Staff.Shift.View",
            "Staff.Shift.Create",
            "Staff.Shift.Edit",
            "Staff.Attendance.View",
            "Staff.Attendance.Manage",
            "Customer.View",
            "Customer.Create",
            "Customer.Edit",
            "Customer.Loyalty.Manage",
            "Customer.Voucher.Manage",
            "POS.Table.View",
            "POS.Table.Manage",
            "POS.TableManagement.View",
            "POS.Order.View",
            "POS.SalesReport.View",
            "Reservation.View",
            "Reservation.Create",
            "Reservation.Edit",
            "Report.Revenue.View",
            "Report.Finance.View"
        };

        foreach (var permName in managerPermissionNames)
        {
            var perm = permissions.FirstOrDefault(p => p.Name == permName);
            if (perm != null)
            {
                rolePermissions.Add(new RolePermission
                {
                    RolePermissionId = Guid.NewGuid(),
                    RoleId = managerRole.RoleId,
                    PermissionId = perm.PermissionId
                });
            }
        }

        // Staff Role - Limited permissions for POS and order management
        var staffPermissionNames = new[]
        {
            "Dashboard.View",
            "Menu.Dish.View",
            "POS.Table.View",
            "POS.TableManagement.View",
            "POS.Order.Create",
            "POS.Order.View",
            "Customer.View",
            "Customer.Edit"
        };

        foreach (var permName in staffPermissionNames)
        {
            var perm = permissions.FirstOrDefault(p => p.Name == permName);
            if (perm != null)
            {
                rolePermissions.Add(new RolePermission
                {
                    RolePermissionId = Guid.NewGuid(),
                    RoleId = staffRole.RoleId,
                    PermissionId = perm.PermissionId
                });
            }
        }

        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);

        // Seed Tier Configs
        var tierConfigs = new List<TierConfig>
        {
            new TierConfig
            {
                TierConfigId = Guid.Parse("bb0e8400-e29b-41d4-a716-446655440015"),
                Tier = LoyaltyTier.Bronze,
                TierName = "Đồng",
                Description = "Hạng thành viên Đồng",
                MinPoints = 0,
                MaxPoints = 99,
                Color = "secondary",
                Icon = "fas fa-medal",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
            new TierConfig
            {
                TierConfigId = Guid.Parse("bb0e8400-e29b-41d4-a716-446655440016"),
                Tier = LoyaltyTier.Silver,
                TierName = "Bạc",
                Description = "Hạng thành viên Bạc",
                MinPoints = 100,
                MaxPoints = 499,
                Color = "info",
                Icon = "fas fa-medal",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
            new TierConfig
            {
                TierConfigId = Guid.Parse("bb0e8400-e29b-41d4-a716-446655440017"),
                Tier = LoyaltyTier.Gold,
                TierName = "Vàng",
                Description = "Hạng thành viên Vàng",
                MinPoints = 500,
                MaxPoints = 999,
                Color = "warning",
                Icon = "fas fa-medal",
                DisplayOrder = 3,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
            new TierConfig
            {
                TierConfigId = Guid.Parse("bb0e8400-e29b-41d4-a716-446655440018"),
                Tier = LoyaltyTier.Platinum,
                TierName = "Bạch Kim",
                Description = "Hạng thành viên Bạch Kim",
                MinPoints = 1000,
                MaxPoints = null,
                Color = "success",
                Icon = "fas fa-crown",
                DisplayOrder = 4,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            }
        };

        modelBuilder.Entity<TierConfig>().HasData(tierConfigs);

        // Seed Loyalty Settings
        var loyaltySettings = new List<LoyaltySettings>
        {
            new LoyaltySettings
            {
                LoyaltySettingsId = Guid.NewGuid(),
                SettingKey = "MinPointsAmount",
                SettingName = "Giá trị tối thiểu để tích điểm",
                Description = "Giá trị đơn hàng tối thiểu (VND) để có thể tích điểm",
                SettingValue = "50000",
                DataType = "decimal",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
            new LoyaltySettings
            {
                LoyaltySettingsId = Guid.NewGuid(),
                SettingKey = "PointsPerAmount",
                SettingName = "Tỷ lệ tích điểm",
                Description = "Cứ bao nhiêu VND = 1 điểm (ví dụ: 10000 = 1 điểm)",
                SettingValue = "10000",
                DataType = "decimal",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            }
        };

        modelBuilder.Entity<LoyaltySettings>().HasData(loyaltySettings);
    }
}