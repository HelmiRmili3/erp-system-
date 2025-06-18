using System;
using Backend.Domain.Entities;

namespace Backend.Domain.Entities;

public class Expense : BaseAuditableEntity
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

    // Foreign key to Employee
    public int EmployeeId { get; set; }

    // Navigation property
    public Employee Employee { get; set; } = null!;

    // Receipt attachment (could be file path or byte[])
    public string ReceiptPath { get; set; } = string.Empty;
}
