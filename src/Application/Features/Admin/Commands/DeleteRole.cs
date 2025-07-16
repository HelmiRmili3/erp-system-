using Backend.Application.Abstractions;
using Backend.Application.Common.Models;
using Backend.Application.Common.Response;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Features.Admin.Commands
{
    public record DeleteRoleCommand(string Role) : IRequest<Result>;

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly IAdminCommandRepository _adminRepository;

        public DeleteRoleCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            return await _adminRepository.DeleteRoleAsync(request.Role);
        
        }
    }

    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(v => v.Role)
                .NotEmpty()
                .WithMessage("Role name is required");
        }
    }
}
