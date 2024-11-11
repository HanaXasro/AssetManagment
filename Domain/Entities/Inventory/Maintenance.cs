using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Inventory;

public class Maintenance : BaseEntity
{
    public long MaintenanceId { get; set; }
    public long PurchaseId { get; set; }
    public Purchase? Purchase { get; set; }
    public MaintenanceStatus MaintenanceStatus { get; set; } = MaintenanceStatus.Pending;
    public MaintenanceType MaintenanceType { get; set; } = MaintenanceType.InsideCompany;
    [Column(TypeName = "decimal(19,3)")] public decimal? Cost { get; set; }
    public string? Description { get; set; }
}