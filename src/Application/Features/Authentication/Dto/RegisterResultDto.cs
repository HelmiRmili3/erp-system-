using Backend.Domain.Enums;

namespace Backend.Application.Features.Authentication.Dto;

public class RegisterResultDto
{
    public string Id { get; set; } = string.Empty;

    // Identity Info
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    // Personal Info
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }

    // Professional Info
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime? HireDate { get; set; }
    public ContractType? ContractType { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    // Supervisor
    public string? SupervisorId { get; set; }
}
