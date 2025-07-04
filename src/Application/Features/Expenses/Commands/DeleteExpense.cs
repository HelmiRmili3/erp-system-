using Backend.Application.Common.Response;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Expenses.Commands;

public record DeleteExpenseCommand(int Id) : IRequest<Response<int>>;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Response<int>>
{
    private readonly ICommandRepository<Expense> _commandRepository;
    private readonly IQueryRepository<Expense> _queryRepository;

    public DeleteExpenseCommandHandler(
        ICommandRepository<Expense> commandRepository,
        IQueryRepository<Expense> queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<int>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<int>($"Expense with ID {request.Id} not found")
                .WithError($"Expense with ID {request.Id} does not exist.");
        }

        await _commandRepository.DeleteAsync(entity, cancellationToken);
        return new Response<int>(entity.Id, "Expense deleted successfully");
    }
}

public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Expense ID must be greater than 0.");
    }
}
