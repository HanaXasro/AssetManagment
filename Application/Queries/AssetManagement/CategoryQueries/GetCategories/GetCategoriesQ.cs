using Application.Dtos;
using Application.Dtos.AssetDtos;
using MediatR;

namespace Application.Queries.AssetManagement.CategoryQueries.GetCategories;

public record GetCategoriesQ : IRequest<PaginationDto<CategoryDto>>
{
    public string? Search { get; set; }

    public string? OrderBy { get; set; }

    public bool? IsAscending { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}