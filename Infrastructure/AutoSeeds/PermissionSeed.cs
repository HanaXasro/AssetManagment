using Domain.Entities.UserEntity;
using Infrastructure.DataContext;

namespace Infrastructure.AutoSeeds;

public static class PermissionSeed
{
    public static void Seed(DataDbContext context)
    {

        var blockList = new List<string>()
        {
            nameof(RolePermission),
        };
        
        var entityNames = context.Model.GetEntityTypes()
            .Where(e => !blockList.Contains(e.ClrType.Name))
            .Select(e => e.ClrType.Name)
            .ToList();
        
        var seedData = entityNames
            .Select((name, index) => new Permission
            {
                Id = index + 1,
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
            context.Permissions.AddRange(permissions);
            context.SaveChanges();
        }
    }
}