using Backend.Application.Common.Models;
namespace Backend.Application.Abstractions
{
    public interface IAdminCommandRepository
    {
        Task<Result> CreateRoleAsync(string roleName);
        Task<Result> AssignClaimToRoleAsync(string roleId, string claimType, string claimValue);
        Task<Result> AssignRoleToUserAsync(string userId, string roleName);
        Task<Result> DeleteRoleAsync(string roleName);
        Task<Result> DeleteClaimAsync(string roleName, string claimType, string claimValue);
    }
}
