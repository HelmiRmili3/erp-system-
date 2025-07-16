namespace Backend.Application.Features.Admin.Dto
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class RoleWithPermissionsDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<PermissionDto> Permissions { get; set; } = new();
    }

    public class PermissionWithRolesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<RoleDto> Roles { get; set; } = new();
    }
}
