using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

[Table("Branches")]
public class Branch : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BranchId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(16)]
    public string Code { get; set; } = string.Empty;

    public string? Address { get; set; }

    [MaxLength(32)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(64)]
    public string TimeZone { get; set; } = string.Empty;

    [Required]
    [MaxLength(8)]
    public string DefaultCurrency { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public virtual ICollection<RestaurantTable> Tables { get; set; } = new List<RestaurantTable>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    public virtual ICollection<StockCount> StockCounts { get; set; } = new List<StockCount>();
}

[Table("RestaurantTables")]
[Index(nameof(BranchId), nameof(Code), IsUnique = true, Name = "IX_RestaurantTable_Branch_Code")]
public class RestaurantTable : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TableId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [Required]
    [MaxLength(16)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(64)]
    public string? Area { get; set; }

    [Required]
    public int Seats { get; set; }

    [Required]
    public TableStatus Status { get; set; }

    [Required]
    public bool IsOutdoor { get; set; } = false;

    public int? MapX { get; set; }
    public int? MapY { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

[Table("KitchenSections")]
public class KitchenSection : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid KitchenSectionId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [ForeignKey("PrinterDevice")]
    public Guid? PrinterDeviceId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Device? PrinterDevice { get; set; }
    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<KitchenTicket> Tickets { get; set; } = new List<KitchenTicket>();
}

[Table("Reservations")]
[Index(nameof(BranchId), nameof(ReserveStartAt), Name = "IX_Reservation_Branch_StartTime")]
public class Reservation : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ReservationId { get; set; }

    [Required]
    [ForeignKey("Branch")]
    public Guid BranchId { get; set; }

    [ForeignKey("Customer")]
    public Guid? CustomerId { get; set; }

    [ForeignKey("ReservedTable")]
    public Guid? ReservedTableId { get; set; }

    [Required]
    public int PartySize { get; set; }

    [Required]
    public DateTime ReserveStartAt { get; set; }

    [Required]
    public DateTime ReserveEndAt { get; set; }

    [Required]
    public ReservationStatus Status { get; set; }

    [Required]
    public ReservationSource Source { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Branch? Branch { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual RestaurantTable? ReservedTable { get; set; }
}
