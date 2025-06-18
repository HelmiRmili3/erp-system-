using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;
namespace Backend.Application.Features.Authentication.Commands;
// Command to register a new user
public record RegisterUserCommand(string UserName, string Email, string Password) : IRequest<Response<RegisterResultDto>>;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<RegisterResultDto>>
{
    private readonly IAuthenticationCommandRepository _authenticationCommandRepository;
    public RegisterUserCommandHandler(IAuthenticationCommandRepository authenticationCommandRepository)
    {
        _authenticationCommandRepository = authenticationCommandRepository;
    }
    public async Task<Response<RegisterResultDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerDto = new RegisterDto
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password

        };
        var user = await _authenticationCommandRepository.RegisterAsync(registerDto);
       return user;
    }
}
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
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
    }
}
