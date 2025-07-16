using Backend.Application.Abstractions;
using Backend.Application.Common.Models;


namespace Backend.Application.Features.Admin.Commands
{
    public record DeletePermissionsFromRoleCommand(string Role, List<String> Permissions) : IRequest<Result>;

    public class DeletePermissionsFromRoleCommandHandler : IRequestHandler<DeletePermissionsFromRoleCommand, Result>
    {
        private readonly IAdminCommandRepository _adminRepository;

        public DeletePermissionsFromRoleCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Result> Handle(DeletePermissionsFromRoleCommand request, CancellationToken cancellationToken)
        {
            return await _adminRepository.DeletePermissionsFromRoleAsync(request.Role, request.Permissions);
        }
    }

    public class DeletePermissionsFromRoleCommandValidator : AbstractValidator<DeletePermissionsFromRoleCommand>
    {
        public DeletePermissionsFromRoleCommandValidator()
        {
            RuleFor(v => v.Role)
                .NotEmpty()
                .WithMessage("Role name is required.");

            RuleFor(v => v.Permissions)
                .NotNull()
                .WithMessage("Permission IDs list is required.")
                .Must(list => list.Count > 0)
                .WithMessage("At least one permission must be specified.");
        }
    }
}
