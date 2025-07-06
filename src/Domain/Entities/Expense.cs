
using Backend.Domain.Entities;

public class Expense : BaseAuditableEntity
{
    public required string UserId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending; // Pending, Approved, Rejected
    public string? ReceiptPath { get; set; }
}
