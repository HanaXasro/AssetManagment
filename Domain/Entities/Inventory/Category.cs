using System.ComponentModel.DataAnnotations;
using Domain.Entities.General;

namespace Domain.Entities.Inventory;

public class Category : BaseEntity
{
    [Key] public long Id { get; set; }
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public bool HasSubCategory { get; set; }

    public long ParentId { get; set; }

    public Category? Parent { get; set; }

    [MaxLength(255)] public string? Description { get; set; }
    
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }

    public ICollection<Category>? Parents { get; set; }
    public ICollection<Item>? Items { get; set; }
}