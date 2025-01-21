namespace Domain.Entities.General;

public class Branch : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
}