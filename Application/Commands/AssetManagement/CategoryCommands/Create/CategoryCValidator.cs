using Domain.Repositories.AssetRepositories;
using FluentValidation;

namespace Application.Commands.AssetManagement.CategoryCommands.Create;

public class CategoryCValidator : AbstractValidator<CategoryC>
{
    private readonly ICategory _category;
    public CategoryCValidator(ICategory category) 
    {
        _category = category;
        RuleFor(o => o.Name)
            .NotEmpty().NotNull().WithMessage("Name is required.")
            .Length(2, 50).WithMessage("must be between 2 and 50 characters.");
        
        RuleFor(o => o.Description)
            .MaximumLength(255).WithMessage("Description must be no more than 255 characters.")
            .When(o => !string.IsNullOrEmpty(o.Description));
        
        RuleFor(o => o.ParentId)
            .GreaterThanOrEqualTo(1).WithMessage("ParentId must be at least 1 if provided.")
            .When(o => o.ParentId.HasValue)
            .MustAsync(async (id, cancellation) => await IsCategoryExists(id))
            .WithMessage("Category with the given ParentId does not exist.");
        
    }
    
    private async Task<bool> IsCategoryExists(long? subCategoryId)
    {
        if (!subCategoryId.HasValue) return true;
        var category = await _category.GetByIdAsync(subCategoryId.Value);
        return category != null;
    }
    
}