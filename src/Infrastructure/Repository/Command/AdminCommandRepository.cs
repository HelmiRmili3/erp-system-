using System.Reflection;
using System.Security.Claims;
using Backend.Application.Abstractions;
using Backend.Application.Common.Models;
using Backend.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Repository.Command;

public class AdminCommandRepository : IAdminCommandRepository
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminCommandRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<Result> CreateRoleAsync(string roleName)
    {
        bool RoleExists(string type)
        {
            var roleConstants = typeof(Roles).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return roleConstants.Any(field => field.GetValue(null)?.ToString() == type);
        }

        if (!RoleExists(roleName))
            return Result.Failure(new[] { "Role type does not exist" });

        if (await _roleManager.RoleExistsAsync(roleName))
            return Result.Failure(new[] { "Role already exists" });

        var role = new ApplicationRole(roleName);
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return Result.Failure(result.Errors.Select(e => e.Description));

        return Result.Success();
    }

    public async Task<Result> AssignClaimToRoleAsync(string roleName, string claimType, string claimValue)
    {
        bool ClaimExists(string type)
        {
            var claimConstants = typeof(Claims).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return claimConstants.Any(field => field.GetValue(null)?.ToString() == type);
        }

        if (!ClaimExists(claimType))
            return Result.Failure(new[] { "Claim type does not exist" });

        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return Result.Failure(new[] { "Role not found" });

        var roleClaims = await _roleManager.GetClaimsAsync(role);
        if (roleClaims.Any(rc => rc.Type == claimType && rc.Value == claimValue))
            return Result.Failure(new[] { "Claim already assigned to role" });

        var result = await _roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));
        if (!result.Succeeded)
            return Result.Failure(result.Errors.Select(e => e.Description));

        return Result.Success();
    }

    public async Task<Result> AssignRoleToUserAsync(string userId, string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return Result.Failure(new[] { "Role not found" });

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure(new[] { "User not found" });

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
            return Result.Failure(result.Errors.Select(e => e.Description));

        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
            return Result.Failure(new[] { "Role not found" });

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            return Result.Failure(result.Errors.Select(e => e.Description));

        return Result.Success();
    }

    public async Task<Result> DeleteClaimAsync(string roleId, string claimType, string claimValue)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
            return Result.Failure(new[] { "Role not found" });

        var claims = await _roleManager.GetClaimsAsync(role);
        var roleClaim = claims.FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);

        if (roleClaim == null)
            return Result.Failure(new[] { "Claim not found in role" });

        var result = await _roleManager.RemoveClaimAsync(role, roleClaim);
        if (!result.Succeeded)
            return Result.Failure(result.Errors.Select(e => e.Description));

        return Result.Success();
    }
}
