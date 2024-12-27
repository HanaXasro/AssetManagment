using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory;

public class Category : BaseInventoryEntity
{ 
    [Key]
    public long Id { get; set; } 
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty; 
    [MaxLength(255)]
    public string? Description { get; set; }

    public bool HasSubCategory { get; set; }
    public long? ParentId { get; set; }
    public Category? Parent { get; set; }
}