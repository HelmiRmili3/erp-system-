using System.Linq.Expressions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.Dto;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Parameters;

namespace Backend.Application.Features.Certifications.Queries;

public record GetAllCertificationsQuery(
    PagingParameter PagingParameter,
    string? UserId = null,
    int? Day = null,
    int? Month = null,
    int? Year = null
) : IRequest<PagedResponse<List<CertificationDto>>>;

public class GetAllCertificationsQueryHandler : IRequestHandler<GetAllCertificationsQuery, PagedResponse<List<CertificationDto>>>
{
    private readonly IQueryRepository<Certification> _repository;

    public GetAllCertificationsQueryHandler(IQueryRepository<Certification> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<List<CertificationDto>>> Handle(GetAllCertificationsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Certification, bool>> filter = c => true;

        if (!string.IsNullOrWhiteSpace(request.UserId))
        {
            filter = filter.AndAlso(c => c.UserId == request.UserId);
        }

        if (request.Year.HasValue)
        {
            filter = filter.AndAlso(c => c.DateObtained.Year == request.Year.Value);
        }

        if (request.Month.HasValue)
        {
            filter = filter.AndAlso(c => c.DateObtained.Month == request.Month.Value);
        }

        if (request.Day.HasValue)
        {
            filter = filter.AndAlso(c => c.DateObtained.Day == request.Day.Value);
        }

        var pagedResult = await _repository.GetPagedAsync(
                  filter: filter,
                  pageNumber: request.PagingParameter.PageNumber,
                  pageSize: request.PagingParameter.PageSize,
                  includeTable: null,
                  cancellationToken: cancellationToken
              );

        var dtoList = pagedResult.Data?.Select(a => a.ToDto<CertificationDto>()).ToList() ?? new List<CertificationDto>();

        return new PagedResponse<List<CertificationDto>>(
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

public class GetAllCertificationsQueryValidator : AbstractValidator<GetAllCertificationsQuery>
{
    public GetAllCertificationsQueryValidator()
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
            .InclusiveBetween(1900, DateTime.Today.Year)
            .When(x => x.Year.HasValue)
            .WithMessage($"Year must be between 1900 and {DateTime.Today.Year}.");
    }
}

// Helper extension to combine expressions with AND
public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var combined = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter));

        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }
}
