using AutoMapper;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Authentication.Commands;

public record RegisterUserCommand(RegisterUserRequest Data, IFormFile? File)
    : IRequest<Response<RegisterResultDto>>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<RegisterResultDto>>
{
    private readonly IAuthenticationCommandRepository _authenticationCommandRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(
        IAuthenticationCommandRepository authenticationCommandRepository,
        IFileService fileService,
        IMapper mapper)
    {
        _authenticationCommandRepository = authenticationCommandRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<Response<RegisterResultDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

        string? fileUrl = null;
        if (request.File is { Length: > 0 })
        {
            var fileName = $"{Guid.NewGuid()}";
            var relativePath = await _fileService.SaveFileAsync(request.File, fileName, "users");
            fileUrl = $"/files/{relativePath.Replace("\\", "/")}";
        }

        var dto = _mapper.Map<RegisterDto>(request.Data);
        dto.FileUrl = fileUrl!;
        return await _authenticationCommandRepository.RegisterAsync(dto);
    }
}

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now).WithMessage("Birth date must be in the past.")
            .When(x => x.BirthDate.HasValue);

        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("Job title is required.");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required.");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Hire date must be in the past or today.")
            .When(x => x.HireDate.HasValue);
    }
}
