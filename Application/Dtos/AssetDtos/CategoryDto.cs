namespace Application.Dtos.AssetDtos;

public class CategoryDto
{
    public long Id { get; set; } 
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool HasSubCategory { get; set; }
    public long? ParentId { get; set; }
}