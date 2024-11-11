using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Inventory;

public class Inventory : BaseInventoryEntity
{
    [Key] public long InventoryId { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    [Column(TypeName = "decimal(19,10)")]
    public decimal Quantity { get; set; }
    public long PurchaseId { get; set; }
    public Purchase? Purchase { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Active;
    public DateTime? NextTestingDate { get; set; }
}