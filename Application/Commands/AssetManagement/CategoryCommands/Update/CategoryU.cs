using System.Text.Json.Serialization;
using Application.Dtos.AssetDtos;
using MediatR;

namespace Application.Commands.AssetManagement.CategoryCommands.Update;

public record CategoryU : IRequest<CategoryDto>
{
    [JsonIgnore]
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool HasSubCategory { get; set; }
    public long? ParentId { get; set; }
}