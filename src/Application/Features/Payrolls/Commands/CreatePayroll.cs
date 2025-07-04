using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Payrolls.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Payrolls.Commands;

/// <summary>
/// Command to create a new payroll record.
/// </summary>
public record CreatePayrollCommand(PayrollAddDto Payroll) : IRequest<Response<int>>;

public class CreatePayrollCommandHandler : IRequestHandler<CreatePayrollCommand, Response<int>>
{
    private readonly ICommandRepository<Payroll> _repository;

    public CreatePayrollCommandHandler(ICommandRepository<Payroll> repository)
    {
        _repository = repository;
    }

    public async Task<Response<int>> Handle(CreatePayrollCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Payroll;

        var entity = new Payroll
        {
            UserId = dto.UserId,
            Period = dto.Period,
            BaseSalary = dto.BaseSalary,
            Bonuses = dto.Bonuses,
            Deductions = dto.Deductions,
            NetSalary = dto.NetSalary,
            FileUrl = dto.FileUrl,
            IsViewedByEmployee = dto.IsViewedByEmployee
        };

        await _repository.AddAsync(entity, cancellationToken);
        return new Response<int>(entity.Id, "Payroll created successfully.");
    }
}

public class CreatePayrollCommandValidator : AbstractValidator<CreatePayrollCommand>
{
    public CreatePayrollCommandValidator()
    {
        RuleFor(x => x.Payroll.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Payroll.Period)
            .NotEmpty().WithMessage("Period is required.")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("Period must be in the format YYYY-MM.");

        RuleFor(x => x.Payroll.BaseSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Base salary must be non-negative.");

        RuleFor(x => x.Payroll.NetSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Net salary must be non-negative.");
    }
}
