using AutoMapper;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Payrolls.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Application.Features.Payrolls.Queries;

public record GetPayrollQuery(int Id) : IRequest<Response<PayrollDto>>;

public class GetPayrollQueryHandler : IRequestHandler<GetPayrollQuery, Response<PayrollDto>>
{
    private readonly IQueryRepository<Payroll> _repository;
    private readonly IMapper _mapper;

    public GetPayrollQueryHandler(IQueryRepository<Payroll> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<PayrollDto>> Handle(GetPayrollQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<PayrollDto>($"Payroll with ID {request.Id} not found.")
                .WithError("NotFound", "Payroll not found.");
        }

        var dto = _mapper.Map<PayrollDto>(entity);

        return new Response<PayrollDto>(dto, "Payroll retrieved successfully.");
    }
}

public class GetPayrollQueryValidator : AbstractValidator<GetPayrollQuery>
{
    public GetPayrollQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Payroll ID must be greater than zero.");
    }
}
