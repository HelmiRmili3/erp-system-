using System.Security.Claims;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Absences.Dto;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;


namespace Backend.Application.Features.Absences.Commands 
{

public record CreateAbsenceCommand : AbsenceAddDto, IRequest<Response<AbsenceDto>>;

public class CreateAbsenceCommandHandler : IRequestHandler<CreateAbsenceCommand, Response<AbsenceDto>>
{
    private readonly IAbsenceCommandRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateAbsenceCommandHandler(
        IAbsenceCommandRepository repository,
        IHttpContextAccessor httpContextAccessor
)
        {
        _repository = repository ;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<AbsenceDto>> Handle(CreateAbsenceCommand request, CancellationToken cancellationToken)
    {

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return new Response<AbsenceDto>("Absence could not be created").WithError("User is not authenticated.");
            }

        Guard.Against.Null(request.StartDate, nameof(request.StartDate));
        Guard.Against.Null(request.EndDate, nameof(request.EndDate));
        Guard.Against.Null(request.AbsenceType, nameof(request.AbsenceType));
        Guard.Against.Null(request.Reason, nameof(request.Reason));

        var entity = new Absence
        {
            UserId = userId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AbsenceType = request.AbsenceType,
            StatusType = AbsenceStatus.Pending,
            Reason = request.Reason
        };

        var result = await _repository.AddAsync(entity, cancellationToken);

        if (result == null || result.UserId != userId)
                return new Response<AbsenceDto>("Absence could not be created");
        return new Response<AbsenceDto>(result.ToDto<AbsenceDto>(), "Absence created successfully");
    }
}

public class CreateAbsenceCommandValidator : AbstractValidator<CreateAbsenceCommand>
{
    public CreateAbsenceCommandValidator()
    {
        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

        RuleFor(v => v.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");

        RuleFor(v => v.AbsenceType)
            .IsInEnum().WithMessage("Invalid absence type.");

        RuleFor(v => v.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Reason));
    }
}
}
