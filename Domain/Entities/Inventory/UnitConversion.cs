using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Inventory;

public class UnitConversion : BaseInventoryEntity
{
    [Key] public long ConversionId { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    public long FromUnitId { get; set; }
    public UnitOfMeasure? FromUnit { get; set; }
    public long ToUnitId { get; set; }
    public UnitOfMeasure? ToUnit { get; set; }
    [Column(TypeName = "decimal(19,3)")]
    public decimal Factor { get; set; }
}