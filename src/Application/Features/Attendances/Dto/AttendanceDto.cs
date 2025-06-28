namespace Backend.Application.Features.Attendances.Dto
{
    public class AttendanceDto : BaseAttendanceDto
    {
        // Extend here with additional relations if needed
    }

    public class BaseAttendanceDto
    {
        public int Id { get; init; }
        public string UserId { get; set; } = string.Empty;
        public DateTime AttendanceDate { get; set; }
        public string CheckInMethod { get; set; } = string.Empty;
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? IpAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? DeviceId { get; set; }
        public string? Notes { get; set; }
    }

    public record AttendanceAddDto
    {
        public required string UserId { get; set; }
        public DateTime AttendanceDate { get; init; }
        public string CheckInMethod { get; init; } = string.Empty;
        public DateTime? CheckIn { get; init; }
        public DateTime? CheckOut { get; init; }
        public string? IpAddress { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
        public string? DeviceId { get; init; }
        public string? Notes { get; init; }
    }

    public record AttendanceUpdateDto : AttendanceAddDto
    {
        public int Id { get; init; }
    }
}
