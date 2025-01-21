using Domain.Entities;
using Domain.Entities.Inventory;

namespace Domain.Repositories.AssetRepositories;

public interface ICategory
{
    public Task<Category> AddAsync(Category category);
    public Task<Category?> IsUnique(string? name);
    public Task<Category> UpdateAsync(Category category);
    public Task<Category?> GetByIdAsync(long? id);
    public Task<Category> DeleteAsync(Category category);
    public Task<Pagination<Category>> FindAsync(string? search , string? orderBy ,bool isAscending, int page, int pageSize);
    public Task<IEnumerable<Category>> FindAsync(string? search);
    public Task<IEnumerable<Category>> FindSubCategoryAsync(string? search);
}