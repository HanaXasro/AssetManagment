using Domain.Entities.UserEntity;
using Infrastructure.DataContext;

namespace Infrastructure.AutoSeeds;

public static class PermissionSeed
{
    public static async Task Seed(DataDbContext context)
    {

        var blockList = new List<string>()
        {
            nameof(RolePermission),
            nameof(RefreshToken)
        };
        
        var entityNames = context.Model.GetEntityTypes()
            .Where(e => !blockList.Contains(e.ClrType.Name))
            .Select(e => e.ClrType.Name)
            .ToList();
        
        var seedData = entityNames
            .Select((name, index) => new Permission
            {
                Title = name    
            })
            .ToList();

        var existingTitles = context.Permissions.Select(p => p.Title).ToHashSet();

        var newPermissions = seedData
            .Where(data => !existingTitles.Contains(data.Title))
            .Select(data => new Permission() { Id = data.Id, Title = data.Title });

        var permissions = newPermissions as Permission[] ?? newPermissions.ToArray();
        if (permissions.Any())
        {
            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }
    }
}