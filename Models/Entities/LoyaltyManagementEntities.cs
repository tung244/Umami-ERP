using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

/// <summary>
/// Cấu hình thiết lập chương trình tích điểm
/// </summary>
[Table("LoyaltySettings")]
public class LoyaltySettings : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid LoyaltySettingsId { get; set; }

    [Required]
    [MaxLength(255)]
    public string SettingKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string SettingName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [MaxLength(500)]
    public string SettingValue { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DataType { get; set; } = "string"; // string, number, decimal, boolean

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Cấu hình hạng thành viên
/// </summary>
[Table("TierConfigs")]
public class TierConfig : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TierConfigId { get; set; }

    [Required]
    public LoyaltyTier Tier { get; set; }

    [Required]
    [MaxLength(100)]
    public string TierName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MinPoints { get; set; } = 0m;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxPoints { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; } // Màu hiển thị UI

    [MaxLength(50)]
    public string? Icon { get; set; } // Icon hiển thị UI

    public int DisplayOrder { get; set; } = 0;

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual ICollection<TierBenefit> TierBenefits { get; set; } = new List<TierBenefit>();
}

/// <summary>
/// Danh sách quyền lợi có thể áp dụng
/// </summary>
[Table("Benefits")]
public class Benefit : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BenefitId { get; set; }

    [Required]
    [MaxLength(255)]
    public string BenefitName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string BenefitType { get; set; } = string.Empty; // Discount, FreeBirthday Cake, FreeParking, PriorityBooking, etc.

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountPercent { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    public string? AdditionalData { get; set; } // JSON data for flexible benefits

    [MaxLength(50)]
    public string? Icon { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual ICollection<TierBenefit> TierBenefits { get; set; } = new List<TierBenefit>();
}

/// <summary>
/// Liên kết giữa hạng thành viên và quyền lợi
/// </summary>
[Table("TierBenefits")]
[Index(nameof(TierConfigId), nameof(BenefitId), IsUnique = true, Name = "IX_TierBenefit_Unique")]
public class TierBenefit : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TierBenefitId { get; set; }

    [Required]
    [ForeignKey("TierConfig")]
    public Guid TierConfigId { get; set; }

    [Required]
    [ForeignKey("Benefit")]
    public Guid BenefitId { get; set; }

    public string? CustomValue { get; set; } // Override giá trị benefit cụ thể cho tier này

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual TierConfig? TierConfig { get; set; }
    public virtual Benefit? Benefit { get; set; }
}

/// <summary>
/// Quy tắc đổi điểm
/// </summary>
[Table("RedemptionRules")]
public class RedemptionRule : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RedemptionRuleId { get; set; }

    [Required]
    [MaxLength(255)]
    public string RuleName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PointsRequired { get; set; } = 0m;

    [Required]
    [MaxLength(50)]
    public string RewardType { get; set; } = string.Empty; // Voucher, Discount, Gift, etc.

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountPercent { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinOrderAmount { get; set; }

    public int? ValidityDays { get; set; } // Số ngày hiệu lực của voucher/reward

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
