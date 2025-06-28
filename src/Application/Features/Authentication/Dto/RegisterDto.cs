using Backend.Domain.Enums;

namespace Backend.Application.Features.Authentication.Dto
{
    public class RegisterDto
    {
        // Identity Fields
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Phone {  get; set; }

        // Professional Information
        public string JobTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }
        public ContractType? ContractType { get; set; }

        // Optional: you can allow setting Status or leave it default as Active
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        // Optional: Supervisor if registering from admin
        //public string? SupervisorId { get; set; }
    }
}
