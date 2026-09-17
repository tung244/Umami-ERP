using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

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
    public decimal SubTotal { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ServiceChargeAmount { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RoundingAdjustment { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PaidAmount { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceDue { get; set; } = 0m;

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
    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
    public virtual ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();
    public virtual ICollection<DeliveryOrder> DeliveryOrders { get; set; } = new List<DeliveryOrder>();
}

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
    public decimal Quantity { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; } = 0m;

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
    public decimal DiscountAmount { get; set; } = 0m;

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
    public virtual ICollection<KitchenTicket> KitchenTickets { get; set; } = new List<KitchenTicket>();
}

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
    public decimal Amount { get; set; } = 0m;

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
    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}

[Table("Refunds")]
public class Refund : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RefundId { get; set; }

    [Required]
    [ForeignKey("OriginalPayment")]
    public Guid OriginalPaymentId { get; set; }

    [Required]
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0m;

    [Required]
    public RefundMethod RefundMethod { get; set; }

    [Required]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [ForeignKey("ProcessedByStaff")]
    public Guid ProcessedByStaffId { get; set; }

    [Required]
    public DateTime RefundDate { get; set; }

    [Required]
    public RefundStatus Status { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Payment? OriginalPayment { get; set; }
    public virtual Order? Order { get; set; }
    public virtual Staff? ProcessedByStaff { get; set; }
}

[Table("KitchenTickets")]
[Index(nameof(OrderId), Name = "IX_KitchenTicket_OrderId")]
[Index(nameof(KitchenSectionId), Name = "IX_KitchenTicket_KitchenSectionId")]
public class KitchenTicket : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid KitchenTicketId { get; set; }

    [Required]
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }

    [ForeignKey("OrderItem")]
    public Guid? OrderItemId { get; set; }

    [Required]
    [MaxLength(64)]
    public string TicketNumber { get; set; } = string.Empty;

    [Required]
    [ForeignKey("KitchenSection")]
    public Guid KitchenSectionId { get; set; }

    [Required]
    public DateTime SentAt { get; set; }

    public DateTime? ReceivedAt { get; set; }

    [Required]
    public KitchenTicketStatus Status { get; set; }

    [ForeignKey("PrintedByStaff")]
    public Guid? PrintedByStaffId { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Order? Order { get; set; }
    public virtual OrderItem? OrderItem { get; set; }
    public virtual KitchenSection? KitchenSection { get; set; }
    public virtual Staff? PrintedByStaff { get; set; }
}

[Table("DeliveryOrders")]
[Index(nameof(OrderId), Name = "IX_DeliveryOrder_OrderId")]
[Index(nameof(DriverId), Name = "IX_DeliveryOrder_DriverId")]
public class DeliveryOrder : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DeliveryOrderId { get; set; }

    [Required]
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }

    [Required]
    public string DeliveryAddress { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string RecipientName { get; set; } = string.Empty;

    [Required]
    [MaxLength(32)]
    public string RecipientPhone { get; set; } = string.Empty;

    [Required]
    public DeliveryMethod DeliveryMethod { get; set; }

    [ForeignKey("Driver")]
    public Guid? DriverId { get; set; }

    public DateTime? EstimatedDeliveryTime { get; set; }
    public DateTime? ActualDeliveryTime { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DeliveryFee { get; set; } = 0m;

    [Required]
    public DeliveryStatus DeliveryStatus { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Order? Order { get; set; }
    public virtual Driver? Driver { get; set; }
}
