using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;

namespace Backend.Application.Features.Authentication.Commands;

public class LoginUserCommand : IRequest<Response<LoginResultDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<LoginResultDto>>
{
    private readonly IAuthenticationCommandRepository _authenticationCommandRepository;

    public LoginUserCommandHandler(IAuthenticationCommandRepository authenticationCommandRepository)
    {
        _authenticationCommandRepository = authenticationCommandRepository;
    }

    public async Task<Response<LoginResultDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationCommandRepository.LoginAsync(request.Email, request.Password);
    }
}
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }
}

