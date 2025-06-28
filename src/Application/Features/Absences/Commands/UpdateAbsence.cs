using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Backend.Application.Features.Absences.Commands
{
    public record UpdateAbsenceCommand : AbsenceUpdateDto, IRequest<Response<AbsenceDto>>;

    public class UpdateAbsenceCommandHandler : IRequestHandler<UpdateAbsenceCommand, Response<AbsenceDto>>
    {
        private readonly IAbsenceCommandRepository _commandRepository;
        private readonly IAbsenceQueryRepository _queryRepository;

        public UpdateAbsenceCommandHandler(
            IAbsenceCommandRepository commandRepository,
            IAbsenceQueryRepository queryRepository)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
        }

        public async Task<Response<AbsenceDto>> Handle(UpdateAbsenceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                return new Response<AbsenceDto>("Absence not found");

            entity.UserId = request.UserId;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.AbsenceType = request.AbsenceType;
            entity.StatusType = request.StatusType;
            entity.Reason = request.Reason;

            await _commandRepository.UpdateAsync(entity, cancellationToken);

            return new Response<AbsenceDto>(entity.ToDto<AbsenceDto>(), "Absence updated successfully");
        }
    }

    public class UpdateAbsenceCommandValidator : AbstractValidator<UpdateAbsenceCommand>
    {
        public UpdateAbsenceCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid absence ID.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");

            RuleFor(x => x.AbsenceType)
                .IsInEnum().WithMessage("Invalid absence type.");

            RuleFor(x => x.StatusType)
                .IsInEnum().WithMessage("Invalid status type.");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Reason));
        }
    }
}
