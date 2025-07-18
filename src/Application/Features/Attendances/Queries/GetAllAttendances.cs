using Backend.Application.Common.Extensions;
using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Attendances.Dto;
using System.Linq.Expressions;

public record GetAllAttendancesQuery(
    PagingParameter PagingParameter,
    string? UserId,
    int? Day,
    int? Month,
    int? Year
) : IRequest<PagedResponse<List<AttendanceDto>>>;

public class GetAllAttendancesQueryHandler : IRequestHandler<GetAllAttendancesQuery, PagedResponse<List<AttendanceDto>>>
{
    private readonly IAttendanceQueryRepository _repository;

    public GetAllAttendancesQueryHandler(IAttendanceQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<List<AttendanceDto>>> Handle(GetAllAttendancesQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Attendance, bool>> filter = a => true;

        if (!string.IsNullOrWhiteSpace(request.UserId))
            filter = Combine(filter, a => a.UserId == request.UserId);

        if (request.Day.HasValue)
            filter = Combine(filter, a => a.AttendanceDay.Day == request.Day.Value);

        if (request.Month.HasValue)
            filter = Combine(filter, a => a.AttendanceDay.Month == request.Month.Value);

        if (request.Year.HasValue)
            filter = Combine(filter, a => a.AttendanceDay.Year == request.Year.Value);

        // Fetch paged results
        var pagedResult = await _repository.GetPagedAsync(
            filter: filter,
            pageNumber: request.PagingParameter.PageNumber,
            pageSize: request.PagingParameter.PageSize,
            includeTable: null,
            cancellationToken: cancellationToken
        );

        var dtoList = pagedResult.Data?.Select(a => a.ToDto<AttendanceDto>()).ToList() ?? new List<AttendanceDto>();

        return new PagedResponse<List<AttendanceDto>>(
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

    private static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        var param = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(Expression.Invoke(first, param), Expression.Invoke(second, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

/// <summary>
/// Validator for GetAllAttendancesQuery.
/// </summary>
public class GetAllAttendancesQueryValidator : AbstractValidator<GetAllAttendancesQuery>
{
    public GetAllAttendancesQueryValidator()
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
            .InclusiveBetween(2000, DateTime.Now.Year + 1)
            .When(x => x.Year.HasValue)
            .WithMessage("Year must be valid.");
    }
}

