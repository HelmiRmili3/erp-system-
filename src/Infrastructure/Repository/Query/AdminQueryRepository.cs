using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Features.Admin.Repositories;

public class AdminQueryRepository : IAdminQueryRepository
{
    private readonly ApplicationDbContext _context;

    public AdminQueryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<List<PermissionDto>>> GetPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await _context.Permissions
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            })
            .ToListAsync(cancellationToken);

        return new Response<List<PermissionDto>>(permissions);
    }

    public async Task<Response<PermissionDto>> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _context.Permissions
            .Where(p => p.Id == id)
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (permission == null)
            return new Response<PermissionDto>("Permission not found");

        return new Response<PermissionDto>(permission);
    }

    public async Task<Response<List<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _context.ApplicationRoles
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name!
            })
            .ToListAsync(cancellationToken);

        return new Response<List<RoleDto>>(roles);
    }

    public async Task<Response<RoleDto>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
    {
        var role = await _context.ApplicationRoles
            .Where(r => r.Id == roleId)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name!
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (role == null)
            return new Response<RoleDto>("Role not found");

        return new Response<RoleDto>(role);
    }

    public async Task<Response<List<RoleWithPermissionsDto>>> GetRolesWithPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var rolesWithPermissions = await _context.ApplicationRoles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Select(r => new RoleWithPermissionsDto
            {
                Id = r.Id,
                Name = r.Name!,
                Permissions = r.RolePermissions.Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new Response<List<RoleWithPermissionsDto>>(rolesWithPermissions);
    }
}
