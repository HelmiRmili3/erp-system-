using Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }

    // Optional navigation to RolePermission (many-to-many)
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
