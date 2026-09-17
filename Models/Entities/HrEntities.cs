using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;
using System.Linq;

namespace QuanLiKhoHang.Models.Entities;

[Table("Staff")]
[Index(nameof(EmployeeNumber), IsUnique = true, Name = "IX_Staff_EmployeeNumber")]
[Index(nameof(Email), Name = "IX_Staff_Email")]
[Index(nameof(RoleId), Name = "IX_Staff_RoleId")]
[Index(nameof(BranchId), Name = "IX_Staff_BranchId")]
[Index(nameof(IsActive), Name = "IX_Staff_IsActive")]
public class Staff : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StaffId { get; set; }

    [Required]
    [MaxLength(32)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(128)]
    public string? Email { get; set; }

    [MaxLength(32)]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng 0")]
    public string? PhoneNumber { get; set; }

    [Required]
    [MaxLength(256)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [ForeignKey("Role")]
    public Guid RoleId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    public DateTime? HireDate { get; set; }

    [Required]
    public EmploymentType EmploymentType { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? BaseSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? HourlyRate { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public string? Address { get; set; }

    [MaxLength(32)]
    public string? TaxId { get; set; }

    [MaxLength(256)]
    public string? AvatarUrl { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));

    // Navigation Properties
    public virtual Role? Role { get; set; }
    public virtual Branch? Branch { get; set; }
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public virtual ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();
    public virtual ICollection<Order> CreatedOrders { get; set; } = new List<Order>();
    public virtual ICollection<Order> ClosedOrders { get; set; } = new List<Order>();
    public virtual ICollection<Payment> ProcessedPayments { get; set; } = new List<Payment>();
    public virtual ICollection<Refund> ProcessedRefunds { get; set; } = new List<Refund>();
    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public virtual ICollection<StaffPermission> Permissions { get; set; } = new List<StaffPermission>();
}

[Table("Roles")]
[Index(nameof(Name), IsUnique = true, Name = "IX_Role_Name")]
public class Role : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RoleId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? DefaultPermissions { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    public virtual ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
}

[Table("Permissions")]
[Index(nameof(Name), IsUnique = true, Name = "IX_Permission_Name")]
public class Permission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PermissionId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Navigation Properties
    public virtual ICollection<RolePermission> Roles { get; set; } = new List<RolePermission>();
    public virtual ICollection<StaffPermission> StaffMembers { get; set; } = new List<StaffPermission>();
}

[Table("RolePermissions")]
[Index(nameof(RoleId), Name = "IX_RolePermission_RoleId")]
[Index(nameof(PermissionId), Name = "IX_RolePermission_PermissionId")]
public class RolePermission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RolePermissionId { get; set; }

    [Required]
    [ForeignKey("Role")]
    public Guid RoleId { get; set; }

    [Required]
    [ForeignKey("Permission")]
    public Guid PermissionId { get; set; }

    // Navigation Properties
    public virtual Role? Role { get; set; }
    public virtual Permission? Permission { get; set; }
}

[Table("StaffPermissions")]
[Index(nameof(StaffId), Name = "IX_StaffPermission_StaffId")]
[Index(nameof(PermissionId), Name = "IX_StaffPermission_PermissionId")]
public class StaffPermission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StaffPermissionId { get; set; }

    [Required]
    [ForeignKey("Staff")]
    public Guid StaffId { get; set; }

    [Required]
    [ForeignKey("Permission")]
    public Guid PermissionId { get; set; }

    // Navigation Properties
    public virtual Staff? Staff { get; set; }
    public virtual Permission? Permission { get; set; }
}

[Table("Shifts")]
[Index(nameof(BranchId), Name = "IX_Shift_BranchId")]
[Index(nameof(Name), Name = "IX_Shift_Name")]
public class Shift : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ShiftId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}

[Table("Attendances")]
[Index(nameof(StaffId), Name = "IX_Attendance_StaffId")]
[Index(nameof(ClockInAt), Name = "IX_Attendance_ClockInAt")]
[Index(nameof(ShiftId), Name = "IX_Attendance_ShiftId")]
[Index(nameof(ApprovedByStaffId), Name = "IX_Attendance_ApprovedByStaffId")]
[Index(nameof(Status), Name = "IX_Attendance_Status")]
public class Attendance : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AttendanceId { get; set; }

    [Required]
    [ForeignKey("Staff")]
    public Guid StaffId { get; set; }

    [Required]
    public DateTime ClockInAt { get; set; }

    public DateTime? ClockOutAt { get; set; }

    [Required]
    public ClockType ClockType { get; set; }

    [MaxLength(128)]
    public string? Location { get; set; }

    [ForeignKey("Shift")]
    public Guid? ShiftId { get; set; }

    [ForeignKey("ApprovedByStaff")]
    public Guid? ApprovedByStaffId { get; set; }

    [Required]
    public AttendanceStatus Status { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Staff? Staff { get; set; }
    public virtual Shift? Shift { get; set; }
    public virtual Staff? ApprovedByStaff { get; set; }
}

[Table("PayrollRecords")]
[Index(nameof(StaffId), Name = "IX_PayrollRecord_StaffId")]
[Index(nameof(PayrollPeriodStart), Name = "IX_PayrollRecord_PayrollPeriodStart")]
[Index(nameof(PayrollPeriodEnd), Name = "IX_PayrollRecord_PayrollPeriodEnd")]
[Index(nameof(PaidAt), Name = "IX_PayrollRecord_PaidAt")]
public class PayrollRecord : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PayrollId { get; set; }

    [Required]
    [ForeignKey("Staff")]
    public Guid StaffId { get; set; }

    [Required]
    public DateOnly PayrollPeriodStart { get; set; }

    [Required]
    public DateOnly PayrollPeriodEnd { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossPay { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal NetPay { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Deductions { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Taxes { get; set; } = 0m;

    public DateTime? PaidAt { get; set; }

    [Required]
    public PayrollPaymentMethod PaymentMethod { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Staff? Staff { get; set; }
}

[Table("StaffShifts")]
[Index(nameof(StaffId), Name = "IX_StaffShift_StaffId")]
[Index(nameof(ShiftId), Name = "IX_StaffShift_ShiftId")]
[Index(nameof(WorkDate), Name = "IX_StaffShift_WorkDate")]
[Index(nameof(Status), Name = "IX_StaffShift_Status")]
public class StaffShift : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StaffShiftId { get; set; }

    [Required]
    [ForeignKey("Staff")]
    public Guid StaffId { get; set; }

    [Required]
    [ForeignKey("Shift")]
    public Guid ShiftId { get; set; }

    [Required]
    public DateTime WorkDate { get; set; }

    [Required]
    public ShiftStatus Status { get; set; } = ShiftStatus.Scheduled;

    public DateTime? ApprovedAt { get; set; }

    [ForeignKey("ApprovedByStaff")]
    public Guid? ApprovedByStaffId { get; set; }

    public string? RejectionReason { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Staff? Staff { get; set; }
    public virtual Shift? Shift { get; set; }
    public virtual Staff? ApprovedByStaff { get; set; }
}
