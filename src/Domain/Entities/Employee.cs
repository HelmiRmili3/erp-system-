using System;
namespace Backend.Domain.Entities;

public class Employee : BaseAuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string ContractType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public ICollection<Attendance>? Attendances { get; set; } = new List<Attendance>();
    //public ICollection<Absence>? Absences { get; set; } = new List<Absence>();
    public ICollection<Payroll>? Payrolls { get; set; } = new List<Payroll>();
    public ICollection<Expense>? Expenses { get; set; } = new List<Expense>();
}
