using Application.Dtos.AssetDtos;
using AutoMapper;
using Domain.Repositories.AssetRepositories;
using MediatR;

namespace Application.Queries.AssetManagement.CategoryQueries.GetSubList;

public class CategorySubListQh(IMapper mapper,ICategory categoryRepo) : IRequestHandler<CategorySubListQ , List<CategoryListDto>>
{
    public async Task<List<CategoryListDto>> Handle(CategorySubListQ request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepo.FindSubCategoryAsync(request.Search);
        return mapper.Map<List<CategoryListDto>>(categories);
    }
}