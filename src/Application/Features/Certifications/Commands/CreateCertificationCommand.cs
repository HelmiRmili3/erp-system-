using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.Dto;
using Backend.Application.Features.Certifications.IRepositories;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Certifications.Commands;

public record CreateCertificationCommand : CreateCertificationDto, IRequest<Response<CertificationDto>>;

public class CreateCertificationCommandHandler : IRequestHandler<CreateCertificationCommand, Response<CertificationDto>>
{
    private readonly ICertificationCommandRepository _repository;

    public CreateCertificationCommandHandler(ICertificationCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<CertificationDto>> Handle(CreateCertificationCommand request, CancellationToken cancellationToken)
    {
        // Guard clauses for required fields
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
        Guard.Against.NullOrEmpty(request.Authority, nameof(request.Authority));
        Guard.Against.Null(request.DateObtained, nameof(request.DateObtained));
        Guard.Against.NullOrEmpty(request.FileUrl, nameof(request.FileUrl));

        var entity = new Certification
        {
            UserId = request.UserId,
            Name = request.Name,
            Authority = request.Authority,
            DateObtained = request.DateObtained,
            FileUrl = request.FileUrl
        };

        var result = await _repository.AddAsync(entity, cancellationToken);

        if (result == null)
        {
            throw new ApplicationException("Certification could not be created.");
        }

        return new Response<CertificationDto>(result.ToDto<CertificationDto>(), "Certification created successfully.");
    }
}

public class CreateCertificationCommandValidator : AbstractValidator<CreateCertificationCommand>
{
    public CreateCertificationCommandValidator()
    {
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
