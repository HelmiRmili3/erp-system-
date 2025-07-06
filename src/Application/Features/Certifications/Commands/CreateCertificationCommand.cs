using System.Security.Claims;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Certifications.Dto;
using Backend.Application.Features.Certifications.IRepositories;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Certifications.Commands;

/// <summary>
/// Command to create a certification with file upload and user identity from JWT.
/// </summary>
public record CreateCertificationCommand(CreateCertificationDto Certification, IFormFile? File) : IRequest<Response<CertificationDto>>;

public class CreateCertificationCommandHandler : IRequestHandler<CreateCertificationCommand, Response<CertificationDto>>
{
    private readonly ICertificationCommandRepository _repository;
    private readonly IFileService _fileService;

    public CreateCertificationCommandHandler(
        ICertificationCommandRepository repository,
        IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task<Response<CertificationDto>> Handle(CreateCertificationCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Certification;
 
        string? fileUrl = null;
        if (request.File is { Length: > 0 })
        {
            var fileName = $"{Guid.NewGuid()}";
            var relativePath = await _fileService.SaveFileAsync(request.File, fileName, "certifications");
            fileUrl = $"/files/{relativePath.Replace("\\", "/")}";
        }

        var entity = new Certification
        {
            UserId = dto.UserId,
            Name = dto.Name,
            Authority = dto.Authority,
            DateObtained = dto.DateObtained,
            FileUrl = fileUrl
        };

        var result = await _repository.AddAsync(entity, cancellationToken);

        if (result == null)
        {
            return new Response<CertificationDto>("Failed to create certification.");
        }

        return new Response<CertificationDto>(result.ToDto<CertificationDto>(), "Certification created successfully.");
    }
}

public class CreateCertificationCommandValidator : AbstractValidator<CreateCertificationCommand>
{
    public CreateCertificationCommandValidator()
    {
        RuleFor(x => x.Certification.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Certification.Authority)
            .NotEmpty().WithMessage("Authority is required.")
            .MaximumLength(200).WithMessage("Authority must not exceed 200 characters.");

        RuleFor(x => x.Certification.DateObtained)
            .NotEmpty().WithMessage("Date obtained is required.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Date obtained cannot be in the future.");

        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File!.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("File must be less than or equal to 5MB.");
        });
    }
}
