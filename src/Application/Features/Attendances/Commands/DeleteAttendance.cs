using Backend.Application.Common.Response;

namespace Backend.Application.Features.Attendances.Commands;

/// <summary>
/// Command to delete an attendance record by its ID.
/// </summary>
/// <param name="Id">The ID of the attendance to delete.</param>
public record DeleteAttendanceCommand(int Id) : IRequest<Response<string>>;
/// <summary>
/// Handles deletion of an attendance record.
/// </summary>
public class DeleteAttendanceCommandHandler : IRequestHandler<DeleteAttendanceCommand, Response<string>>
{
    private readonly IAttendanceCommandRepository _commandRepository;
    private readonly IAttendanceQueryRepository _queryRepository;

    public DeleteAttendanceCommandHandler(
        IAttendanceCommandRepository commandRepository,
        IAttendanceQueryRepository queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<string>> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
    {
        var attendance = await _queryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (attendance == null)
        {
            return new Response<string>("Attendance record not found.");
        }

        await _commandRepository.DeleteAsync(attendance, cancellationToken);

        return new Response<string>("Attendance deleted successfully.");
    }

public class DeleteAttendanceCommandValidator : AbstractValidator<DeleteAttendanceCommand>
{
    public DeleteAttendanceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID must be a positive number.");
    }
}

}
