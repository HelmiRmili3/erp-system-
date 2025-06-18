using Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Employees.Dto;

    public class EmployeeDto : BaseEmployeeDto
    {
    public BaseEmployeeDto? ParentEmployee { get; set; }
    }


    public class BaseEmployeeDto
    {
        private static IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor(); // Default fallback

        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public int Id { get; init; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; init; }
        public string ContractType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Employee, EmployeeDto>();
            }
        }
    }

public record EmployeeAddDto
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string JobTitle { get; init; } = string.Empty;
        public string Department { get; init; } = string.Empty;
        public DateTime HireDate { get; init; }
        public string ContractType { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
    }

    public record EmployeeUpdateDto : EmployeeAddDto
    {

    }
