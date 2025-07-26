using System.Security.Claims;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.IRepositories;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Absences.Commands
{
    public record DeleteAbsenceCommand(int Id) : IRequest<Response<string>>;

    public class DeleteAbsenceCommandHandler : IRequestHandler<DeleteAbsenceCommand, Response<string>>
    {
        private readonly IAbsenceCommandRepository _commandRepository;
        private readonly IAbsenceQueryRepository _queryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteAbsenceCommandHandler(
            IAbsenceCommandRepository commandRepository,
            IAbsenceQueryRepository queryRepository,
            IHttpContextAccessor httpContextAccessor

            )
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<string>> Handle(DeleteAbsenceCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                var response = new Response<string>("Absence could not be deleted")
            .WithError("User is not authenticated.");
                return response;

            }
            var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                return new Response<string>("Absence not found");
            if (entity.UserId != userId)
                return new Response<string>("Failed to delete absence.");
            try
            {
                await _commandRepository.DeleteAsync(entity, cancellationToken);
                return new Response<string>(request.Id.ToString(), $"Absence with ID {request.Id} deleted successfully.");

            }
            catch (Exception ex)
            {
                return new Response<string>("Failed to delete absence.")
                    .WithError(ex.Message)
                    .WithCorrelationId(Guid.NewGuid().ToString())
                    .WithError("InternalServerError");
            }
        }
    }

    public class DeleteAbsenceCommandValidator : AbstractValidator<DeleteAbsenceCommand>
    {
        public DeleteAbsenceCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid absence ID.");
        }
    }
}
