using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;


namespace Backend.Application.Features.Absences.Commands 
{

public record CreateAbsenceCommand : AbsenceAddDto, IRequest<Response<AbsenceDto>>;

public class CreateAbsenceCommandHandler : IRequestHandler<CreateAbsenceCommand, Response<AbsenceDto>>
{
    private readonly IAbsenceCommandRepository _repository;

    public CreateAbsenceCommandHandler(IAbsenceCommandRepository repository)
    {
        _repository = repository ;
    }

    public async Task<Response<AbsenceDto>> Handle(CreateAbsenceCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Null(request.StartDate, nameof(request.StartDate));
        Guard.Against.Null(request.EndDate, nameof(request.EndDate));
        Guard.Against.Null(request.AbsenceType, nameof(request.AbsenceType));
        Guard.Against.Null(request.StatusType, nameof(request.StatusType));

        var entity = new Absence
        {
            UserId = request.UserId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AbsenceType = request.AbsenceType,
            StatusType = request.StatusType,
            Reason = request.Reason
        };

        var result = await _repository.AddAsync(entity, cancellationToken);

        if (result == null)
        {
            throw new ApplicationException("Absence could not be created");
        }

        return new Response<AbsenceDto>(result.ToDto<AbsenceDto>(), "Absence created successfully");
    }
}

public class CreateAbsenceCommandValidator : AbstractValidator<CreateAbsenceCommand>
{
    public CreateAbsenceCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

        RuleFor(v => v.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");

        RuleFor(v => v.AbsenceType)
            .IsInEnum().WithMessage("Invalid absence type.");

        RuleFor(v => v.StatusType)
            .IsInEnum().WithMessage("Invalid status type.");

        RuleFor(v => v.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Reason));
    }
}
}
