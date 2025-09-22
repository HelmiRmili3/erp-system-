using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Payrolls.Dtos;
using Backend.Application.Features.User.IRepositories;
using Backend.Application.Features.User.Dto;
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
    private readonly IUserQueryRepository _userRepository;

    public GetPayrollQueryHandler(IQueryRepository<Payroll> repository, IUserQueryRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<Response<PayrollDto>> Handle(GetPayrollQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<PayrollDto>($"Payroll with ID {request.Id} not found.")
                .WithError("NotFound", "Payroll not found.");
        }

        UserDataDto? userDto = null;

        if (!string.IsNullOrWhiteSpace(entity.UserId))
        {
            userDto = await _userRepository.GetByIdAsync(entity.UserId, cancellationToken);
        }

        var dto = new PayrollDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Period = entity.Period,
            BaseSalary = entity.BaseSalary,
            Bonuses = entity.Bonuses,
            Deductions = entity.Deductions,
            NetSalary = entity.NetSalary,
            FileUrl = entity.FileUrl,
            IsViewedByEmployee = entity.IsViewedByEmployee,
            User = userDto
        };

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
