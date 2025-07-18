﻿
using Backend.Application.Common.Models;
using Backend.Application.Abstractions;
using Backend.Infrastructure.Data;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AdminCommandRepository : IAdminCommandRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AdminCommandRepository(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> CreateRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return Result.Failure(new[] { "Role already exists" });

        var role = new ApplicationRole(roleName);
        var result = await _roleManager.CreateAsync(role);
        return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task<Result> AssignPermissionsToRoleAsync(string roleName, IEnumerable<string> permissionNames)
    {
        var role = await _roleManager.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == roleName);

        if (role == null)
            return Result.Failure(new[] { "Role not found." });

        // Get permissions from DB that match permissionNames
        var permissions = await _context.Permissions
            .Where(p => permissionNames.Contains(p.Name))
            .ToListAsync();

        if (permissions.Count != permissionNames.Count())
            return Result.Failure(new[] { "One or more permissions do not exist." });

        // Add new permissions to the role if not already assigned
        foreach (var permission in permissions)
        {
            if (!role.RolePermissions.Any(rp => rp.PermissionId == permission.Id))
            {
                role.RolePermissions.Add(item: new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id
                });
            }
        }

        await _context.SaveChangesAsync();
        return Result.Success();
    }


    public async Task<Result> AssignRolesToUserAsync(string userId, IEnumerable<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure(new[] { "User with ID '{userId}' not found." });

        var errors = new List<string>();

        foreach (var role in roles)
        {
            if (string.IsNullOrEmpty(role))
            {
                errors.Add("Role name is null or empty.");
                continue;
            }

            bool isInRole = await _userManager.IsInRoleAsync(user, role);
            if (!isInRole)
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors.Select(e => e.Description));
                }
            }
        }

        if (errors.Any())
            return Result.Failure(errors);

        return Result.Success();
    }




    public async Task<Result> AssignPermissionsToUserAsync(string userId, IEnumerable<string> permissionNames)
    {
        var user = await _userManager.Users.Include(u => u.UserPermissions).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return Result.Failure(new[] { "User not found." });

        var permissions = await _context.Permissions
            .Where(p => permissionNames.Contains(p.Name))
            .ToListAsync();

        if (permissions.Count != permissionNames.Count())
            return Result.Failure(new[] { "One or more permissions do not exist." });

        // Assuming UserPermissions is a navigation collection of user-permission links
        foreach (var permission in permissions)
        {
            if (!user.UserPermissions.Any(up => up.PermissionId == permission.Id))
            {
                user.UserPermissions.Add(new UserPermission
                {
                    UserId = user.Id,
                    PermissionId = permission.Id
                });
            }
        }

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return Result.Failure(new[] { "Role not found." });

        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task<Result> DeletePermissionsFromRoleAsync(string roleName, IEnumerable<string> permissionNames)
    {
        var role = await _roleManager.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Name == roleName);

        if (role == null) return Result.Failure(new[] { "Role not found." });

        var permissionsToRemove = role.RolePermissions
            .Where(rp => permissionNames.Contains(rp.Permission.Name))
            .ToList();

        if (!permissionsToRemove.Any())
            return Result.Failure(new[] { "No matching permissions found on the role." });

        _context.RemoveRange(permissionsToRemove);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
