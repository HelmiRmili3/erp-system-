using System.Linq.Expressions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Payrolls.Dtos;

using Backend.Application.Features.Certifications.Queries;
using Backend.Application.Common.Parameters;
using Backend.Application.Common.Extensions;

namespace Backend.Application.Features.Payrolls.Queries;

public record GetAllPayrollsQuery(
    PagingParameter PagingParameter,
    string? UserId = null,
    int? Month = null,
    int? Year = null
) : IRequest<PagedResponse<List<PayrollDto>>>;

public class GetPayrollsQueryHandler : IRequestHandler<GetAllPayrollsQuery, PagedResponse<List<PayrollDto>>>
{
    private readonly IQueryRepository<Payroll> _queryRepository;

    public GetPayrollsQueryHandler(IQueryRepository<Payroll> queryRepository, IMapper mapper)
    {
        _queryRepository = queryRepository;
    }

    public async Task<PagedResponse<List<PayrollDto>>> Handle(GetAllPayrollsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Payroll, bool>> filter = p => true;

        if (!string.IsNullOrWhiteSpace(request.UserId))
            filter = filter.AndAlso(p => p.UserId == request.UserId);

        if (request.Year.HasValue)
            filter = filter.AndAlso(p => p.Created.Year == request.Year.Value);

        if (request.Month.HasValue)
            filter = filter.AndAlso(p => p.Created.Month == request.Month.Value);


        var pagedResult = await _queryRepository.GetPagedAsync(
                 filter: filter,
                 pageNumber: request.PagingParameter.PageNumber,
                 pageSize: request.PagingParameter.PageSize,
                 includeTable: null,
                 cancellationToken: cancellationToken
             );

        var dtoList = pagedResult.Data?.Select(a => a.ToDto<PayrollDto>()).ToList() ?? new List<PayrollDto>();

        return new PagedResponse<List<PayrollDto>>(
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

public class GetPayrollsQueryValidator : AbstractValidator<GetAllPayrollsQuery>
{
    public GetPayrollsQueryValidator()
    {
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
