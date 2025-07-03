using Backend.Domain.Entities;

public class Attendance : BaseAuditableEntity
{
    public required string UserId { get; set; }

    public required DateOnly AttendanceDay { get; set; }

    // Check-in details
    public TimeOnly? CheckIn { get; set; }
    public  CheckMethod? CheckInMethod { get; set; }
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
