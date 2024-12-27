using Application.Dtos.AssetDtos;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.Inventory;
using Domain.Repositories.AssetRepositories;
using MediatR;

namespace Application.Commands.AssetManagement.CategoryCommands.Update;

public class CategoryUh(IMapper mapper,ICategory categoryRepo) : IRequestHandler<CategoryU , CategoryDto>
{
    public async Task<CategoryDto> Handle(CategoryU request, CancellationToken cancellationToken)
    {
        
        var category = await categoryRepo.GetByIdAsync(request.Id);
        if (category == null)
            throw new NotFoundException($"Category not found.", request.Id!);
        
        var validator = new CategoryUValidator(categoryRepo);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new BadRequestException("The orphan form contains invalid data. Please review the fields.",validationResult);
        
        if (category.Name != request.Name)
        {
            var isUnique = await categoryRepo.IsUnique(request.Name);
            if (isUnique != null)
                throw new UnprocessableException("Name already exists.", nameof(isUnique.Name));
        }
        
        var updateCategory = await categoryRepo.UpdateAsync(mapper.Map<Category>(request));
        return mapper.Map<CategoryDto>(updateCategory);
        
    }
}