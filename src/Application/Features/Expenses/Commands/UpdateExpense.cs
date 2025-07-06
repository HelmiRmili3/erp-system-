using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Expenses.Commands;

public record UpdateExpenseCommand(ExpenseUpdateDto Expense) : IRequest<Response<int>>;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Response<int>>
{
    private readonly ICommandRepository<Expense> _commandRepository;
    private readonly IQueryRepository<Expense> _queryRepository;

    public UpdateExpenseCommandHandler(
        ICommandRepository<Expense> commandRepository,
        IQueryRepository<Expense> queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<int>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Expense;
        var entity = await _queryRepository.GetByIdAsync(dto.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<int>($"Expense with ID {dto.Id} not found.")
                .WithError("The specified expense record does not exist.");
        }

        entity.Description = dto.Description;
        entity.Amount = dto.Amount;
        entity.ExpenseDate = dto.ExpenseDate.ToUniversalTime();
        entity.Category = dto.Category;
        entity.Status = dto.Status;
        entity.ReceiptPath = dto.ReceiptPath;

        await _commandRepository.UpdateAsync(entity, cancellationToken);

        return new Response<int>(entity.Id, "Expense updated successfully.");
    }
}

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Expense.Id)
            .GreaterThan(0).WithMessage("Expense ID must be greater than 0.");

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
