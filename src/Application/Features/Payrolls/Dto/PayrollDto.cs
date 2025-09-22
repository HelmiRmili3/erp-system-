using Backend.Application.Features.User.Dto;

namespace Backend.Application.Features.Payrolls.Dtos
{
    public class PayrollDto : BasePayrollDto
    {
        public int Id { get; init; }
        public UserDataDto? User { get; set; }

    }

    public class BasePayrollDto
    {
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Month/year period (e.g., "2025-06" or "June 2025")
        /// </summary>
        public string Period { get; set; } = string.Empty;

        public decimal BaseSalary { get; set; }

        public decimal Bonuses { get; set; }

        public decimal Deductions { get; set; }

        public decimal NetSalary { get; set; }

        /// <summary>
        /// Link to the uploaded PDF or external payroll file
        /// </summary>
        public string? FileUrl { get; set; }

        /// <summary>
        /// Indicates if the employee has opened the payslip
        /// </summary>
        public bool IsViewedByEmployee { get; set; }
    }

    public record PayrollAddDto
    {
        public required string UserId { get; init; }
        public required string Period { get; init; }
        public decimal BaseSalary { get; init; }
        public decimal Bonuses { get; init; }
        public decimal Deductions { get; init; }
        public decimal NetSalary { get; init; }
        public bool IsViewedByEmployee { get; init; } = false;
    }

    public record PayrollUpdateDto : PayrollAddDto
    {
        public int Id { get; init; }
    }
}
