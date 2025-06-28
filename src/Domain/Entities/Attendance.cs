using Backend.Domain.Entities;

public class Attendance : BaseAuditableEntity
{
    public required string UserId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public string CheckInMethod { get; set; } = null!;
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public string? IpAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? DeviceId { get; set; }
    public string? Notes { get; set; }
}
