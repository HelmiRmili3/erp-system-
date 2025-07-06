using Backend.Domain.Enums;

namespace Backend.Application.Features.Expenses.Dtos
{
    public class ExpenseDto : BaseExpenseDto
    {
        public int Id { get; init; }
    }

    public class BaseExpenseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
        public string ReceiptPath { get; set; } = string.Empty;
    }

    public record ExpenseAddDto
    {
        public string Description { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime ExpenseDate { get; init; }
        public string Category { get; init; } = string.Empty;
    }

    public record ExpenseUpdateDto : ExpenseAddDto
    {
        public int Id { get; init; }
        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
        public string ReceiptPath { get; set; } = string.Empty;
    }
}

