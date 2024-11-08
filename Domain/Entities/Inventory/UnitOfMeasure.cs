using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory;

public class UnitOfMeasure  : BaseInventoryEntity
{
    [Key] public long UomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
}