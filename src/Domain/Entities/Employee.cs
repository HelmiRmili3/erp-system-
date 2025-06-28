using Backend.Domain.Entities;

public class Employee : BaseAuditableEntity
{

    // Personal info
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }

    // Professional info
    public string JobTitle { get; set; } = null!;
    public string Department { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public string? ContractType { get; set; }
}
