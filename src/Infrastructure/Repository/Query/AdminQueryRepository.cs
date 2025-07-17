using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Features.Admin.Repositories;

public class AdminQueryRepository : IAdminQueryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminQueryRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
    public async Task<Response<List<UserDto>>> GetUsersWithRolesAndPermissionsAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
            .Include(u => u.Supervisor)
            .ToListAsync(cancellationToken);

        var userIds = users.Select(u => u.Id).ToList();

        // Get user roles
        var userRoles = await _context.UserRoles
            .Where(ur => userIds.Contains(ur.UserId))
            .ToListAsync(cancellationToken);

        // Get all role IDs used by users
        var roleIds = userRoles.Select(ur => ur.RoleId).Distinct().ToList();

        // Load roles with their names
        var roles = await _context.ApplicationRoles
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        // Get role permissions (optional, you already have this)
        var rolePermissions = await _context.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Include(rp => rp.Permission)
            .ToListAsync(cancellationToken);

        var userDtos = users.Select(u =>
        {
            var rolesForUserIds = userRoles
                .Where(ur => ur.UserId == u.Id)
                .Select(ur => ur.RoleId)
                .ToList();

            // Map role IDs to role names
            var rolesForUserNames = roles
                .Where(r => rolesForUserIds.Contains(r.Id))
                .Select(r => r.Name!)
                .ToList();

            var rolePermissionsForUser = rolePermissions
                .Where(rp => rolesForUserIds.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Name)
                .ToList();

            var userPermissions = u.UserPermissions.Select(up => up.Permission.Name).ToList();

            var allPermissions = rolePermissionsForUser.Concat(userPermissions).Distinct().ToList();

            return new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = $"{u.FirstName} {u.LastName}",
                Email = u.Email!,
                BirthDate = u.BirthDate,
                Address = u.Address,
                JobTitle = u.JobTitle,
                Department = u.Department,
                HireDate = u.HireDate,
                ContractType = u.ContractType.ToString()!,
                Status = u.Status.ToString(),
                CreatedAt = u.CreatedAt,
                CreatedBy = u.CreatedBy ?? "",
                UpdatedAt = u.UpdatedAt,
                UpdatedBy = u.UpdatedBy ?? "",
                SupervisorId = u.SupervisorId ?? "",
                SupervisorFullName = u.Supervisor != null ? $"{u.Supervisor.FirstName} {u.Supervisor.LastName}" : "",
                Roles = rolesForUserNames,    // **HERE: role names instead of role IDs**
                Permissions = allPermissions
            };
        }).ToList();

        return new Response<List<UserDto>>(userDtos);
    }

}
