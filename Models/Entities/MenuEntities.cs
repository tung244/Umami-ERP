using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuanLiKhoHang.Models.Enums;

namespace QuanLiKhoHang.Models.Entities;

[Table("Dishes")]
[Index(nameof(CategoryId), Name = "IX_Dish_CategoryId")]
[Index(nameof(KitchenSectionId), Name = "IX_Dish_KitchenSectionId")]
public class Dish : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DishId { get; set; }

    [MaxLength(64)]
    public string? Sku { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [ForeignKey("Category")]
    public Guid? CategoryId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DefaultServingPrice { get; set; } = 0m;

    [Required]
    public bool Active { get; set; } = true;

    [Required]
    public bool IsAvailableOnline { get; set; } = true;

    public int? PreparationTimeMinutes { get; set; }

    [ForeignKey("KitchenSection")]
    public Guid? KitchenSectionId { get; set; }

    [Required]
    public bool IsComposite { get; set; } = false;

    [MaxLength(64)]
    public string? PortionSize { get; set; }

    [MaxLength(256)]
    public string? ImageUrl { get; set; }

    [Required]
    public bool Taxable { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Category? Category { get; set; }
    public virtual KitchenSection? KitchenSection { get; set; }
    public virtual ICollection<MenuPriceHistory> PriceHistory { get; set; } = new List<MenuPriceHistory>();
    public virtual ICollection<DishIngredient> Ingredients { get; set; } = new List<DishIngredient>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

[Table("Categories")]
[Index(nameof(ParentCategoryId), Name = "IX_Category_ParentCategoryId")]
[Index(nameof(DisplayOrder), Name = "IX_Category_DisplayOrder")]
public class Category : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CategoryId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [ForeignKey("ParentCategory")]
    public Guid? ParentCategoryId { get; set; }

    public int? DisplayOrder { get; set; }

    [Required]
    public bool Active { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}

[Table("DishIngredients")]
[Index(nameof(DishId), Name = "IX_DishIngredient_DishId")]
[Index(nameof(InventoryItemId), Name = "IX_DishIngredient_InventoryItemId")]
public class DishIngredient : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DishIngredientId { get; set; }

    [Required]
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }

    [Required]
    [ForeignKey("InventoryItem")]
    public Guid InventoryItemId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal QuantityPerPortion { get; set; } = 0m;

    [Column(TypeName = "decimal(18,4)")]
    public decimal? UnitMultiplier { get; set; }

    [Required]
    public bool IsOptional { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Dish? Dish { get; set; }
    public virtual InventoryItem? InventoryItem { get; set; }
}

[Table("MenuPriceHistory")]
[Index(nameof(DishId), Name = "IX_MenuPriceHistory_DishId")]
[Index(nameof(EffectiveFrom), nameof(EffectiveTo), Name = "IX_MenuPriceHistory_EffectivePeriod")]
public class MenuPriceHistory : IAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MenuPriceId { get; set; }

    [Required]
    [ForeignKey("Dish")]
    public Guid DishId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0m;

    [Required]
    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    [ForeignKey("ChangedByStaff")]
    public Guid? ChangedByStaffId { get; set; }

    [MaxLength(256)]
    public string? Reason { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Dish? Dish { get; set; }
    public virtual Staff? ChangedByStaff { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
