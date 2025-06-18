using System.Reflection;
using System.Security.Claims;
using Backend.Application.Abstractions;
using Backend.Application.Common.Response;
using Backend.Domain.Constants;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Repository.Command
{
    public class AdminCommandRepository : IAdminCommandRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminCommandRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Response<string>> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ApplicationException("Role already exists");
            }

            var role = new ApplicationRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new ApplicationException("Role could not be created");
            }

            return new Response<String>(roleName, "Role created successfully");
        }

        public async Task<Response<string>> AssignClaimToRoleAsync(string roleName, string claimType, string claimValue)
        {
            bool ClaimExists(string type)
            {
                var claimConstants = typeof(Claims).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
                return claimConstants.Any(field => field.GetValue(null)?.ToString() == type);
            }

            if (!ClaimExists(claimType))
                throw new ApplicationException("Claim type does not exist");

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new ApplicationException("Role not found");

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            if (roleClaims.Any(rc => rc.Type == claimType && rc.Value == claimValue))
                throw new ApplicationException("Claim already assigned to role");

            var result = await _roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));
            if (!result.Succeeded)
                throw new ApplicationException("Failed to assign claim to role");

            return new Response<string>(roleName, "Claim assigned to role successfully");
        }

        public async Task<Response<string>> AssignRoleToUserAsync(string userId, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new ApplicationException("Role not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApplicationException("User not found");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new ApplicationException("Role could not be assigned to user");

            return new Response<string>(userId, "Role assigned to user successfully");
        }

        public async Task<Response<string>> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new ApplicationException("Role not found");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new ApplicationException("Role could not be deleted");

            return new Response<string>(roleId, "Role deleted successfully");
        }

        public async Task<Response<string>> DeleteClaimAsync(string roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new ApplicationException("Role not found");

            var claims = await _roleManager.GetClaimsAsync(role);
            var roleClaim = claims.FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);

            if (roleClaim == null)
                throw new ApplicationException("Claim not found in role");

            var result = await _roleManager.RemoveClaimAsync(role, roleClaim);
            if (!result.Succeeded)
                throw new ApplicationException("Claim could not be removed from role");

            return new Response<string>(roleId, "Claim removed from role successfully");
        }
    }
}
