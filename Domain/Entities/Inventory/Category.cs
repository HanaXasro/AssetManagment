using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory;

public class Category : BaseInventoryEntity
{ 
    [Key]
    public long CategoryId { get; set; } 
    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty; 
    [MaxLength(255)]
    public string? Description { get; set; } 
}