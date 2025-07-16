
using Backend.Domain.Entities;
public class Permission
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();


}
