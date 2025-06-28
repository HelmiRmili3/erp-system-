

using System.ComponentModel.DataAnnotations.Schema;

using Backend.Domain.Entities;
public class Contract : BaseAuditableEntity
{
    public required string UserId { get; set; }
    public ContractType ContractType { get; set; } = ContractType.Permanent; // CDI, CDD, etc.
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // null if ongoing
    public string? FileUrl { get; set; } // PDF contract path
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active; // or Terminated, Suspended
}
