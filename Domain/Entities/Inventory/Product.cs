using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory;

public class Product : BaseInventoryEntity
{
    [Key] public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }

    public long CategoryId { get; set; }
    public Category? Category { get; set; }

    public long UnitOfMeasureId { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }

    public ProductUsage ProductUsage { get; set; } = ProductUsage.NotUsable;
    
    public ICollection<Inventory>? Inventories { get; set; }
}