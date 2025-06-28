
using Backend.Domain.Entities;

public class Certification : BaseAuditableEntity
{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public required string Authority { get; set; }
    public DateTime DateObtained { get; set; }
    public required string FileUrl { get; set; } // PDF contract path
}
