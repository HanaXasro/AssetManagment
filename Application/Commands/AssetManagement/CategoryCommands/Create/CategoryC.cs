using Application.Dtos.AssetDtos;
using MediatR;

namespace Application.Commands.AssetManagement.CategoryCommands.Create;

public record CategoryC : IRequest<CategoryDto>
{
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public bool? HasSubCategory { get; set; }
    public long? ParentId { get; set; }
}