using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Inventory;

public class Purchase : BaseInventoryEntity
{
    [Key] public long PurchaseId { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public long Quantity { get; set; }
    [Column(TypeName = "decimal(19,3)")] public decimal Cost { get; set; }
    [Column(TypeName = "decimal(19,3)")] public decimal TotalCost => Quantity * Cost;
    public PurchaseStatus Status { get; set; } = PurchaseStatus.UnderTest;
    public string? Description { get; set; }

}