using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuanLiKhoHang.Models.Entities;

[Table("Drivers")]
[Index(nameof(Name), Name = "IX_Driver_Name")]
[Index(nameof(PhoneNumber), Name = "IX_Driver_PhoneNumber")]
[Index(nameof(IsActive), Name = "IX_Driver_IsActive")]
public class Driver : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DriverId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(32)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(128)]
    public string? VehicleInfo { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public int AssignedOrdersCount { get; set; } = 0;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual ICollection<DeliveryOrder> DeliveryOrders { get; set; } = new List<DeliveryOrder>();
}
