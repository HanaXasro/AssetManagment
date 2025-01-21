using Application.Dtos;
using Application.Dtos.AssetDtos;
using Application.Helper;
using AutoMapper;
using Domain.Entities.Inventory;
using Domain.Repositories.AssetRepositories;
using MediatR;

namespace Application.Queries.AssetManagement.CategoryQueries.GetCategories;

public class GetCategoriesQh(IMapper mapper,ICategory categoryRepo) : IRequestHandler<GetCategoriesQ ,PaginationDto<CategoryDto>>
{
    public async Task<PaginationDto<CategoryDto>> Handle(GetCategoriesQ request, CancellationToken cancellationToken)
    {
        var isProperty = FindPropertyHelper.FindProperty<Category>(request.OrderBy);
        var categories = await categoryRepo.FindAsync(request.Search, isProperty ? request.OrderBy : "Id", request.IsAscending ?? true,
            request.Page ?? 1, request.PageSize ?? 10);
        return mapper.Map<PaginationDto<CategoryDto>>(categories);
    }
}