using Application.Dtos.AssetDtos;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.Inventory;
using Domain.Repositories.AssetRepositories;
using MediatR;

namespace Application.Commands.AssetManagement.CategoryCommands.Create;

public class CategoryCh(IMapper mapper , ICategory categoryRepo) : IRequestHandler<CategoryC , CategoryDto>
{
    public async Task<CategoryDto> Handle(CategoryC request, CancellationToken cancellationToken)
    {
        
        var isUnique = await categoryRepo.IsUnique(request.Name);
        if (isUnique != null)
            throw new UnprocessableException("Name already exists.", isUnique.Name);
        
        var validator = new CategoryCValidator(categoryRepo);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new BadRequestException("The orphan form contains invalid data. Please review the fields.",validationResult);
        
        var category = await categoryRepo.AddAsync(mapper.Map<Category>(request));
        return mapper.Map<CategoryDto>(category);
    }
}