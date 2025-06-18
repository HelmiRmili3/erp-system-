using Backend.Application.Abstractions;
using Backend.Application.Common.Response;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Features.Admin.Commands;
public record CreateRoleCommand(string Name) : IRequest<Response<string>>;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<string>>
{
    private readonly IAdminCommandRepository _adminRepository;

    public CreateRoleCommandHandler(IAdminCommandRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<Response<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await _adminRepository.CreateRoleAsync(request.Name);
    }
}

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(256)
            .NotEmpty()
            .WithMessage("Role name is required and must not exceed 256 characters");
    }
}
