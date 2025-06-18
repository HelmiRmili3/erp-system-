using Backend.Application.Abstractions;
using Backend.Application.Common.Response;

namespace Backend.Application.Features.Admin.Commands
{
    public record DeleteRoleClaimCommand(string RoleId, string ClaimType, string ClaimValue) : IRequest<Response<string>>;

    public class DeleteRoleClaimCommandHandler : IRequestHandler<DeleteRoleClaimCommand, Response<string>>
    {
        private readonly IAdminCommandRepository _adminRepository;

        public DeleteRoleClaimCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Response<string>> Handle(DeleteRoleClaimCommand request, CancellationToken cancellationToken)
        {
            // Find the role by ID
            return await _adminRepository.DeleteClaimAsync(request.RoleId,request.ClaimType, request.ClaimValue);
          
        }
    }

    public class DeleteRoleClaimCommandValidator : AbstractValidator<DeleteRoleClaimCommand>
    {
        public DeleteRoleClaimCommandValidator()
        {
            RuleFor(v => v.RoleId)
                .NotEmpty()
                .WithMessage("Role ID is required");

            RuleFor(v => v.ClaimType)
                .NotEmpty()
                .WithMessage("Claim type is required");

            RuleFor(v => v.ClaimValue)
                .NotEmpty()
                .WithMessage("Claim value is required");
        }
    }
}
