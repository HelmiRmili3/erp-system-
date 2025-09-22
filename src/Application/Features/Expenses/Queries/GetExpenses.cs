using System.Linq.Expressions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Expenses.Dtos;
using Backend.Application.Common.Parameters;
using Backend.Application.Common.Extensions;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;
using Backend.Application.Features.Certifications.Queries;

namespace Backend.Application.Features.Expenses.Queries
{
    public record GetAllExpensesQuery(
        PagingParameter PagingParameter,
        string? UserId = null,
        int? Day = null,
        int? Month = null,
        int? Year = null
    ) : IRequest<PagedResponse<List<ExpenseDto>>>;

    public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, PagedResponse<List<ExpenseDto>>>
    {
        private readonly IQueryRepository<Expense> _queryRepository;
        private readonly IUserQueryRepository _userRepository;

        public GetAllExpensesQueryHandler(
            IQueryRepository<Expense> queryRepository,
            IUserQueryRepository userRepository)
        {
            _queryRepository = queryRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResponse<List<ExpenseDto>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
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

            var pagedResult = await _queryRepository.GetPagedAsync(
                filter: filter,
                pageNumber: request.PagingParameter.PageNumber,
                pageSize: request.PagingParameter.PageSize,
                includeTable: null,
                cancellationToken: cancellationToken
            );

            var dtoList = new List<ExpenseDto>();

            if (pagedResult.Data != null)
            {
                foreach (var expense in pagedResult.Data)
                {
                    UserDataDto? userDto = null;

                    if (!string.IsNullOrWhiteSpace(expense.UserId))
                    {
                        userDto = await _userRepository.GetByIdAsync(expense.UserId, cancellationToken);
                    }

                    dtoList.Add(new ExpenseDto
                    {
                        Id = expense.Id,
                        UserId = expense.UserId,
                        Description = expense.Description,
                        Amount = expense.Amount,
                        ExpenseDate = expense.ExpenseDate,
                        Category = expense.Category,
                        Status = expense.Status,
                        ReceiptPath = expense.ReceiptPath!,
                        User = userDto
                    });
                }
            }

            return new PagedResponse<List<ExpenseDto>>(
                data: dtoList,
                pageNumber: pagedResult.PageNumber,
                pageSize: pagedResult.PageSize,
                recordsCount: new RecordsCount
                {
                    RecordsFiltered = pagedResult.RecordsFiltered,
                    RecordsTotal = pagedResult.RecordsTotal
                }
            );
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
