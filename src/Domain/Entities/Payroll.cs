namespace Backend.Domain.Entities;

public class Payroll :BaseAuditableEntity
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;  // Navigation property

}
