using Backend.Application.Features.User.Dto;
using Backend.Domain.Enums;

namespace Backend.Application.Features.Attendances.Dto
{
    public class AttendanceDto : BaseAttendanceDto
    {
        public UserDataDto? User { get; set; }

    }

    public class BaseAttendanceDto
    {
        public int Id { get; init; }
        public string UserId { get; set; } = string.Empty;

        public DateOnly AttendanceDay { get; set; }

        // Check-in details
        public TimeOnly? CheckIn { get; set; }
        public CheckMethod? CheckInMethod { get; set; }
        public double? CheckInLatitude { get; set; }
        public double? CheckInLongitude { get; set; }
        public string? CheckInDeviceId { get; set; }
        public string? CheckInIpAddress { get; set; }
        public bool IsCheckInByAdmin { get; set; }

        // Check-out details
        public TimeOnly? CheckOut { get; set; }
        public CheckMethod? CheckOutMethod { get; set; } 
        public double? CheckOutLatitude { get; set; }
        public double? CheckOutLongitude { get; set; }
        public string? CheckOutDeviceId { get; set; }
        public string? CheckOutIpAddress { get; set; }
        public bool IsCheckOutByAdmin { get; set; }
    }

    public record AttendanceAddDto
    {
        public required string UserId { get; set; }
        public DateOnly AttendanceDay { get; init; }
        public TimeOnly? CheckIn { get; init; }
        public CheckMethod? CheckInMethod { get; init; }
        public double? CheckInLatitude { get; init; }
        public double? CheckInLongitude { get; init; }
        public string? CheckInDeviceId { get; init; }
        public string? CheckInIpAddress { get; init; }
        public bool IsCheckInByAdmin { get; init; }

        public TimeOnly? CheckOut { get; init; }
        public CheckMethod? CheckOutMethod { get; init; }
        public double? CheckOutLatitude { get; init; }
        public double? CheckOutLongitude { get; init; }
        public string? CheckOutDeviceId { get; init; }
        public string? CheckOutIpAddress { get; init; }
        public bool IsCheckOutByAdmin { get; init; }
    }

    public record AttendanceUpdateDto : AttendanceAddDto
    {
        public int Id { get; init; }
    }
}
