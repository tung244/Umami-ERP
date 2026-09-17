using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

[Table("Customers")]
public class Customer : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CustomerId { get; set; }

    [MaxLength(64)]
    public string? FirstName { get; set; }

    [MaxLength(64)]
    public string? LastName { get; set; }

    [MaxLength(128)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(32)]
    [Phone]
    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [ForeignKey("PreferredBranch")]
    public Guid? PreferredBranchId { get; set; }

    public string? Notes { get; set; }

    [NotMapped]
    public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));

    // Navigation Properties
    public virtual Branch? PreferredBranch { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<LoyaltyAccount> LoyaltyAccounts { get; set; } = new List<LoyaltyAccount>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

[Table("LoyaltyAccounts")]
[Index(nameof(CustomerId), Name = "IX_LoyaltyAccount_CustomerId")]
public class LoyaltyAccount : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid LoyaltyAccountId { get; set; }

    [Required]
    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PointsBalance { get; set; } = 0m;

    [Required]
    public LoyaltyTier Tier { get; set; }

    [Required]
    public DateTime TierEffectiveFrom { get; set; }

    public DateTime? TierEffectiveTo { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalEarnedPoints { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalRedeemedPoints { get; set; } = 0m;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<LoyaltyTransaction> Transactions { get; set; } = new List<LoyaltyTransaction>();
}

[Table("LoyaltyTransactions")]
[Index(nameof(LoyaltyAccountId), Name = "IX_LoyaltyTransaction_LoyaltyAccountId")]
[Index(nameof(OrderId), Name = "IX_LoyaltyTransaction_OrderId")]
public class LoyaltyTransaction : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid LoyaltyTransactionId { get; set; }

    [Required]
    [ForeignKey("LoyaltyAccount")]
    public Guid LoyaltyAccountId { get; set; }

    [ForeignKey("Order")]
    public Guid? OrderId { get; set; }

    [Required]
    public LoyaltyTransactionType Type { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Points { get; set; } = 0m;

    public string? Reason { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual LoyaltyAccount? LoyaltyAccount { get; set; }
    public virtual Order? Order { get; set; }
}

[Table("Vouchers")]
[Index(nameof(Code), IsUnique = true, Name = "IX_Voucher_Code")]
[Index(nameof(ValidFrom), nameof(ValidTo), Name = "IX_Voucher_ValidityPeriod")]
public class Voucher : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid VoucherId { get; set; }

    [Required]
    [MaxLength(64)]
    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public VoucherDiscountType DiscountType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; } = 0m;

    [Required]
    public DateTime ValidFrom { get; set; }

    [Required]
    public DateTime ValidTo { get; set; }

    public int? UsageLimitPerCustomer { get; set; }
    public int? TotalUsageLimit { get; set; }

    [MaxLength(128)]
    public string? AppliedTo { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinimumOrderAmount { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
