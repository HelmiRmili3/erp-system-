using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Attendances.Dto;

public record MarkAttendanceCommand : AttendanceAddDto, IRequest<Response<AttendanceDto>>;

public class MarkAttendanceCommandHandler : IRequestHandler<MarkAttendanceCommand, Response<AttendanceDto>>
{
    private readonly IAttendanceCommandRepository _commandRepository;
    private readonly IAttendanceQueryRepository _queryRepository;

    public MarkAttendanceCommandHandler(
        IAttendanceCommandRepository commandRepository,
        IAttendanceQueryRepository queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<Response<AttendanceDto>> Handle(MarkAttendanceCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));
        Guard.Against.Null(request.AttendanceDate, nameof(request.AttendanceDate));

        var today = request.AttendanceDate.Date;

        var existingAttendance = await _queryRepository.GetSingleByFilterAsync(
            a => a.UserId == request.UserId && a.AttendanceDate.Date == today,
            null,
            cancellationToken
        );

        if (existingAttendance is null)
        {
            // First time → Check-In
            var newAttendance = new Attendance
            {
                UserId = request.UserId,
                AttendanceDate = today,
                CheckIn = request.CheckIn,
                CheckInMethod = request.CheckInMethod,
                IpAddress = request.IpAddress,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                DeviceId = request.DeviceId,
                Notes = request.Notes
            };

            var result = await _commandRepository.AddAsync(newAttendance, cancellationToken);

            if (result == null)
                throw new ApplicationException("Attendance could not be created.");

            return new Response<AttendanceDto>(result.ToDto<AttendanceDto>(), "Check-in successful.");
        }

        if (existingAttendance.CheckIn == null)
        {
            existingAttendance.CheckIn = request.CheckIn;
            existingAttendance.CheckInMethod = request.CheckInMethod;

            await _commandRepository.UpdateAsync(existingAttendance, cancellationToken);

            return new Response<AttendanceDto>(existingAttendance.ToDto<AttendanceDto>(), "Check-in recorded.");
        }

        if (existingAttendance.CheckOut == null)
        {
            existingAttendance.CheckOut = request.CheckOut;

            await _commandRepository.UpdateAsync(existingAttendance, cancellationToken);

            return new Response<AttendanceDto>(existingAttendance.ToDto<AttendanceDto>(), "Check-out recorded.");
        }

        return new Response<AttendanceDto>(existingAttendance.ToDto<AttendanceDto>(), "User already checked out today.");
    }
}
