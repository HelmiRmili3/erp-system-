using System.Security.Claims;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Response;
using Backend.Application.Features.Attendances.Dto;
using Microsoft.AspNetCore.Http;

public record MarkAttendanceCommand : AttendanceAddDto, IRequest<Response<AttendanceDto>>;

public class MarkAttendanceCommandHandler : IRequestHandler<MarkAttendanceCommand, Response<AttendanceDto>>
{
    private readonly IAttendanceCommandRepository _commandRepository;
    private readonly IAttendanceQueryRepository _queryRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MarkAttendanceCommandHandler(
        IAttendanceCommandRepository commandRepository,
        IAttendanceQueryRepository queryRepository,
        IHttpContextAccessor httpContextAccessor
)


    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _httpContextAccessor = httpContextAccessor;

    }

    public async Task<Response<AttendanceDto>> Handle(MarkAttendanceCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return new Response<AttendanceDto>("Attendance could not be created").WithError("User is not authenticated.");
        }
        Guard.Against.NullOrEmpty(request.UserId, nameof(request.UserId));


        var existingAttendance = await _queryRepository.GetSingleByFilterAsync(
            a => a.UserId == request.UserId && a.AttendanceDay == request.AttendanceDay,
            null,
            cancellationToken
        );

        if (existingAttendance is null)
        {
            // First time → Check-In
            var newAttendance = new Attendance
            {
                UserId = userId,
                AttendanceDay = request.AttendanceDay,
                CheckIn = request.CheckIn,
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
            existingAttendance.CheckIn = request.CheckIn;
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
            existingAttendance.CheckOut = request.CheckOut;
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
