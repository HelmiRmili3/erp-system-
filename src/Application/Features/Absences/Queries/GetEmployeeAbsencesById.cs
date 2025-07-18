using Backend.Application.Common.Extensions;
using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;

public record GetEmployeeAbsencesByIdQuery(PagingParameter PagingParameter,int? Month, int? Year)
    : IRequest<PagedResponse<List<AbsenceDto>>>;

public class GetEmployeeAbsencesByIdQueryHandler : IRequestHandler<GetEmployeeAbsencesByIdQuery, PagedResponse<List<AbsenceDto>>>
{
    private readonly IAbsenceQueryRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetEmployeeAbsencesByIdQueryHandler(IAbsenceQueryRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PagedResponse<List<AbsenceDto>>> Handle(GetEmployeeAbsencesByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                var response = new PagedResponse<List<AbsenceDto>>(
                  new List<AbsenceDto>(),
                  0,
                  0,
                  new RecordsCount { RecordsFiltered = 0, RecordsTotal = 0 }
              );
                response.Message = "User is not authenticated.";
                return response;
            }
        // Base filter for UserId
        Expression<Func<Absence, bool>> filter = a => a.UserId == userId;

        if (request.Month.HasValue)
            filter = Combine(filter, a => a.StartDate.Month == request.Month.Value);

        if (request.Year.HasValue)
            filter = Combine(filter, a => a.StartDate.Year == request.Year.Value);

        var pagedResult = await _repository.GetPagedAsync(
            filter: filter,
            pageNumber: request.PagingParameter.PageNumber,
            pageSize: request.PagingParameter.PageSize,
            includeTable: null,
            cancellationToken: cancellationToken
        );

        var dtoList = pagedResult.Data?.Select(a => a.ToDto<AbsenceDto>()).ToList() ?? new List<AbsenceDto>();

        return new PagedResponse<List<AbsenceDto>>(
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
        var body = Expression.AndAlso(
            Expression.Invoke(first, param),
            Expression.Invoke(second, param)
        );
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
