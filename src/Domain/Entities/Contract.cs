
public class Contract : BaseAuditableEntity
{
    public required string UserId { get; set; }
    public ContractType ContractType { get; set; } = ContractType.Permanent; // CDI, CDD, etc.
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } 
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public string? FileUrl { get; set; }

}
