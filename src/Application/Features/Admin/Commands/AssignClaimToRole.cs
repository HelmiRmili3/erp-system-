using Backend.Application.Abstractions;
using Backend.Application.Common.Models;

namespace Backend.Application.Features.Admin.Commands
{
    public record AssignClaimToRoleCommand(string roleName, string ClaimType, string ClaimValue) : IRequest<Result>;

    public class AssignClaimToRoleCommandHandler : IRequestHandler<AssignClaimToRoleCommand, Result>
    {
        private readonly IAdminCommandRepository _adminRepository;

        public AssignClaimToRoleCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

       

        public async Task<Result> Handle(AssignClaimToRoleCommand request, CancellationToken cancellationToken)
        {

         return   await _adminRepository.AssignClaimToRoleAsync(request.roleName, request.ClaimType,request.ClaimValue);

        }
    }

    public class AssignClaimToRoleCommandValidator : AbstractValidator<AssignClaimToRoleCommand>
    {
        public AssignClaimToRoleCommandValidator()
        {
            RuleFor(v => v.roleName)
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
