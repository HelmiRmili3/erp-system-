using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Attendances.Dto;

using System.Linq.Expressions;


/// <summary>
/// Query to get all attendances, optionally filtered by day/month/year.
/// </summary>
public record GetAllAttendancesQuery(int? Day, int? Month, int? Year) : IRequest<Response<List<AttendanceDto>>>;

/// <summary>
/// Handler for GetAllAttendancesQuery.
/// </summary>
public class GetAllAttendancesQueryHandler : IRequestHandler<GetAllAttendancesQuery, Response<List<AttendanceDto>>>
{
    private readonly IAttendanceQueryRepository _repository;

    public GetAllAttendancesQueryHandler(IAttendanceQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<AttendanceDto>>> Handle(GetAllAttendancesQuery request, CancellationToken cancellationToken)
    {
        // Base expression: match all
        Expression<Func<Attendance, bool>> filter = a => true;

        if (request.Day.HasValue)
        {
            var day = request.Day.Value;
            filter = Combine(filter, a => a.AttendanceDate.Day == day);
        }

        if (request.Month.HasValue)
        {
            var month = request.Month.Value;
            filter = Combine(filter, a => a.AttendanceDate.Month == month);
        }

        if (request.Year.HasValue)
        {
            var year = request.Year.Value;
            filter = Combine(filter, a => a.AttendanceDate.Year == year);
        }

        var attendances = await _repository.GetAllByFilterAsync(filter, null, cancellationToken);
        var dtos = attendances.Select(a => a.ToDto<AttendanceDto>()).ToList();

        return new Response<List<AttendanceDto>>(dtos, "Attendances fetched successfully.");
    }

    /// <summary>
    /// Combines two expressions with logical AND.
    /// </summary>
    private static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        var param = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(first, param),
            Expression.Invoke(second, param)
        );

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
