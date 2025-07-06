using System.Security.Claims;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Expenses.Commands;

/// <summary>
/// Command to create an expense with receipt upload.
/// </summary>
public record CreateExpenseCommand(ExpenseAddDto Expense, IFormFile? File) : IRequest<Response<int>>;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Response<int>>
{
    private readonly ICommandRepository<Expense> _repository;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateExpenseCommandHandler(
        ICommandRepository<Expense> repository,
        IFileService fileService,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<int>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Expense;

        // Extract user ID from JWT token claims
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            return new Response<int>("Unauthorized: User ID not found.");

        string? receiptUrl = null;

        if (request.File is { Length: > 0 })
        {
            var fileName = $"{Guid.NewGuid()}";
            var relativePath = await _fileService.SaveFileAsync(request.File, fileName, "expenses");
            receiptUrl = $"/files/{relativePath.Replace("\\", "/")}";
        }

        var expense = new Expense
        {
            UserId = userId,
            Description = dto.Description,
            Amount = dto.Amount,
            ExpenseDate = dto.ExpenseDate.ToUniversalTime(),
            Category = dto.Category,
            Status = ExpenseStatus.Pending,
            ReceiptPath = receiptUrl
        };

        await _repository.AddAsync(expense, cancellationToken);

        return new Response<int>(expense.Id, "Expense created successfully.");
    }
}

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.Expense.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Expense.ExpenseDate)
            .NotEmpty().WithMessage("Expense date is required.");

        RuleFor(x => x.Expense.Category)
            .NotEmpty().WithMessage("Category is required.");

        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File!.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024) // Max 5MB
                .WithMessage("File must be less than or equal to 5MB.");
        });
    }
}
