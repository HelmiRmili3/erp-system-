using Backend.Application.Abstractions;
using Backend.Application.Common.Response;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Features.Admin.Commands
{
    public record AssignRoleToUserCommand(string UserId, string RoleName) : IRequest<Response<string>>;

    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Response<string>>
    {
        private readonly IAdminCommandRepository _adminRepository;


        public AssignRoleToUserCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Response<string>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
           return await _adminRepository.AssignRoleToUserAsync(request.UserId, request.RoleName);
        }
    }

    public class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
    {
        public AssignRoleToUserCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");

            RuleFor(v => v.RoleName)
                .NotEmpty()
                .WithMessage("Role name is required");
        }
    }
}
