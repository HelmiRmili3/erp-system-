using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Common.Extensions;
using Backend.Application.Features.Payrolls.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;
using Backend.Application.Features.Certifications.Queries;

namespace Backend.Application.Features.Payrolls.Queries;

public record GetAllPayrollsQuery(
    string? UserId = null,
    int? Day = null,
    int? Month = null,
    int? Year = null
) : IRequest<Response<List<PayrollDto>>>;

public class GetPayrollsQueryHandler : IRequestHandler<GetAllPayrollsQuery, Response<List<PayrollDto>>>
{
    private readonly IQueryRepository<Payroll> _queryRepository;
    private readonly IMapper _mapper;

    public GetPayrollsQueryHandler(IQueryRepository<Payroll> queryRepository, IMapper mapper)
    {
        _queryRepository = queryRepository;
        _mapper = mapper;
    }

    public async Task<Response<List<PayrollDto>>> Handle(GetAllPayrollsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Payroll, bool>> filter = p => true;

        if (!string.IsNullOrWhiteSpace(request.UserId))
            filter = filter.AndAlso(p => p.UserId == request.UserId);

        if (request.Year.HasValue)
            filter = filter.AndAlso(p => p.Created.Year == request.Year.Value);

        if (request.Month.HasValue)
            filter = filter.AndAlso(p => p.Created.Month == request.Month.Value);

        if (request.Day.HasValue)
            filter = filter.AndAlso(p => p.Created.Day == request.Day.Value);

        var payrolls = await _queryRepository.GetAllByFilterAsync(filter, includeTable: null, cancellationToken);

        var dtos = payrolls.Select(p => _mapper.Map<PayrollDto>(p)).ToList();

        return new Response<List<PayrollDto>>(dtos, "Payrolls retrieved successfully.");
    }
}

public class GetPayrollsQueryValidator : AbstractValidator<GetAllPayrollsQuery>
{
    public GetPayrollsQueryValidator()
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
