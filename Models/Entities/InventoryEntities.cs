using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

[Table("InventoryItems")]
[Index(nameof(Sku), nameof(SupplierId), IsUnique = true, Name = "IX_InventoryItem_Sku_Supplier")]
[Index(nameof(Name), Name = "IX_InventoryItem_Name")]
[Index(nameof(IsActive), Name = "IX_InventoryItem_IsActive")]
public class InventoryItem : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ItemId { get; set; }

    [MaxLength(64)]
    public string? Sku { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(32)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal CurrentQuantity { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal ReorderLevel { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal ReorderQuantity { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal CostPerUnit { get; set; } = 0m;

    public int? ShelfLifeDays { get; set; }

    [Required]
    public StorageCondition StorageCondition { get; set; } = StorageCondition.RoomTemperature;

    [Required]
    public bool IsActive { get; set; } = true;

    [ForeignKey("Supplier")]
    public Guid? SupplierId { get; set; }

    [ForeignKey("StorageLocation")]
    public Guid? StorageLocationId { get; set; }

    [MaxLength(256)]
    public string? ImageUrl { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Supplier? Supplier { get; set; }
    public virtual StorageLocation? StorageLocation { get; set; }
    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
    public virtual ICollection<StockCountLine> StockCountLines { get; set; } = new List<StockCountLine>();
}

[Table("StockTransactions")]
[Index(nameof(ItemId), Name = "IX_StockTransaction_ItemId")]
[Index(nameof(TransactionType), Name = "IX_StockTransaction_TransactionType")]
[Index(nameof(CreatedAt), Name = "IX_StockTransaction_CreatedAt")]
public class StockTransaction : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StockTransactionId { get; set; }

    [Required]
    [ForeignKey("Item")]
    public Guid ItemId { get; set; }

    [Required]
    public StockTransactionType TransactionType { get; set; }

    public Guid? ReferenceId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; } = 0m;

    [Column(TypeName = "decimal(18,4)")]
    public decimal? UnitCost { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal? TotalCost { get; set; }

    [MaxLength(64)]
    public string? LotNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [ForeignKey("CreatedByStaff")]
    public Guid? CreatedByStaffId { get; set; }
     [ForeignKey("Supplier")]
    public Guid? SupplierId { get; set; }


    public string? Note { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual InventoryItem? Item { get; set; }
    public virtual Staff? CreatedByStaff { get; set; }
}

[Table("PurchaseOrders")]
[Index(nameof(PONumber), IsUnique = true, Name = "IX_PurchaseOrder_PONumber")]
[Index(nameof(SupplierId), Name = "IX_PurchaseOrder_SupplierId")]
[Index(nameof(BranchId), Name = "IX_PurchaseOrder_BranchId")]
[Index(nameof(Status), Name = "IX_PurchaseOrder_Status")]
[Index(nameof(OrderedAt), Name = "IX_PurchaseOrder_OrderedAt")]
public class PurchaseOrder : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PurchaseOrderId { get; set; }

    [Required]
    [MaxLength(64)]
    public string PONumber { get; set; } = string.Empty;

    [Required]
    [ForeignKey("Supplier")]
    public Guid SupplierId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [ForeignKey("CreatedByStaff")]
    public Guid? CreatedByStaffId { get; set; }

    [Required]
    public DateTime OrderedAt { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    [Required]
    public PurchaseOrderStatus Status { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; } = 0m;

    [Required]
    [MaxLength(8)]
    public string Currency { get; set; } = string.Empty;

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Supplier? Supplier { get; set; }
    public virtual Branch? Branch { get; set; }
    public virtual Staff? CreatedByStaff { get; set; }
    public virtual ICollection<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();
    public virtual ICollection<SupplierInvoice> SupplierInvoices { get; set; } = new List<SupplierInvoice>();
}

[Table("PurchaseOrderLines")]
[Index(nameof(PurchaseOrderId), Name = "IX_PurchaseOrderLine_PurchaseOrderId")]
[Index(nameof(ItemId), Name = "IX_PurchaseOrderLine_ItemId")]
public class PurchaseOrderLine : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid POLineId { get; set; }

    [Required]
    [ForeignKey("PurchaseOrder")]
    public Guid PurchaseOrderId { get; set; }

    [Required]
    [ForeignKey("Item")]
    public Guid ItemId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal QuantityOrdered { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal QuantityReceived { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal UnitCost { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal LineTotal { get; set; } = 0m;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
    public virtual InventoryItem? Item { get; set; }
}

[Table("StockCounts")]
[Index(nameof(BranchId), Name = "IX_StockCount_BranchId")]
[Index(nameof(PerformedByStaffId), Name = "IX_StockCount_PerformedByStaffId")]
[Index(nameof(CountDate), Name = "IX_StockCount_CountDate")]
[Index(nameof(Status), Name = "IX_StockCount_Status")]
public class StockCount : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StockCountId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [ForeignKey("PerformedByStaff")]
    public Guid? PerformedByStaffId { get; set; }

    [Required]
    public DateTime CountDate { get; set; }

    [Required]
    public StockCountStatus Status { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
    public virtual Staff? PerformedByStaff { get; set; }
    public virtual ICollection<StockCountLine> Lines { get; set; } = new List<StockCountLine>();
}

[Table("StockCountLines")]
[Index(nameof(StockCountId), Name = "IX_StockCountLine_StockCountId")]
[Index(nameof(ItemId), Name = "IX_StockCountLine_ItemId")]
public class StockCountLine : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StockCountLineId { get; set; }

    [Required]
    [ForeignKey("StockCount")]
    public Guid StockCountId { get; set; }

    [Required]
    [ForeignKey("Item")]
    public Guid ItemId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal CountedQuantity { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal SystemQuantity { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Variance { get; set; } = 0m;

    public string? Remark { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual StockCount? StockCount { get; set; }
    public virtual InventoryItem? Item { get; set; }
}

[Table("StorageLocations")]
[Index(nameof(BranchId), Name = "IX_StorageLocation_BranchId")]
[Index(nameof(IsActive), Name = "IX_StorageLocation_IsActive")]
public class StorageLocation : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StorageLocationId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [Required]
    public StorageLocationType Type { get; set; }

    [Required]
    public StorageCondition Condition { get; set; }

    [MaxLength(256)]
    public string? Description { get; set; }

    [MaxLength(256)]
    public string? Address { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal? Capacity { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
    public virtual ICollection<StorageItem> StorageItems { get; set; } = new List<StorageItem>();
    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}

[Table("StorageItems")]
[Index(nameof(StorageLocationId), Name = "IX_StorageItem_StorageLocationId")]
[Index(nameof(InventoryItemId), Name = "IX_StorageItem_InventoryItemId")]
[Index(nameof(ExpiryDate), Name = "IX_StorageItem_ExpiryDate")]
public class StorageItem : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StorageItemId { get; set; }

    [Required]
    [ForeignKey("StorageLocation")]
    public Guid StorageLocationId { get; set; }

    [Required]
    [ForeignKey("InventoryItem")]
    public Guid InventoryItemId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; } = 0m;

    [MaxLength(64)]
    public string? LotNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal? UnitCost { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual StorageLocation? StorageLocation { get; set; }
    public virtual InventoryItem? InventoryItem { get; set; }
}

[Table("StockAlertThresholds")]
[Index(nameof(ItemId), IsUnique = true, Name = "IX_StockAlertThreshold_ItemId")]
public class StockAlertThreshold : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ThresholdId { get; set; }

    [Required]
    [ForeignKey("InventoryItem")]
    public Guid ItemId { get; set; }

    /// <summary>
    /// Mức tối thiểu để kích hoạt cảnh báo
    /// Nếu = 0, sử dụng giá trị global mặc định
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal MinimumThreshold { get; set; } = 0m;

    /// <summary>
    /// Danh sách email nhận cảnh báo, cách nhau bằng dấu phẩy
    /// </summary>
    [MaxLength(512)]
    public string? AlertEmails { get; set; }

    /// <summary>
    /// Bật/tắt cảnh báo cho sản phẩm này
    /// </summary>
    [Required]
    public bool IsEnabled { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual InventoryItem? InventoryItem { get; set; }
}

[Table("StockAlertNotifications")]
[Index(nameof(ItemId), Name = "IX_StockAlertNotification_ItemId")]
[Index(nameof(CreatedAt), Name = "IX_StockAlertNotification_CreatedAt")]
[Index(nameof(NotificationStatus), Name = "IX_StockAlertNotification_Status")]
public class StockAlertNotification : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid NotificationId { get; set; }

    [Required]
    [ForeignKey("InventoryItem")]
    public Guid ItemId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal CurrentQuantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal ThresholdQuantity { get; set; }

    [MaxLength(512)]
    public string? RecipientEmails { get; set; }

    /// <summary>
    /// Pending, Sent, Failed
    /// </summary>
    [MaxLength(32)]
    public string NotificationStatus { get; set; } = "Pending";

    public string? ErrorMessage { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? SentAt { get; set; }

    // Navigation Properties
    public virtual InventoryItem? InventoryItem { get; set; }
}

