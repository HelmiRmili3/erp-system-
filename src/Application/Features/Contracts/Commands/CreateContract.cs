using System.Security.Claims;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
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
    private readonly IMapper _mapper;


    public CreateContractCommandHandler(
        ICommandRepository<Contract> repository,
        IFileService fileService,
         IMapper mapper)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;

    }

    public async Task<Response<int>> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        string? fileUrl = null;

        if (request.File is { Length: > 0 })
        {
            var fileName = $"{Guid.NewGuid()}";
            var relativePath = await _fileService.SaveFileAsync(request.File, fileName, "contracts");
            fileUrl = $"/files/{relativePath.Replace("\\", "/")}";
        }

        // Map manually
        var contract = new Contract
        {
            UserId = request.Contract.UserId,
            ContractType = request.Contract.ContractType,
            StartDate = request.Contract.StartDate,
            EndDate = request.Contract.EndDate,
            Status = request.Contract.Status,
            FileUrl = fileUrl // safe assignment
        };

        await _repository.AddAsync(contract, cancellationToken);

        return new Response<int>(contract.Id, "Contract created successfully.");
    }


}

public class CreateContractCommandValidator : AbstractValidator<ContractAddDto>
{
    public CreateContractCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("End date must be after the start date.");
    }
}
