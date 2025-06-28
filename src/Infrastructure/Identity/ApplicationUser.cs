using Backend.Domain.Entities;
using Backend.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Personal Information
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }

    // Professional Information
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime? HireDate { get; set; }
    public ContractType? ContractType { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active; // Active, Inactive, Terminated, etc.

    // Audit Properties (from BaseAuditableEntity)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; } 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }

    // Self-referencing supervisor relationship
    public string? SupervisorId { get; set; }

    [ForeignKey("SupervisorId")]
    public virtual ApplicationUser? Supervisor { get; set; }

    public virtual ICollection<ApplicationUser> Subordinates { get; set; } = new List<ApplicationUser>();

    // Navigation properties for related entities
    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    // Computed properties
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}".Trim();

    [NotMapped]
    public int? YearsOfService => HireDate.HasValue
        ? (int?)((DateTime.UtcNow - HireDate.Value).TotalDays / 365.25)
        : null;
}

