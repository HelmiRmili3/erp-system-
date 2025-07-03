using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Authentication.Commands;

public record ChangePasswordCommand(ChangePasswordDataDto Data) : IRequest<Response<string>>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Response<string>>
{
    private readonly IAuthenticationCommandRepository _authenticationCommandRepository;

    public ChangePasswordCommandHandler(IAuthenticationCommandRepository authenticationCommandRepository)
    {
        _authenticationCommandRepository = authenticationCommandRepository;
    }

    public async Task<Response<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationCommandRepository.ChangePasswordAsync(request.Data);
    }
}

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Data.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");

        RuleFor(x => x.Data.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters.")
            .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("New password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character.");

        RuleFor(x => x.Data.ConfirmNewPassword)
            .Equal(x => x.Data.NewPassword).WithMessage("Passwords do not match.");
    }
}
