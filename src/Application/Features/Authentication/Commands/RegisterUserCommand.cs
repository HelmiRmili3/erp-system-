using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;
namespace Backend.Application.Features.Authentication.Commands;
public record RegisterUserCommand(RegisterDto data) : IRequest<Response<RegisterResultDto>>;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<RegisterResultDto>>
{
    private readonly IAuthenticationCommandRepository _authenticationCommandRepository;
    public RegisterUserCommandHandler(IAuthenticationCommandRepository authenticationCommandRepository)
    {
        _authenticationCommandRepository = authenticationCommandRepository;
    }
    public async Task<Response<RegisterResultDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationCommandRepository.RegisterAsync(request.data);
    }
}
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.data.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

        RuleFor(x => x.data.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.data.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.data.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.data.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.data.BirthDate)
            .LessThan(DateTime.Now).WithMessage("Birth date must be in the past.")
            .When(x => x.data.BirthDate.HasValue);

        RuleFor(x => x.data.JobTitle)
            .NotEmpty().WithMessage("Job title is required.");

        RuleFor(x => x.data.Department)
            .NotEmpty().WithMessage("Department is required.");

        RuleFor(x => x.data.HireDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Hire date must be in the past or today.")
            .When(x => x.data.HireDate.HasValue);
    }
}

