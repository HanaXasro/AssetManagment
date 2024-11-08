using Domain.Entities.UserEntity;

namespace Domain.Entities.Inventory;

public class BaseInventoryEntity : BaseEntity
{
    public List<Guid> UserId { get; set; } = new();
}