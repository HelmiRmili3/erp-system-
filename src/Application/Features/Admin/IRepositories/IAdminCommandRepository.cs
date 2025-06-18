using System.Threading.Tasks;
using Backend.Application.Common.Response;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Abstractions
{
    public interface IAdminCommandRepository
    {
        Task<Response<String>> CreateRoleAsync(string roleName);
        Task<Response<string>> AssignClaimToRoleAsync(string roleId, string claimType, string claimValue);
        Task<Response<string>> AssignRoleToUserAsync(string userId, string roleName);
        Task<Response<string>> DeleteRoleAsync(string roleName);
        Task<Response<string>> DeleteClaimAsync(string roleName, string claimType, string claimValue);
    }
}
