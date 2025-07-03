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
        // AttendanceDay is DateOnly, no .Date property
        var attendanceDay = DateOnly.FromDateTime(DateTime.Now);

        var existingAttendance = await _queryRepository.GetSingleByFilterAsync(
            a => a.UserId == request.UserId && a.AttendanceDay == attendanceDay,
            null,
            cancellationToken
        );

        if (existingAttendance is null)
        {
            // First time → Check-In
            var newAttendance = new Attendance
            {
                UserId = request.UserId,
                AttendanceDay = attendanceDay,
                CheckIn = TimeOnly.FromDateTime(DateTime.Now),
                CheckInMethod = request.CheckInMethod,
                CheckInLatitude = request.CheckInLatitude,
                CheckInLongitude = request.CheckInLongitude,
                CheckInDeviceId = request.CheckInDeviceId,
                CheckInIpAddress = request.CheckInIpAddress,
                IsCheckInByAdmin = request.IsCheckInByAdmin
            };

            var result = await _commandRepository.AddAsync(newAttendance, cancellationToken);

            if (result == null)
                throw new ApplicationException("Attendance could not be created.");

            return new Response<AttendanceDto>(result.ToDto<AttendanceDto>(), "Check-in successful.");
        }

        if (existingAttendance.CheckIn == null)
        {
            existingAttendance.CheckIn = TimeOnly.FromDateTime(DateTime.Now);
            existingAttendance.CheckInMethod = request.CheckInMethod;
            existingAttendance.CheckInLatitude = request.CheckInLatitude;
            existingAttendance.CheckInLongitude = request.CheckInLongitude;
            existingAttendance.CheckInDeviceId = request.CheckInDeviceId;
            existingAttendance.CheckInIpAddress = request.CheckInIpAddress;
            existingAttendance.IsCheckInByAdmin = request.IsCheckInByAdmin;

            await _commandRepository.UpdateAsync(existingAttendance, cancellationToken);

            return new Response<AttendanceDto>(existingAttendance.ToDto<AttendanceDto>(), "Check-in recorded.");
        }

        if (existingAttendance.CheckOut == null)
        {
            existingAttendance.CheckOut = TimeOnly.FromDateTime(DateTime.Now);
            existingAttendance.CheckOutMethod = request.CheckOutMethod;
            existingAttendance.CheckOutLatitude = request.CheckOutLatitude;
            existingAttendance.CheckOutLongitude = request.CheckOutLongitude;
            existingAttendance.CheckOutDeviceId = request.CheckOutDeviceId;
            existingAttendance.CheckOutIpAddress = request.CheckOutIpAddress;
            existingAttendance.IsCheckOutByAdmin = request.IsCheckOutByAdmin;

            await _commandRepository.UpdateAsync(existingAttendance, cancellationToken);

            return new Response<AttendanceDto>(existingAttendance.ToDto<AttendanceDto>(), "Check-out recorded.");
        }

        return new Response<AttendanceDto>(existingAttendance.ToDto<AttendanceDto>(), "User already checked out today.");
    }
}
