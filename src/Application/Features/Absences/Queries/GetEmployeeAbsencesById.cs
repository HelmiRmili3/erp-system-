using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Backend.Application.Features.Absences.Queries;

public record GetEmployeeAbsencesByIdQuery(int? Month, int? Year) : IRequest<Response<List<AbsenceDto>>>;

public class GetEmployeeAbsencesByIdQueryHandler : IRequestHandler<GetEmployeeAbsencesByIdQuery, Response<List<AbsenceDto>>>
{
    private readonly IAbsenceQueryRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetEmployeeAbsencesByIdQueryHandler(IAbsenceQueryRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<List<AbsenceDto>>> Handle(GetEmployeeAbsencesByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new Response<List<AbsenceDto>>("User is not authenticated.");

        // Build expression filter manually
        Expression<Func<Absence, bool>> filter = a => a.UserId == userId;

        if (request.Month.HasValue)
        {
            var month = request.Month.Value;
            filter = Combine(filter, a => a.StartDate.Month == month);
        }

        if (request.Year.HasValue)
        {
            var year = request.Year.Value;
            filter = Combine(filter, a => a.StartDate.Year == year);
        }

        var absences = await _repository.GetAllByFilterAsync(filter, null, cancellationToken);
        var dtos = absences.Select(a => a.ToDto<AbsenceDto>()).ToList();

        return new Response<List<AbsenceDto>>(dtos, "Absences fetched successfully.");
    }

    // Combines two expressions with AND logic
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
