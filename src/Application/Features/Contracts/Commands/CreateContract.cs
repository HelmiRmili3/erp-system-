using System.Security.Claims;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Contracts.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Contracts.Commands;

/// <summary>
/// Command to create a contract with file upload.
/// </summary>
public record CreateContractCommand(ContractAddDto Contract, IFormFile? File) : IRequest<Response<int>>;

public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, Response<int>>
{
    private readonly ICommandRepository<Contract> _repository;
    private readonly IFileService _fileService;

    public CreateContractCommandHandler(
        ICommandRepository<Contract> repository,
        IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task<Response<int>> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Contract;

        string? fileUrl = null;

        if (request.File is { Length: > 0 })
        {
            var fileName = $"{Guid.NewGuid()}";
            var relativePath = await _fileService.SaveFileAsync(request.File, fileName, "contracts");
            fileUrl = $"/files/{relativePath.Replace("\\", "/")}";
        }

        var entity = new Contract
        {
            UserId = dto.UserId,
            ContractType = dto.ContractType,
            StartDate = dto.StartDate.ToUniversalTime(),
            EndDate = dto.EndDate?.ToUniversalTime(),
            FileUrl = fileUrl,
            Status = dto.Status
        };

        await _repository.AddAsync(entity, cancellationToken);
        return new Response<int>(entity.Id, "Contract created successfully.");
    }
}

public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
{
    public CreateContractCommandValidator()
    {
        RuleFor(x => x.Contract.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.Contract.EndDate)
            .GreaterThan(x => x.Contract.StartDate)
            .When(x => x.Contract.EndDate.HasValue)
            .WithMessage("End date must be after the start date.");

        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File!.Length)
                .LessThanOrEqualTo(10 * 1024 * 1024) // e.g., Max 10MB
                .WithMessage("File must be less than or equal to 10MB.");
        });
    }
}
