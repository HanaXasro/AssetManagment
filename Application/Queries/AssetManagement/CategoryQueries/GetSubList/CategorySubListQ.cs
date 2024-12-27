using Application.Dtos.AssetDtos;
using MediatR;

namespace Application.Queries.AssetManagement.CategoryQueries.GetSubList;

public record CategorySubListQ(string? Search) : IRequest<List<CategoryListDto>>;