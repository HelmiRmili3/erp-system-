public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty; // Or nullable
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // e.g., Insert, Update, Delete
    public string? KeyValues { get; set; } // e.g., {"Id": 1}
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
