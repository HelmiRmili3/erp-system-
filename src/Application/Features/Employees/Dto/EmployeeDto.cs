namespace Backend.Application.Features.Employees.Dto
{
    public class EmployeeDto : BaseEmployeeDto
    {
        // Self-referencing supervisor
        public BaseEmployeeDto? Supervisor { get; set; }
    }

    public class BaseEmployeeDto
    {
        public int Id { get; init; }

        // Add UserId to link with AspNetUsers table
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; init; }
        public string? ContractType { get; set; }
        public string Status { get; set; } = string.Empty;

      
    }

    public record EmployeeAddDto
    {
    
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public DateTime BirthDate { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Address { get; init; }
        public string JobTitle { get; init; } = string.Empty;
        public string Department { get; init; } = string.Empty;
        public DateTime HireDate { get; init; }
        public string? ContractType { get; init; }
        public string Status { get; init; } = string.Empty;

        public int? SupervisorId { get; init; }
    }

    public record EmployeeUpdateDto : EmployeeAddDto
    {
        public int Id { get; init; }
    }
}
