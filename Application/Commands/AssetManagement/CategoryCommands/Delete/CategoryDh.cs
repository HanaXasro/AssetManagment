using Application.Dtos.AssetDtos;
using Application.Exceptions;
using AutoMapper;
using Domain.Repositories.AssetRepositories;
using MediatR;

namespace Application.Commands.AssetManagement.CategoryCommands.Delete;

public class CategoryDh(IMapper mapper, ICategory categoryRepo) : IRequestHandler<CategoryD , CategoryDto>
{
    public async Task<CategoryDto> Handle(CategoryD request, CancellationToken cancellationToken)
    {
        var category = await categoryRepo.GetByIdAsync(request.Id);
        if(category == null)
            throw new NotFoundException("Category not found" , request.Id);
        var deleted = await categoryRepo.DeleteAsync(category);
        return mapper.Map<CategoryDto>(deleted);
    }
}