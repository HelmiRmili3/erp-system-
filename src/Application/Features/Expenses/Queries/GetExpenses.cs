using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Common.Extensions;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;
using Backend.Application.Features.Certifications.Queries;

namespace Backend.Application.Features.Expenses.Queries
{
    public record GetAllExpensesQuery(
        string? UserId = null,
        int? Day = null,
        int? Month = null,
        int? Year = null
    ) : IRequest<Response<List<ExpenseDto>>>;

    public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, Response<List<ExpenseDto>>>
    {
        private readonly IQueryRepository<Expense> _queryRepository;
        private readonly ICommandRepository<Expense> _commandRepository; // Included but not used here

        public GetAllExpensesQueryHandler(
            IQueryRepository<Expense> queryRepository,
            ICommandRepository<Expense> commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task<Response<List<ExpenseDto>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Expense, bool>> filter = e => true;

            if (!string.IsNullOrWhiteSpace(request.UserId))
                filter = filter.AndAlso(e => e.UserId == request.UserId);

            if (request.Year.HasValue)
                filter = filter.AndAlso(e => e.ExpenseDate.Year == request.Year.Value);

            if (request.Month.HasValue)
                filter = filter.AndAlso(e => e.ExpenseDate.Month == request.Month.Value);

            if (request.Day.HasValue)
                filter = filter.AndAlso(e => e.ExpenseDate.Day == request.Day.Value);

            var expenses = await _queryRepository.GetAllByFilterAsync(filter, includeTable: null, cancellationToken);

            var dtos = expenses.Select(e => new ExpenseDto
            {
                Id = e.Id,
                UserId = e.UserId,
                Description = e.Description,
                Amount = e.Amount,
                ExpenseDate = e.ExpenseDate,
                Category = e.Category,
                Status = e.Status,
            }).ToList();

            return new Response<List<ExpenseDto>>(dtos, "Expenses retrieved successfully.");
        }
    }

    public class GetAllExpensesQueryValidator : AbstractValidator<GetAllExpensesQuery>
    {
        public GetAllExpensesQueryValidator()
        {
            RuleFor(x => x.Day)
                .InclusiveBetween(1, 31)
                .When(x => x.Day.HasValue)
                .WithMessage("Day must be between 1 and 31.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .When(x => x.Month.HasValue)
                .WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.UtcNow.Year)
                .When(x => x.Year.HasValue)
                .WithMessage($"Year must be between 1900 and {DateTime.UtcNow.Year}.");
        }
    }
}
