using Backend.Application.Common.Response;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Contracts.Commands;

public record DeleteContractCommand(int Id) : IRequest<Response<int>>;

public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand, Response<int>>
{
    private readonly ICommandRepository<Contract> _commandRepository;
    private readonly IQueryRepository<Contract> _queryRepository;

    public DeleteContractCommandHandler(
        ICommandRepository<Contract> commandRepository,
        IQueryRepository<Contract> queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<int>> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
    {
        var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<int>($"Contract with ID {request.Id} not found")
                .WithError($"Contract with ID {request.Id} does not exist.");
        }

        await _commandRepository.DeleteAsync(entity, cancellationToken);
        return new Response<int>(entity.Id, "Contract deleted successfully");
    }
}

public class DeleteContractCommandValidator : AbstractValidator<DeleteContractCommand>
{
    public DeleteContractCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Contract ID must be greater than 0.");
    }
}
