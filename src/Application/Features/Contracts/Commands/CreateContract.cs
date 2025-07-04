using Backend.Application.Common.Response;
using Backend.Application.Features.Contracts.Dtos;
using Backend.Domain.Entities;
using Backend.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Contracts.Commands;

public record CreateContractCommand(ContractAddDto Contract) : IRequest<Response<int>>;

public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, Response<int>>
{
    private readonly ICommandRepository<Contract> _repository;

    public CreateContractCommandHandler(ICommandRepository<Contract> repository)
    {
        _repository = repository;
    }

    public async Task<Response<int>> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Contract;

        var entity = new Contract
        {
            UserId = dto.UserId,
            ContractType = dto.ContractType,
            StartDate = dto.StartDate.ToUniversalTime(),
            EndDate = dto.EndDate?.ToUniversalTime(),
            FileUrl = dto.FileUrl,
            Status = dto.Status
        };

        await _repository.AddAsync(entity, cancellationToken);
        return new Response<int>(entity.Id, "Contract created successfully");
    }
}

public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
{
    public CreateContractCommandValidator()
    {
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
