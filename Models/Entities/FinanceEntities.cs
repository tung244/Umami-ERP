using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

[Table("Suppliers")]
[Index(nameof(Name), Name = "IX_Supplier_Name")]
[Index(nameof(Email), Name = "IX_Supplier_Email")]
[Index(nameof(IsActive), Name = "IX_Supplier_IsActive")]
[Index(nameof(SupplierCode), IsUnique = true, Name = "IX_Supplier_SupplierCode")]
public class Supplier : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid SupplierId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(32)]
    public string? SupplierCode { get; set; } // Mã nhà cung cấp duy nhất

    [MaxLength(128)]
    public string? PrimaryContactName { get; set; }

    [MaxLength(32)]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng 0")]
    public string? Phone { get; set; }

    [MaxLength(128)]
    public string? Email { get; set; }

    public string? Address { get; set; }

    [MaxLength(32)]
    public string? PaymentTerms { get; set; }

    [MaxLength(8)]
    public string? Currency { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public virtual ICollection<SupplierInvoice> SupplierInvoices { get; set; } = new List<SupplierInvoice>();
}

[Table("SupplierInvoices")]
[Index(nameof(SupplierId), Name = "IX_SupplierInvoice_SupplierId")]
[Index(nameof(InvoiceNumber), IsUnique = true, Name = "IX_SupplierInvoice_InvoiceNumber")]
[Index(nameof(InvoiceDate), Name = "IX_SupplierInvoice_InvoiceDate")]
[Index(nameof(DueDate), Name = "IX_SupplierInvoice_DueDate")]
[Index(nameof(Status), Name = "IX_SupplierInvoice_Status")]
public class SupplierInvoice : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid SupplierInvoiceId { get; set; }

    [Required]
    [ForeignKey("Supplier")]
    public Guid SupplierId { get; set; }

    [Required]
    [MaxLength(64)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    public DateOnly InvoiceDate { get; set; }

    [Required]
    public DateOnly DueDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceDue { get; set; } = 0m;

    [Required]
    [MaxLength(32)]
    public string Status { get; set; } = string.Empty;

    [ForeignKey("LinkedPurchaseOrder")]
    public Guid? LinkedPOId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Supplier? Supplier { get; set; }
    public virtual PurchaseOrder? LinkedPurchaseOrder { get; set; }
}

[Table("FinancialTransactions")]
[Index(nameof(BranchId), Name = "IX_FinancialTransaction_BranchId")]
[Index(nameof(Type), Name = "IX_FinancialTransaction_Type")]
[Index(nameof(Account), Name = "IX_FinancialTransaction_Account")]
[Index(nameof(Date), Name = "IX_FinancialTransaction_Date")]
[Index(nameof(CreatedByStaffId), Name = "IX_FinancialTransaction_CreatedByStaffId")]
public class FinancialTransaction : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TransactionId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [Required]
    public FinancialTransactionType Type { get; set; }

    public Guid? RelatedId { get; set; }

    [Required]
    [MaxLength(64)]
    public string Account { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0m;

    [Required]
    [MaxLength(8)]
    public string Currency { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    public string? Description { get; set; }

    [ForeignKey("CreatedByStaff")]
    public Guid? CreatedByStaffId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
    public virtual Staff? CreatedByStaff { get; set; }
}

[Table("ReportsCache")]
[Index(nameof(ReportType), Name = "IX_ReportsCache_ReportType")]
[Index(nameof(Period), Name = "IX_ReportsCache_Period")]
[Index(nameof(GeneratedAt), Name = "IX_ReportsCache_GeneratedAt")]
public class ReportsCache : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ReportId { get; set; }

    [Required]
    [MaxLength(64)]
    public string ReportType { get; set; } = string.Empty;

    [Required]
    [MaxLength(16)]
    public string Period { get; set; } = string.Empty;

    [ForeignKey("Branch")]
    public Guid? BranchId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string DataJson { get; set; } = string.Empty;

    [Required]
    public DateTime GeneratedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
}

[Table("AccountReceivables")]
[Index(nameof(ReferenceId), Name = "IX_AccountReceivable_ReferenceId")]
[Index(nameof(PartyId), Name = "IX_AccountReceivable_PartyId")]
[Index(nameof(DueDate), Name = "IX_AccountReceivable_DueDate")]
[Index(nameof(Status), Name = "IX_AccountReceivable_Status")]
public class AccountReceivable : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ARId { get; set; }

    [Required]
    public Guid ReferenceId { get; set; }

    public Guid? PartyId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountDue { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceDue { get; set; } = 0m;

    public DateTime? DueDate { get; set; }

    [MaxLength(32)]
    public string? Status { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

[Table("AccountPayables")]
[Index(nameof(ReferenceId), Name = "IX_AccountPayable_ReferenceId")]
[Index(nameof(PartyId), Name = "IX_AccountPayable_PartyId")]
[Index(nameof(DueDate), Name = "IX_AccountPayable_DueDate")]
[Index(nameof(Status), Name = "IX_AccountPayable_Status")]
public class AccountPayable : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid APId { get; set; }

    [Required]
    public Guid ReferenceId { get; set; }

    public Guid? PartyId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountDue { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceDue { get; set; } = 0m;

    public DateTime? DueDate { get; set; }

    [MaxLength(32)]
    public string? Status { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

[Table("Expenses")]
[Index(nameof(BranchId), Name = "IX_Expense_BranchId")]
[Index(nameof(Category), Name = "IX_Expense_Category")]
[Index(nameof(PaidAt), Name = "IX_Expense_PaidAt")]
[Index(nameof(CreatedByStaffId), Name = "IX_Expense_CreatedByStaffId")]
public class Expense : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExpenseId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [Required]
    [MaxLength(64)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0m;

    [MaxLength(128)]
    public string? PaidTo { get; set; }

    [Required]
    public DateTime PaidAt { get; set; }

    [MaxLength(256)]
    public string? ReceiptImageUrl { get; set; }

    public string? Notes { get; set; }

    [ForeignKey("CreatedByStaff")]
    public Guid? CreatedByStaffId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
    public virtual Staff? CreatedByStaff { get; set; }
}
