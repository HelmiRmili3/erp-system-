using System.Linq.Expressions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Contracts.Dtos;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Parameters;
using Backend.Application.Features.Certifications.Queries;

namespace Backend.Application.Features.Contracts.Queries;

public record GetAllContractsQuery(
    PagingParameter PagingParameter,
    string? UserId = null,
    int? Day = null,
    int? Month = null,
    int? Year = null
) : IRequest<PagedResponse<List<ContractDto>>>;

public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, PagedResponse<List<ContractDto>>>
{
    private readonly IQueryRepository<Contract> _repository;

    public GetAllContractsQueryHandler(IQueryRepository<Contract> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<List<ContractDto>>> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Contract, bool>> filter = c => true;

        if (!string.IsNullOrWhiteSpace(request.UserId))
        {
            filter = filter.AndAlso(c => c.UserId == request.UserId);
        }

        if (request.Year.HasValue)
        {
            filter = filter.AndAlso(c => c.StartDate.Year == request.Year.Value);
        }

        if (request.Month.HasValue)
        {
            filter = filter.AndAlso(c => c.StartDate.Month == request.Month.Value);
        }

        if (request.Day.HasValue)
        {
            filter = filter.AndAlso(c => c.StartDate.Day == request.Day.Value);
        }

        var pagedResult = await _repository.GetPagedAsync(
            filter,
            request.PagingParameter.PageNumber,
            request.PagingParameter.PageSize,
            includeTable: null,
            cancellationToken
        );

        var dtoList = pagedResult.Data?.Select(a => a.ToDto<ContractDto>()).ToList() ?? new List<ContractDto>();

        return new PagedResponse<List<ContractDto>>(
            data: dtoList,
            pageNumber: request.PagingParameter.PageNumber,
            pageSize: request.PagingParameter.PageSize,
            recordsCount: new RecordsCount
            {
                RecordsFiltered = pagedResult.RecordsFiltered,
                RecordsTotal = pagedResult.RecordsTotal
            }
        );
    }
}

public class GetAllContractsQueryValidator : AbstractValidator<GetAllContractsQuery>
{
    public GetAllContractsQueryValidator()
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
