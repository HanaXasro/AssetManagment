using Domain.Entities;
using Domain.Entities.Inventory;
using Domain.Repositories.AssetRepositories;
using Gridify;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AssetRepositories;

public class CategoryRepository(DataDbContext context) : ICategory
{
    public async Task<Category> AddAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> IsUnique(string? name)
    {
        return await context.Categories.ApplyFiltering($"Name={name}").SingleOrDefaultAsync();
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        var find = await context.Categories.FindAsync(category.Id);
        find!.Name = category.Name;
        find.Description = category.Description;
        find.HasSubCategory = category.HasSubCategory;
        find.ParentId = category.ParentId;
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> GetByIdAsync(long? id)
    {
        return await context.Categories.FindAsync(id); 
    }

    public async Task<Category> DeleteAsync(Category category)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Pagination<Category>> FindAsync(string? search, string? orderBy, bool isAscending, int page, int pageSize)
    {
        var categories = context.Categories.Include(e => e.Parent)
            .ApplyFiltering($"name =*{search ?? ""}|description =*{search ?? ""}")
            .ApplyOrdering($"{orderBy} {(isAscending ? "asc" : "desc")}")
            .AsQueryable();
        var pagedCategories = categories.ApplyPaging(page, pageSize);
        var result = await pagedCategories.ToListAsync();
        var totalCount = await categories.CountAsync();
        var pagination = new Pagination<Category>(result, page, pageSize , totalCount);
        return pagination;
    }
    
    public async Task<IEnumerable<Category>> FindAsync(string? search)
    {
        return await context.Categories
            .ApplyFiltering($"Name *={search ?? ""}").ToListAsync();
    }

    public async Task<IEnumerable<Category>> FindSubCategoryAsync(string? search)
    {
        return await context.Categories
            .ApplyFiltering($"Name =*{search ?? ""},HasSubCategory=true").ToListAsync();
    }
}