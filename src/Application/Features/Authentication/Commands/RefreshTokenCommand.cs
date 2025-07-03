using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Authentication.Commands;

public class RefreshTokenCommand : IRequest<Response<LoginResultDto>>
{
    public string Token { get; set; } = string.Empty;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response<LoginResultDto>>
{
    private readonly IAuthenticationCommandRepository _authenticationCommandRepository;

    public RefreshTokenCommandHandler(IAuthenticationCommandRepository authenticationCommandRepository)
    {
        _authenticationCommandRepository = authenticationCommandRepository;
    }

    public async Task<Response<LoginResultDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationCommandRepository.RefreshTokenAsync(request.Token);
    }
}

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
