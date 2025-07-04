using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.Dto;
using Backend.Application.Features.Certifications.IRepositories;


namespace Backend.Application.Features.Certifications.Commands;

public record UpdateCertificationCommand : UpdateCertificationDto, IRequest<Response<CertificationDto>>;

public class UpdateCertificationCommandHandler : IRequestHandler<UpdateCertificationCommand, Response<CertificationDto>>
{
    private readonly ICertificationCommandRepository _commandRepository;
    private readonly ICertificationQueryRepository _queryRepository;

    public UpdateCertificationCommandHandler(
        ICertificationCommandRepository commandRepository,
        ICertificationQueryRepository queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<CertificationDto>> Handle(UpdateCertificationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return new Response<CertificationDto>($"Certification with Id {request.Id} not found.");

        // Update fields
        entity.UserId = request.UserId;
        entity.Name = request.Name;
        entity.Authority = request.Authority;
        entity.DateObtained = request.DateObtained;
        entity.FileUrl = request.FileUrl;

        await _commandRepository.UpdateAsync(entity, cancellationToken);

        return new Response<CertificationDto>(entity.ToDto<CertificationDto>(), "Certification updated successfully.");
    }
}

public class UpdateCertificationCommandValidator : AbstractValidator<UpdateCertificationCommand>
{
    public UpdateCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Certification Id must be greater than 0.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Authority)
            .NotEmpty().WithMessage("Authority is required.")
            .MaximumLength(200).WithMessage("Authority must not exceed 200 characters.");

        RuleFor(x => x.DateObtained)
            .NotEmpty().WithMessage("Date obtained is required.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Date obtained cannot be in the future.");

        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("File URL is required.")
            .MaximumLength(2048).WithMessage("File URL must not exceed 2048 characters.");
    }
}
