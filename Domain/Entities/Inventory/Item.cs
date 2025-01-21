using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Inventory;

public class Item : BaseEntity
{
    [Key] public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public long CategoryId { get; set; }
    public Category? Category { get; set; }

    public long UnitOfMeasureId { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }
    [Precision(10, 3)]
    public decimal Cost { get; set; }
    public DateTime PurchaseDate { get; set; }

}