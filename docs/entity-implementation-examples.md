# 📋 Entity Implementation Examples - QuanLiKhoHang

## 🚀 C# Entity Classes với EF Core Annotations

Dưới đây là các ví dụ về cách implement entities từ specifications sang C# code với Entity Framework Core.

### Order Entity Implementation

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities.OrderingEntities
{
    /// <summary>
    /// Đơn hàng - Core entity cho hệ thống POS
    /// </summary>
    [Table("Orders")]
    [Index(nameof(BranchId), nameof(OrderNumber), IsUnique = true, Name = "IX_Order_Branch_OrderNumber")]
    [Index(nameof(PlacedAt), Name = "IX_Order_PlacedAt")]
    [Index(nameof(Status), Name = "IX_Order_Status")]
    public class Order : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Branch")]
        public Guid BranchId { get; set; }

        [ForeignKey("Table")]
        public Guid? TableId { get; set; }

        [ForeignKey("Customer")]
        public Guid? CustomerId { get; set; }

        [Required]
        [ForeignKey("CreatedByStaff")]
        public Guid CreatedByStaffId { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public OrderSource OrderSource { get; set; }

        [Required]
        public DateTime PlacedAt { get; set; }

        public DateTime? ServedAt { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServiceChargeAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RoundingAdjustment { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceDue { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public bool IsVoided { get; set; } = false;

        public string? VoidReason { get; set; }

        public string? Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("ClosedByStaff")]
        public Guid? ClosedByStaffId { get; set; }

        // Navigation Properties
        public virtual Branch? Branch { get; set; }
        public virtual RestaurantTable? Table { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Staff? CreatedByStaff { get; set; }
        public virtual Staff? ClosedByStaff { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<KitchenTicket> KitchenTickets { get; set; } = new List<KitchenTicket>();
    }
}
```

### OrderItem Entity Implementation

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities.OrderingEntities
{
    /// <summary>
    /// Item trong đơn hàng
    /// </summary>
    [Table("OrderItems")]
    [Index(nameof(OrderId), Name = "IX_OrderItem_OrderId")]
    [Index(nameof(DishId), Name = "IX_OrderItem_DishId")]
    public class OrderItem : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderItemId { get; set; }

        [Required]
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [Required]
        [ForeignKey("Dish")]
        public Guid DishId { get; set; }

        [Required]
        [ForeignKey("MenuPrice")]
        public Guid MenuPriceId { get; set; }

        [ForeignKey("ParentOrderItem")]
        public Guid? ParentOrderItemId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }

        [Required]
        public OrderItemStatus Status { get; set; }

        [ForeignKey("KitchenSection")]
        public Guid? KitchenSectionId { get; set; }

        [Required]
        public DateTime RequestedAt { get; set; }

        public DateTime? ServedAt { get; set; }

        [Required]
        public bool IsDiscounted { get; set; } = false;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        public string? SpecialInstructions { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Order? Order { get; set; }
        public virtual Dish? Dish { get; set; }
        public virtual MenuPriceHistory? MenuPrice { get; set; }
        public virtual OrderItem? ParentOrderItem { get; set; }
        public virtual KitchenSection? KitchenSection { get; set; }

        public virtual ICollection<OrderItem> ChildOrderItems { get; set; } = new List<OrderItem>();
    }
}
```

### Payment Entity Implementation

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanLiKhoHang.Models.Entities;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities.OrderingEntities
{
    /// <summary>
    /// Thanh toán cho đơn hàng
    /// </summary>
    [Table("Payments")]
    [Index(nameof(OrderId), Name = "IX_Payment_OrderId")]
    public class Payment : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PaymentId { get; set; }

        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }

        [MaxLength(100)]
        public string? TransactionReference { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [MaxLength(50)]
        public string? CardType { get; set; }

        [MaxLength(4)]
        public string? CardLast4 { get; set; }

        [MaxLength(50)]
        public string? AuthCode { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

        [ForeignKey("ProcessedByStaff")]
        public Guid? ProcessedByStaffId { get; set; }

        public string? Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Order? Order { get; set; }
        public virtual Staff? ProcessedByStaff { get; set; }
    }
}
```

## 🔧 EF Core Configuration

### DbContext Configuration

```csharp
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Entities;

namespace QuanLiKhoHang.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Ordering Module
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Refund> Refunds { get; set; } = null!;
        public DbSet<KitchenTicket> KitchenTickets { get; set; } = null!;

        // Menu Module
        public DbSet<Dish> Dishes { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<DishIngredient> DishIngredients { get; set; } = null!;
        public DbSet<MenuPriceHistory> MenuPriceHistory { get; set; } = null!;

        // Inventory Module
        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
        public DbSet<StockTransaction> StockTransactions { get; set; } = null!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; } = null!;

        // Staff Module
        public DbSet<Staff> Staff { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<StaffPermission> StaffPermissions { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<PayrollRecord> PayrollRecords { get; set; } = null!;

        // Customer Module
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<LoyaltyAccount> LoyaltyAccounts { get; set; } = null!;
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; } = null!;
        public DbSet<Voucher> Vouchers { get; set; } = null!;

        // Branch & Operations
        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<RestaurantTable> RestaurantTables { get; set; } = null!;
        public DbSet<KitchenSection> KitchenSections { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;

        // Finance
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<SupplierInvoice> SupplierInvoices { get; set; } = null!;
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; } = null!;
        public DbSet<AccountReceivable> AccountsReceivable { get; set; } = null!;
        public DbSet<AccountPayable> AccountsPayable { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;

        // System
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Device> Devices { get; set; } = null!;
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; } = null!;
        public DbSet<StockCount> StockCounts { get; set; } = null!;
        public DbSet<StockCountLine> StockCountLines { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
```

### Migration Commands

```bash
# Tạo migration mới
dotnet ef migrations add InitialCreate

# Cập nhật database
dotnet ef database update

# Tạo script SQL
dotnet ef migrations script

# Xem trạng thái migration
dotnet ef migrations list
```