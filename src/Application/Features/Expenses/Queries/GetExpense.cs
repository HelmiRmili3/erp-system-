using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Expenses.Queries;

public record GetExpenseByIdQuery(int Id) : IRequest<Response<ExpenseDto>>;

public class GetExpenseQueryHandler : IRequestHandler<GetExpenseByIdQuery, Response<ExpenseDto>>
{
    private readonly IQueryRepository<Expense> _repository;
    private readonly IUserQueryRepository _userRepository;

    public GetExpenseQueryHandler(
        IQueryRepository<Expense> repository,
        IUserQueryRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<Response<ExpenseDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<ExpenseDto>($"Expense with ID {request.Id} not found.")
                .WithError("NotFound", "Expense not found.");
        }

        UserDataDto? userDto = null;
        if (!string.IsNullOrWhiteSpace(entity.UserId))
        {
            userDto = await _userRepository.GetByIdAsync(entity.UserId, cancellationToken);
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
            ReceiptPath = entity.ReceiptPath!,
            User = userDto
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
