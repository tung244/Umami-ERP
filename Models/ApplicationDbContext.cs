using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
      public DbSet<Order> Orders => Set<Order>();
      public DbSet<OrderItem> OrderItems => Set<OrderItem>();
      public DbSet<Payment> Payments => Set<Payment>();
      public DbSet<Refund> Refunds => Set<Refund>();
      public DbSet<Dish> Dishes => Set<Dish>();
      public DbSet<Category> Categories => Set<Category>();
      public DbSet<DishIngredient> DishIngredients => Set<DishIngredient>();
      public DbSet<MenuPriceHistory> MenuPriceHistory => Set<MenuPriceHistory>();
      public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
      public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
      public DbSet<StorageLocation> StorageLocations => Set<StorageLocation>();
      public DbSet<StorageItem> StorageItems => Set<StorageItem>();
      public DbSet<StockAlertThreshold> StockAlertThresholds => Set<StockAlertThreshold>();
      public DbSet<StockAlertNotification> StockAlertNotifications => Set<StockAlertNotification>();
      public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
      public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
      public DbSet<Staff> Staff => Set<Staff>();
      public DbSet<Role> Roles => Set<Role>();
      public DbSet<Permission> Permissions => Set<Permission>();
      public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
      public DbSet<StaffPermission> StaffPermissions => Set<StaffPermission>();
      public DbSet<Shift> Shifts => Set<Shift>();
      public DbSet<StaffShift> StaffShifts => Set<StaffShift>();
      public DbSet<Attendance> Attendances => Set<Attendance>();
      public DbSet<PayrollRecord> PayrollRecords => Set<PayrollRecord>();
      public DbSet<Customer> Customers => Set<Customer>();
      public DbSet<LoyaltyAccount> LoyaltyAccounts => Set<LoyaltyAccount>();
      public DbSet<LoyaltyTransaction> LoyaltyTransactions => Set<LoyaltyTransaction>();
      public DbSet<Voucher> Vouchers => Set<Voucher>();
      public DbSet<Branch> Branches => Set<Branch>();
      public DbSet<RestaurantTable> RestaurantTables => Set<RestaurantTable>();
      public DbSet<KitchenSection> KitchenSections => Set<KitchenSection>();
      public DbSet<KitchenTicket> KitchenTickets => Set<KitchenTicket>();
      public DbSet<Reservation> Reservations => Set<Reservation>();
      public DbSet<DeliveryOrder> DeliveryOrders => Set<DeliveryOrder>();
      public DbSet<Driver> Drivers => Set<Driver>();
      public DbSet<Supplier> Suppliers => Set<Supplier>();
      public DbSet<SupplierInvoice> SupplierInvoices => Set<SupplierInvoice>();
      public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();
      public DbSet<AccountReceivable> AccountsReceivable => Set<AccountReceivable>();
      public DbSet<AccountPayable> AccountsPayable => Set<AccountPayable>();
      public DbSet<Expense> Expenses => Set<Expense>();
      public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
      public DbSet<Device> Devices => Set<Device>();
      public DbSet<ApplicationSetting> ApplicationSettings => Set<ApplicationSetting>();
      public DbSet<StockCount> StockCounts => Set<StockCount>();
      public DbSet<StockCountLine> StockCountLines => Set<StockCountLine>();
      public DbSet<LoyaltySettings> LoyaltySettings => Set<LoyaltySettings>();
      public DbSet<TierConfig> TierConfigs => Set<TierConfig>();
      public DbSet<Benefit> Benefits => Set<Benefit>();
      public DbSet<TierBenefit> TierBenefits => Set<TierBenefit>();
      public DbSet<RedemptionRule> RedemptionRules => Set<RedemptionRule>();
      public DbSet<ReportsCache> ReportsCaches => Set<ReportsCache>();

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                  entity.HasKey(e => e.OrderId);
                  entity.Property(e => e.OrderNumber).HasMaxLength(64);
                  entity.HasIndex(e => new { e.BranchId, e.OrderNumber }).IsUnique();
                  entity.HasIndex(e => e.PlacedAt);
                  entity.HasIndex(e => e.Status);
                  entity.HasOne(e => e.Branch)
                    .WithMany(b => b.Orders)
                    .HasForeignKey(e => e.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.Table)
                    .WithMany(t => t.Orders)
                    .HasForeignKey(e => e.TableId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.CreatedByStaff)
                    .WithMany(s => s.CreatedOrders)
                    .HasForeignKey(e => e.CreatedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.ClosedByStaff)
                    .WithMany(s => s.ClosedOrders)
                    .HasForeignKey(e => e.ClosedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                  entity.HasKey(e => e.OrderItemId);
                  entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId);
                  entity.HasOne(e => e.Dish)
                    .WithMany(d => d.OrderItems)
                    .HasForeignKey(e => e.DishId);
                  entity.HasOne(e => e.MenuPrice)
                    .WithMany(d => d.OrderItems)
                    .HasForeignKey(e => e.MenuPriceId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.KitchenSection)
                    .WithMany(k => k.OrderItems)
                    .HasForeignKey(e => e.KitchenSectionId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.ParentOrderItem)
                    .WithMany(e => e.ChildOrderItems)
                    .HasForeignKey(e => e.ParentOrderItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MenuPriceHistory>(entity =>
            {
                  entity.HasKey(e => e.MenuPriceId);
                  entity.HasOne(e => e.Dish)
                    .WithMany(d => d.PriceHistory)
                    .HasForeignKey(e => e.DishId);
                  entity.HasOne(e => e.ChangedByStaff)
                    .WithMany()
                    .HasForeignKey(e => e.ChangedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                  entity.HasKey(e => e.CategoryId);
                  entity.HasOne(e => e.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                  entity.HasKey(e => e.ItemId);
                  entity.HasIndex(e => e.Name);
                  entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.InventoryItems)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                  entity.HasKey(e => e.PurchaseOrderId);
                  entity.HasIndex(e => new { e.SupplierId, e.Status });
                  entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.PurchaseOrders)
                    .HasForeignKey(e => e.SupplierId);
                  entity.HasOne(e => e.Branch)
                    .WithMany(b => b.PurchaseOrders)
                    .HasForeignKey(e => e.BranchId);
                  entity.HasOne(e => e.CreatedByStaff)
                    .WithMany(s => s.PurchaseOrders)
                    .HasForeignKey(e => e.CreatedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PurchaseOrderLine>(entity =>
            {
                  entity.HasKey(e => e.POLineId);
                  entity.HasOne(e => e.PurchaseOrder)
                    .WithMany(p => p.Lines)
                    .HasForeignKey(e => e.PurchaseOrderId);
                  entity.HasOne(e => e.Item)
                    .WithMany(i => i.PurchaseOrderLines)
                    .HasForeignKey(e => e.ItemId);
            });

            modelBuilder.Entity<StockTransaction>(entity =>
            {
                  entity.HasKey(e => e.StockTransactionId);
                  entity.HasOne(e => e.Item)
                    .WithMany(i => i.StockTransactions)
                    .HasForeignKey(e => e.ItemId);
                  entity.HasOne(e => e.CreatedByStaff)
                    .WithMany(s => s.StockTransactions)
                    .HasForeignKey(e => e.CreatedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StockCount>(entity =>
            {
                  entity.HasKey(e => e.StockCountId);
                  entity.HasOne(e => e.Branch)
                    .WithMany(b => b.StockCounts)
                    .HasForeignKey(e => e.BranchId);
                  entity.HasOne(e => e.PerformedByStaff)
                    .WithMany()
                    .HasForeignKey(e => e.PerformedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StockCountLine>(entity =>
            {
                  entity.HasKey(e => e.StockCountLineId);
                  entity.HasOne(e => e.StockCount)
                    .WithMany(c => c.Lines)
                    .HasForeignKey(e => e.StockCountId);
                  entity.HasOne(e => e.Item)
                    .WithMany(i => i.StockCountLines)
                    .HasForeignKey(e => e.ItemId);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                  entity.HasKey(e => e.StaffId);
                  entity.HasIndex(e => e.EmployeeNumber).IsUnique();
                  entity.HasOne(e => e.Role)
                    .WithMany(r => r.StaffMembers)
                    .HasForeignKey(e => e.RoleId);
                  entity.HasOne(e => e.Branch)
                    .WithMany(b => b.StaffMembers)
                    .HasForeignKey(e => e.BranchId);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                  entity.HasKey(e => new { e.RoleId, e.PermissionId });
                  entity.HasOne(e => e.Role)
                    .WithMany(r => r.Permissions)
                    .HasForeignKey(e => e.RoleId);
                  entity.HasOne(e => e.Permission)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(e => e.PermissionId);
            });

            modelBuilder.Entity<StaffPermission>(entity =>
            {
                  entity.HasKey(e => new { e.StaffId, e.PermissionId });
                  entity.HasOne(e => e.Staff)
                    .WithMany(s => s.Permissions)
                    .HasForeignKey(e => e.StaffId);
                  entity.HasOne(e => e.Permission)
                    .WithMany(p => p.StaffMembers)
                    .HasForeignKey(e => e.PermissionId);
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                  entity.HasKey(e => e.AttendanceId);
                  entity.HasOne(e => e.Staff)
                    .WithMany(s => s.Attendances)
                    .HasForeignKey(e => e.StaffId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.ApprovedByStaff)
                    .WithMany()
                    .HasForeignKey(e => e.ApprovedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.Shift)
                    .WithMany(s => s.Attendances)
                    .HasForeignKey(e => e.ShiftId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StaffShift>(entity =>
            {
                  entity.HasKey(e => e.StaffShiftId);
                  entity.HasIndex(e => new { e.StaffId, e.WorkDate }).IsUnique();
                  entity.HasOne(e => e.Staff)
                    .WithMany()
                    .HasForeignKey(e => e.StaffId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.Shift)
                    .WithMany()
                    .HasForeignKey(e => e.ShiftId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.ApprovedByStaff)
                    .WithMany()
                    .HasForeignKey(e => e.ApprovedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                  entity.HasKey(e => e.CustomerId);
                  entity.HasIndex(e => e.Email).IsUnique(false);
                  entity.HasIndex(e => e.PhoneNumber).IsUnique(false);
                  entity.HasOne(e => e.PreferredBranch)
                    .WithMany()
                    .HasForeignKey(e => e.PreferredBranchId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LoyaltyAccount>(entity =>
            {
                  entity.HasKey(e => e.LoyaltyAccountId);
                  entity.HasOne(e => e.Customer)
                    .WithMany(c => c.LoyaltyAccounts)
                    .HasForeignKey(e => e.CustomerId);
            });

            modelBuilder.Entity<LoyaltyTransaction>(entity =>
            {
                  entity.HasKey(e => e.LoyaltyTransactionId);
                  entity.HasOne(e => e.LoyaltyAccount)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(e => e.LoyaltyAccountId);
                  entity.HasOne(e => e.Order)
                    .WithMany(o => o.LoyaltyTransactions)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                  entity.HasKey(e => e.VoucherId);
                  entity.HasIndex(e => e.Code).IsUnique();
            });

            modelBuilder.Entity<RestaurantTable>(entity =>
            {
                  entity.HasKey(e => e.TableId);
                  entity.HasOne(e => e.Branch)
                    .WithMany(b => b.Tables)
                    .HasForeignKey(e => e.BranchId);
            });

            modelBuilder.Entity<KitchenSection>(entity =>
            {
                  entity.HasKey(e => e.KitchenSectionId);
                  entity.HasOne(e => e.PrinterDevice)
                    .WithMany()
                    .HasForeignKey(e => e.PrinterDeviceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<KitchenTicket>(entity =>
            {
                  entity.HasKey(e => e.KitchenTicketId);
                  entity.HasOne(e => e.Order)
                    .WithMany(o => o.KitchenTickets)
                    .HasForeignKey(e => e.OrderId);
                  entity.HasOne(e => e.OrderItem)
                    .WithMany(i => i.KitchenTickets)
                    .HasForeignKey(e => e.OrderItemId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.KitchenSection)
                    .WithMany(k => k.Tickets)
                    .HasForeignKey(e => e.KitchenSectionId);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                  entity.HasKey(e => e.ReservationId);
                  entity.HasOne(e => e.Branch)
                    .WithMany(b => b.Reservations)
                    .HasForeignKey(e => e.BranchId);
                  entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Reservations)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.ReservedTable)
                    .WithMany(t => t.Reservations)
                    .HasForeignKey(e => e.ReservedTableId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DeliveryOrder>(entity =>
            {
                  entity.HasKey(e => e.DeliveryOrderId);
                  entity.HasOne(e => e.Order)
                    .WithMany(o => o.DeliveryOrders)
                    .HasForeignKey(e => e.OrderId);
                  entity.HasOne(e => e.Driver)
                    .WithMany(d => d.DeliveryOrders)
                    .HasForeignKey(e => e.DriverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                  entity.HasKey(e => e.PaymentId);
                  entity.HasOne(e => e.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.ProcessedByStaff)
                    .WithMany(s => s.ProcessedPayments)
                    .HasForeignKey(e => e.ProcessedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                  entity.HasKey(e => e.RefundId);
                  entity.HasOne(e => e.OriginalPayment)
                    .WithMany(p => p.Refunds)
                    .HasForeignKey(e => e.OriginalPaymentId);
                  entity.HasOne(e => e.Order)
                    .WithMany(o => o.Refunds)
                    .HasForeignKey(e => e.OrderId);
                  entity.HasOne(e => e.ProcessedByStaff)
                    .WithMany(s => s.ProcessedRefunds)
                    .HasForeignKey(e => e.ProcessedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                  entity.HasKey(e => e.DriverId);
            });

            modelBuilder.Entity<SupplierInvoice>(entity =>
            {
                  entity.HasKey(e => e.SupplierInvoiceId);
                  entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.SupplierInvoices)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
                  entity.HasOne(e => e.LinkedPurchaseOrder)
                    .WithMany(p => p.SupplierInvoices)
                    .HasForeignKey(e => e.LinkedPOId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FinancialTransaction>(entity =>
            {
                  entity.HasKey(e => e.TransactionId);
                  entity.HasOne(e => e.Branch)
                    .WithMany()
                    .HasForeignKey(e => e.BranchId);
                  entity.HasOne(e => e.CreatedByStaff)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AccountReceivable>(entity =>
            {
                  entity.HasKey(e => e.ARId);
            });

            modelBuilder.Entity<AccountPayable>(entity =>
            {
                  entity.HasKey(e => e.APId);
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                  entity.HasKey(e => e.ExpenseId);
                  entity.HasOne(e => e.Branch)
                    .WithMany()
                    .HasForeignKey(e => e.BranchId);
                  entity.HasOne(e => e.CreatedByStaff)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                  entity.HasKey(e => e.AuditId);
                  entity.HasOne(e => e.ChangedByStaff)
                    .WithMany()
                    .HasForeignKey(e => e.ChangedByStaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Device>(entity =>
            {
                  entity.HasKey(e => e.DeviceId);
                  entity.HasOne(e => e.Branch)
                    .WithMany()
                    .HasForeignKey(e => e.BranchId);
            });

            modelBuilder.Entity<ApplicationSetting>(entity =>
            {
                  entity.HasKey(e => e.SettingKey);
                  entity.HasOne(e => e.Branch)
                    .WithMany()
                    .HasForeignKey(e => e.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LoyaltySettings>(entity =>
            {
                  entity.HasKey(e => e.LoyaltySettingsId);
                  entity.HasIndex(e => e.SettingKey).IsUnique();
            });

            modelBuilder.Entity<TierConfig>(entity =>
            {
                  entity.HasKey(e => e.TierConfigId);
                  entity.HasIndex(e => e.Tier).IsUnique();
            });

            modelBuilder.Entity<Benefit>(entity =>
            {
                  entity.HasKey(e => e.BenefitId);
                  entity.HasIndex(e => e.BenefitType);
            });

            modelBuilder.Entity<TierBenefit>(entity =>
            {
                  entity.HasKey(e => e.TierBenefitId);
                  entity.HasOne(e => e.TierConfig)
                    .WithMany(t => t.TierBenefits)
                    .HasForeignKey(e => e.TierConfigId);
                  entity.HasOne(e => e.Benefit)
                    .WithMany(b => b.TierBenefits)
                    .HasForeignKey(e => e.BenefitId);
            });

            modelBuilder.Entity<RedemptionRule>(entity =>
            {
                  entity.HasKey(e => e.RedemptionRuleId);
                  entity.HasIndex(e => e.RewardType);
            });

            modelBuilder.Entity<ReportsCache>(entity =>
            {
                  entity.HasKey(e => e.ReportId);
                  entity.HasIndex(e => e.ReportType);
                  entity.HasIndex(e => e.Period);
                  entity.HasIndex(e => e.GeneratedAt);
                  entity.HasOne(e => e.Branch)
                    .WithMany()
                    .HasForeignKey(e => e.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            modelBuilder.Seed();
      }
}
