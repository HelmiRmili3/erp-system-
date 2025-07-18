using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Backend.Application.Common.Parameters;
using Backend.Application.Common.Models;

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
    public async Task<PagedResponse<List<PermissionDto>>> GetPermissionsWithPaginationAsync(
     int pageNumber,
     int pageSize,
     CancellationToken cancellationToken)
    {
        var source = _context.Permissions
            .OrderBy(p => p.Id)
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            });

        var totalRecords = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        var recordsCount = new RecordsCount
        {
            RecordsFiltered = totalRecords,
            RecordsTotal = totalRecords
        };
        return new PagedResponse<List<PermissionDto>>(
            items,
            pageNumber,
            pageSize,
            recordsCount
        );
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

    public async Task<PagedResponse<List<RoleDto>>> GetRolesAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ApplicationRoles
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name!
            });

        var totalRecords = await query.CountAsync(cancellationToken);

        var roles = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        var recordsCount = new RecordsCount
        {
            RecordsFiltered = totalRecords,
            RecordsTotal = totalRecords
        };
        return new PagedResponse<List<RoleDto>>(roles, pageNumber, pageSize, recordsCount);
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

    public async Task<PagedResponse<List<RoleWithPermissionsDto>>> GetRolesWithPermissionsAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var source = _context.ApplicationRoles
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
            });

        var totalRecords = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var recordsCount = new RecordsCount
        {
            RecordsFiltered = totalRecords,
            RecordsTotal = totalRecords
        };

        return new PagedResponse<List<RoleWithPermissionsDto>>(
            items,
            pageNumber,
            pageSize,
            recordsCount
        );
    }
    public async Task<PagedResponse<List<UserDto>>> GetUsersWithRolesAndPermissionsAsync(
    int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var totalRecords = await _context.Users.CountAsync(cancellationToken);

        var users = await _context.Users
            .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
            .Include(u => u.Supervisor)
            .OrderBy(u => u.LastName) 
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var userIds = users.Select(u => u.Id).ToList();

        var userRoles = await _context.UserRoles
            .Where(ur => userIds.Contains(ur.UserId))
            .ToListAsync(cancellationToken);

        var roleIds = userRoles.Select(ur => ur.RoleId).Distinct().ToList();

        var roles = await _context.ApplicationRoles
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

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
                Roles = rolesForUserNames,
                Permissions = allPermissions
            };
        }).ToList();

        // Use your RecordsCount type (replace with your actual implementation)
        var recordsCount = new RecordsCount
        {
            RecordsFiltered = totalRecords,
            RecordsTotal = totalRecords
        };

        return new PagedResponse<List<UserDto>>(userDtos, pageNumber, pageSize, recordsCount);
    }

   
}
