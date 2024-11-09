using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory;

public class Inventory : BaseInventoryEntity
{
    [Key]
    public long InventoryId { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public long Quantity { get; set; }
    public DateTime LastEntry { get; set; }
    public string? Notes { get; set; } 
    
}