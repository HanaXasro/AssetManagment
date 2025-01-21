namespace Domain.Entities.General;

public class Employee : BaseEntity
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string MiddelName { get; set; }
    public string LastName { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
}