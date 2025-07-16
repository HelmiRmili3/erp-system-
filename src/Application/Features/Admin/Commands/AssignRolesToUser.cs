using Backend.Application.Abstractions;
using Backend.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Admin.Commands
{
    public record AssignRolesToUserCommand(string UserId, List<string> RoleNames) : IRequest<Result>;

    public class AssignRolesToUserCommandHandler : IRequestHandler<AssignRolesToUserCommand, Result>
    {
        private readonly IAdminCommandRepository _adminRepository;

        public AssignRolesToUserCommandHandler(IAdminCommandRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Result> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
        {
            return await _adminRepository.AssignRolesToUserAsync(request.UserId, request.RoleNames);
        }
    }

    public class AssignRolesToUserCommandValidator : AbstractValidator<AssignRolesToUserCommand>
    {
        public AssignRolesToUserCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(v => v.RoleNames)
                .NotNull()
                .WithMessage("Roles list is required.")
                .Must(list => list.Count > 0)
                .WithMessage("At least one role must be provided.");
        }
    }
}
