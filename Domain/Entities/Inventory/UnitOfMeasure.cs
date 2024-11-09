using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory;

public class UnitOfMeasure  : BaseInventoryEntity
{
    [Key] public long UomId { get; set; }
    [MaxLength(255)]
    public string? UnitName { get; set; }
    public string Code { get; set; } = string.Empty;

}