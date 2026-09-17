using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

[Table("AuditLogs")]
[Index(nameof(EntityName), Name = "IX_AuditLog_EntityName")]
[Index(nameof(EntityId), Name = "IX_AuditLog_EntityId")]
[Index(nameof(Action), Name = "IX_AuditLog_Action")]
[Index(nameof(ChangedByStaffId), Name = "IX_AuditLog_ChangedByStaffId")]
[Index(nameof(ChangedAt), Name = "IX_AuditLog_ChangedAt")]
public class AuditLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AuditId { get; set; }

    [Required]
    [MaxLength(128)]
    public string EntityName { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string EntityId { get; set; } = string.Empty;

    [Required]
    public AuditAction Action { get; set; }

    [ForeignKey("ChangedByStaff")]
    public Guid? ChangedByStaffId { get; set; }

    [Required]
    public DateTime ChangedAt { get; set; }

    public string? ChangeSummary { get; set; }

    [MaxLength(64)]
    public string? IpAddress { get; set; }

    [MaxLength(256)]
    public string? UserAgent { get; set; }

    // Navigation Properties
    public virtual Staff? ChangedByStaff { get; set; }
}

[Table("Devices")]
[Index(nameof(BranchId), Name = "IX_Device_BranchId")]
[Index(nameof(Name), Name = "IX_Device_Name")]
[Index(nameof(Type), Name = "IX_Device_Type")]
[Index(nameof(Identifier), IsUnique = true, Name = "IX_Device_Identifier")]
[Index(nameof(IsActive), Name = "IX_Device_IsActive")]
[Index(nameof(LastSeenAt), Name = "IX_Device_LastSeenAt")]
public class Device : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DeviceId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DeviceType Type { get; set; }

    [Required]
    [MaxLength(128)]
    public string Identifier { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;

    public DateTime? LastSeenAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
}

[Table("ApplicationSettings")]
[Index(nameof(BranchScoped), Name = "IX_ApplicationSetting_BranchScoped")]
[Index(nameof(BranchId), Name = "IX_ApplicationSetting_BranchId")]
[Index(nameof(UpdatedAt), Name = "IX_ApplicationSetting_UpdatedAt")]
public class ApplicationSetting
{
    [Key]
    [MaxLength(128)]
    public string SettingKey { get; set; } = string.Empty;

    public string? SettingValue { get; set; }
    public string? Description { get; set; }

    [Required]
    public bool BranchScoped { get; set; }

    [ForeignKey("Branch")]
    public Guid? BranchId { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
}
