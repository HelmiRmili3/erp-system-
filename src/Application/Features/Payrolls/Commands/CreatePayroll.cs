using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Payrolls.Dtos;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Payrolls.Commands;

/// <summary>
/// Command to create a new payroll record with optional file upload.
/// </summary>
public record CreatePayrollCommand(PayrollAddDto Payroll, IFormFile? File) : IRequest<Response<int>>;

/// <summary>
/// Handles the creation of a new payroll.
/// </summary>
public class CreatePayrollCommandHandler : IRequestHandler<CreatePayrollCommand, Response<int>>
{
    private readonly ICommandRepository<Payroll> _repository;
    private readonly IFileService _fileService;

    public CreatePayrollCommandHandler(ICommandRepository<Payroll> repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task<Response<int>> Handle(CreatePayrollCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Payroll;

        string? fileUrl = null;
        if (request.File is { Length: > 0 })
        {
            var fileName = $"{Guid.NewGuid()}";
            var relativePath = await _fileService.SaveFileAsync(request.File, fileName, "payrolls");
            fileUrl = $"/files/{relativePath.Replace("\\", "/")}";
        }

        var entity = new Payroll
        {
            UserId = dto.UserId,
            Period = dto.Period,
            BaseSalary = dto.BaseSalary,
            Bonuses = dto.Bonuses,
            Deductions = dto.Deductions,
            NetSalary = dto.NetSalary,
            FileUrl = fileUrl,
            IsViewedByEmployee = dto.IsViewedByEmployee
        };

        await _repository.AddAsync(entity, cancellationToken);
        return new Response<int>(entity.Id, "Payroll created successfully.");
    }
}

/// <summary>
/// Validator for the CreatePayrollCommand.
/// </summary>
public class CreatePayrollCommandValidator : AbstractValidator<CreatePayrollCommand>
{
    public CreatePayrollCommandValidator()
    {
        RuleFor(x => x.Payroll.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Payroll.Period)
            .NotEmpty().WithMessage("Period is required.")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("Period must be in the format YYYY-MM.");

        RuleFor(x => x.Payroll.BaseSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Base salary must be non-negative.");

        RuleFor(x => x.Payroll.Bonuses)
            .GreaterThanOrEqualTo(0).WithMessage("Bonuses must be non-negative.");

        RuleFor(x => x.Payroll.Deductions)
            .GreaterThanOrEqualTo(0).WithMessage("Deductions must be non-negative.");

        RuleFor(x => x.Payroll.NetSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Net salary must be non-negative.");

        // Optional: Validate file size and type
        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File!.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024) // Max 5MB
                .WithMessage("File must be less than or equal to 5MB.");

            RuleFor(x => x.File!.ContentType)
                .Equal("application/pdf")
                .WithMessage("Only PDF files are allowed.");
        });
    }
}
