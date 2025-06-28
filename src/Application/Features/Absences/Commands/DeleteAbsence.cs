using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.IRepositories;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Absences.Commands
{
    public record DeleteAbsenceCommand(int Id) : IRequest<Response<string>>;

    public class DeleteAbsenceCommandHandler : IRequestHandler<DeleteAbsenceCommand, Response<string>>
    {
        private readonly IAbsenceCommandRepository _commandRepository;
        private readonly IAbsenceQueryRepository _queryRepository;

        public DeleteAbsenceCommandHandler(
            IAbsenceCommandRepository commandRepository,
            IAbsenceQueryRepository queryRepository)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
        }

        public async Task<Response<string>> Handle(DeleteAbsenceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                return new Response<string>("Absence not found");

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
