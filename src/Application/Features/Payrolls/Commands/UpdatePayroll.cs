using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Payrolls.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Payrolls.Commands;

/// <summary>
/// Command to update a payroll using DTO.
/// </summary>
public record UpdatePayrollCommand(PayrollUpdateDto Payroll) : IRequest<Response<string>>;

public class UpdatePayrollCommandHandler : IRequestHandler<UpdatePayrollCommand, Response<string>>
{
    private readonly ICommandRepository<Payroll> _commandRepository;
    private readonly IQueryRepository<Payroll> _queryRepository;

    public UpdatePayrollCommandHandler(
        ICommandRepository<Payroll> commandRepository,
        IQueryRepository<Payroll> queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<string>> Handle(UpdatePayrollCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Payroll;

        var entity = await _queryRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (entity == null)
        {
            return new Response<string>($"Payroll with ID {dto.Id} not found.");
        }

        entity.UserId = dto.UserId;
        entity.Period = dto.Period;
        entity.BaseSalary = dto.BaseSalary;
        entity.Bonuses = dto.Bonuses;
        entity.Deductions = dto.Deductions;
        entity.NetSalary = dto.NetSalary;
        entity.IsViewedByEmployee = dto.IsViewedByEmployee;

        await _commandRepository.UpdateAsync(entity, cancellationToken);

        return new Response<string>($"Payroll with ID {dto.Id} updated successfully.") { Succeeded = true };
    }
}

public class UpdatePayrollCommandValidator : AbstractValidator<UpdatePayrollCommand>
{
    public UpdatePayrollCommandValidator()
    {
        RuleFor(x => x.Payroll.Id).GreaterThan(0);
        RuleFor(x => x.Payroll.UserId).NotEmpty();
        RuleFor(x => x.Payroll.Period).NotEmpty();
        RuleFor(x => x.Payroll.BaseSalary).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Payroll.Bonuses).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Payroll.Deductions).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Payroll.NetSalary).GreaterThanOrEqualTo(0);
    }
}
