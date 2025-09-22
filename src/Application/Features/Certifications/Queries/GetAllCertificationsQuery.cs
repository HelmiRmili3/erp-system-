using System.Linq.Expressions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.Dto;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Parameters;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;

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
    private readonly IUserQueryRepository _userRepository;

    public GetAllCertificationsQueryHandler(
        IQueryRepository<Certification> repository,
        IUserQueryRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
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
            includeTable: null, // No navigation property here
            cancellationToken: cancellationToken
        );

        var dtoList = new List<CertificationDto>();

        if (pagedResult.Data != null)
        {
            foreach (var cert in pagedResult.Data)
            {
                UserDataDto? userDto = null;

                if (!string.IsNullOrWhiteSpace(cert.UserId))
                {
                    userDto = await _userRepository.GetByIdAsync(cert.UserId, cancellationToken);
                }

                dtoList.Add(new CertificationDto
                {
                    Id = cert.Id,
                    UserId = cert.UserId,
                    Name = cert.Name,
                    Authority = cert.Authority,
                    DateObtained = cert.DateObtained,
                    FileUrl = cert.FileUrl ?? string.Empty,
                    User = userDto
                });
            }
        }

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
