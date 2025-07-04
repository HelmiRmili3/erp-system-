using System.Linq.Expressions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Contracts.Dtos;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Extensions;
using Backend.Domain.Entities;
using MediatR;
using Backend.Application.Features.Certifications.Queries;

namespace Backend.Application.Features.Contracts.Queries;

/// <summary>
/// Query to get all contracts optionally filtered by user and date.
/// </summary>
public record GetAllContractsQuery(
    string? UserId = null,
    int? Day = null,
    int? Month = null,
    int? Year = null
) : IRequest<Response<List<ContractDto>>>;

public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, Response<List<ContractDto>>>
{
    private readonly IQueryRepository<Contract> _repository;

    public GetAllContractsQueryHandler(IQueryRepository<Contract> repository)
    {
        _repository = repository;
    }

    public async Task<Response<List<ContractDto>>> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
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

        var contracts = await _repository.GetAllByFilterAsync(filter, includeTable: null, cancellationToken);
        var dtos = contracts.Select(c => c.ToDto<ContractDto>()).ToList();

        return new Response<List<ContractDto>>(dtos, "Contracts retrieved successfully.");
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
