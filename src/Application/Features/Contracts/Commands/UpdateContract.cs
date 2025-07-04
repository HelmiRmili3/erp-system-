using Backend.Application.Common.Response;
using Backend.Application.Common.Interfaces;
using Backend.Application.Features.Contracts.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Contracts.Commands;

public record UpdateContractCommand(ContractUpdateDto Contract) : IRequest<Response<int>>;

public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, Response<int>>
{
    private readonly ICommandRepository<Contract> _commandRepository;
    private readonly IQueryRepository<Contract> _queryRepository;

    public UpdateContractCommandHandler(
        ICommandRepository<Contract> commandRepository,
        IQueryRepository<Contract> queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<int>> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Contract;

        var entity = await _queryRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (entity == null)
        {
            return new Response<int>($"Contract with ID {dto.Id} not found")
                .WithError($"Contract with ID {dto.Id} does not exist.");
        }

        // Update fields
        entity.UserId = dto.UserId;
        entity.ContractType = dto.ContractType;
        entity.StartDate = dto.StartDate.ToUniversalTime();
        entity.EndDate = dto.EndDate?.ToUniversalTime();
        entity.FileUrl = dto.FileUrl;
        entity.Status = dto.Status;

        await _commandRepository.UpdateAsync(entity, cancellationToken);

        return new Response<int>(entity.Id, "Contract updated successfully");
    }
}

public class UpdateContractCommandValidator : AbstractValidator<UpdateContractCommand>
{
    public UpdateContractCommandValidator()
    {
        RuleFor(x => x.Contract.Id)
            .GreaterThan(0).WithMessage("Contract ID must be greater than 0.");

        RuleFor(x => x.Contract.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Contract.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.Contract.EndDate)
            .GreaterThan(x => x.Contract.StartDate)
            .When(x => x.Contract.EndDate.HasValue)
            .WithMessage("End date must be after the start date.");
    }
}
