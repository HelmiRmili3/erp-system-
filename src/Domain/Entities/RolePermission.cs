﻿namespace Backend.Domain.Entities;

public class RolePermission
{
    public required string RoleId { get; set; }
    public Guid PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;
}
