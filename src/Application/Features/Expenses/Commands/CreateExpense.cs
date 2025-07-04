using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Expenses.Commands;

public record CreateExpenseCommand(ExpenseAddDto Expense) : IRequest<Response<int>>;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Response<int>>
{
    private readonly ICommandRepository<Expense> _repository;

    public CreateExpenseCommandHandler(ICommandRepository<Expense> repository)
    {
        _repository = repository;
    }

    public async Task<Response<int>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Expense;

        var entity = new Expense
        {
            UserId = dto.UserId,
            Description = dto.Description,
            Amount = dto.Amount,
            ExpenseDate = dto.ExpenseDate.ToUniversalTime(),
            Category = dto.Category,
            Status = dto.Status,
            ReceiptPath = dto.ReceiptPath
        };

        await _repository.AddAsync(entity, cancellationToken);

        return new Response<int>(entity.Id, "Expense created successfully");
    }
}

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.Expense.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Expense.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Expense.ExpenseDate)
            .NotEmpty().WithMessage("Expense date is required.");

        RuleFor(x => x.Expense.Category)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.Expense.ReceiptPath)
            .NotEmpty().WithMessage("Receipt path is required.");
    }
}
