using Backend.Application.Abstractions;
using Backend.Application.Common.Response;
using Backend.Domain.Constants;

using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;


namespace Backend.Application.Features.Admin.Commands
{
    public record AssignClaimToRoleCommand(string RoleId, string ClaimType, string ClaimValue) : IRequest<Response<string>>;

    public class AssignClaimToRoleCommandHandler : IRequestHandler<AssignClaimToRoleCommand, Response<string>>
    {
        private readonly IAdminCommandRepository _adminRepository;

        public AssignClaimToRoleCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

       

        public async Task<Response<string>> Handle(AssignClaimToRoleCommand request, CancellationToken cancellationToken)
        {

         return   await _adminRepository.AssignClaimToRoleAsync(request.RoleId,request.ClaimType,request.ClaimValue);

        }
    }

    public class AssignClaimToRoleCommandValidator : AbstractValidator<AssignClaimToRoleCommand>
    {
        public AssignClaimToRoleCommandValidator()
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
