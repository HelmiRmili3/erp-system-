using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.IRepositories;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Certifications.Commands;

/// <summary>
/// Command to delete a certification by Id.
/// </summary>
public record DeleteCertificationCommand(int Id) : IRequest<Response<int>>;

/// <summary>
/// Handles the deletion of a certification.
/// </summary>
public class DeleteCertificationCommandHandler : IRequestHandler<DeleteCertificationCommand, Response<int>>
{
    private readonly ICertificationCommandRepository _repository;
    private readonly ICertificationQueryRepository _queryRepository;

    public DeleteCertificationCommandHandler(
        ICertificationCommandRepository repository,
        ICertificationQueryRepository queryRepository)
    {
        _repository = repository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<int>> Handle(DeleteCertificationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            return new Response<int>($"Certification with Id {request.Id} not found")
                .WithError("Certification not found", "NotFound");
        }

        await _repository.DeleteAsync(entity, cancellationToken);

        return new Response<int>(request.Id, "Certification deleted successfully");
    }
}

public class DeleteCertificationCommandValidator : AbstractValidator<DeleteCertificationCommand>
{
    public DeleteCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Certification Id must be greater than 0.");
    }
}
