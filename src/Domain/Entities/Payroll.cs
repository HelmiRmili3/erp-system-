using System.ComponentModel.DataAnnotations.Schema;

using Backend.Domain.Entities;

public class Payroll : BaseAuditableEntity
{
    public required string UserId { get; set; }
    /// <summary>
    /// Month/year period (e.g., "2025-06" or "June 2025")
    /// </summary>
    public string Period { get; set; } = null!;

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
    public bool IsViewedByEmployee { get; set; } = false;
}
