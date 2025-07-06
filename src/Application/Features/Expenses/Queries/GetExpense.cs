using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Expenses.Queries;

public record GetExpenseByIdQuery(int Id) : IRequest<Response<ExpenseDto>>;

public class GetExpenseQueryHandler : IRequestHandler<GetExpenseByIdQuery, Response<ExpenseDto>>
{
    private readonly IQueryRepository<Expense> _repository;

    public GetExpenseQueryHandler(IQueryRepository<Expense> repository)
    {
        _repository = repository;
    }

    public async Task<Response<ExpenseDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<ExpenseDto>($"Expense with ID {request.Id} not found.")
                .WithError("NotFound", "Expense not found.");
        }

        var dto = new ExpenseDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Description = entity.Description,
            Amount = entity.Amount,
            ExpenseDate = entity.ExpenseDate,
            Category = entity.Category,
            Status = entity.Status,
        };

        return new Response<ExpenseDto>(dto, "Expense retrieved successfully.");
    }
}

public class GetExpenseQueryValidator : AbstractValidator<GetExpenseByIdQuery>
{
    public GetExpenseQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Expense ID must be greater than zero.");
    }
}
