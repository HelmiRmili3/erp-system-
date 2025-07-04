using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Payrolls.Commands;

/// <summary>
/// Command to delete a payroll by ID.
/// </summary>
public record DeletePayrollCommand(int Id) : IRequest<Response<string>>;

public class DeletePayrollCommandHandler : IRequestHandler<DeletePayrollCommand, Response<string>>
{
    private readonly ICommandRepository<Payroll> _commandRepository;
    private readonly IQueryRepository<Payroll> _queryRepository;

    public DeletePayrollCommandHandler(
        ICommandRepository<Payroll> commandRepository,
        IQueryRepository<Payroll> queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<string>> Handle(DeletePayrollCommand request, CancellationToken cancellationToken)
    {
        var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<string>($"Payroll with ID {request.Id} not found.");
        }

        await _commandRepository.DeleteAsync(entity, cancellationToken);
        return new Response<string>($"Payroll with ID {request.Id} deleted successfully.") { Succeeded = true };
    }
}

public class DeletePayrollCommandValidator : AbstractValidator<DeletePayrollCommand>
{
    public DeletePayrollCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Payroll ID must be greater than 0.");
    }
}
