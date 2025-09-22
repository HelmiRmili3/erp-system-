
public class Absence : BaseAuditableEntity
{
    public required string UserId { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AbsenceType AbsenceType { get; set; }
    public AbsenceStatus StatusType { get; set; }
    public string? Reason { get; set; }
}
