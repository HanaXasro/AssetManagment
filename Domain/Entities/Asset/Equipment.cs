using Domain.Entities.General;
using Domain.Entities.Inventory;

namespace Domain.Entities.Asset;

public class Equipment : BaseEntity
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string? Description { get; set; }
    public string Manufacturer  { get; set; }
    public string Model { get; set; }
    public string? SerialNumber { get; set; }
    public string Location { get; set; }
    public EquipmentStatus Status { get; set; }
    public long ItemId { get; set; }
    public Item? Item { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public bool SharedAcrossDepartment { get; set; }
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}