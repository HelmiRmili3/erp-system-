using Backend.Application.Abstractions;
using Backend.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Admin.Commands
{
    public record AssignPermissionsToRoleCommand(string Role, List<String> Permissions) : IRequest<Result>;

    public class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand, Result>
    {
        private readonly IAdminCommandRepository _adminRepository;

        public AssignPermissionsToRoleCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Result> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
        {
            return await _adminRepository.AssignPermissionsToRoleAsync(request.Role, request.Permissions);
        }
    }

    public class AssignPermissionsToRoleCommandValidator : AbstractValidator<AssignPermissionsToRoleCommand>
    {
        public AssignPermissionsToRoleCommandValidator()
        {
            RuleFor(v => v.Role)
                .NotEmpty()
                .WithMessage("Role name is required.");

            RuleFor(v => v.Permissions)
                .NotNull()
                .WithMessage("Permissions list is required.")
                .Must(p => p.Count > 0)
                .WithMessage("At least one permission must be provided.");
        }
    }
}
