namespace Domain.Entities.UserEntity;

public class Permission
{
    public long Id { get; set; }
    public string Title { get; set; }
    public ICollection<RolePermission>? RolePermissions { get; set; }
}