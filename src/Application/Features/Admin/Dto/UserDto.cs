using Backend.Domain.Enums; // For ContractType and EmployeeStatus
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Application.Features.Admin.Dto;

public class UserDto
{
    public string Id { get; set; } = string.Empty;

    // Personal Information
    public required string FirstName { get; set; }
    public required string LastName { get; set; } 
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }

    // Professional Information
    public required string JobTitle { get; set; } 
    public required string Department { get; set; } 
    public DateTime? HireDate { get; set; }
    public required string ContractType { get; set; }
    public required string Status { get; set; }

    // Audit Properties
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public required string UpdatedBy { get; set; }

    // Supervisor
    public required string SupervisorId { get; set; }
    public required string SupervisorFullName { get; set; } // Optional: include supervisor name

    // Role and Permission Information
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}
