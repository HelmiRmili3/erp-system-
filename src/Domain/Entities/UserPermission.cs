namespace Backend.Domain.Entities;

public class UserPermission
{
    public required string UserId { get; set; }
    public Guid PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;
}
