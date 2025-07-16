//using Backend.Application.Common.Models;
//namespace Backend.Application.Abstractions
//{
//    public interface IAdminCommandRepository
//    {
//        Task<Result> CreateRoleAsync(string roleName);
//        Task<Result> AssignPermissionsToRoleAsync(string roleId, string claimType, string claimValue);
//        Task<Result> AssignRoleToUserAsync(string userId, string roleName);
//        Task<Result> DeleteRoleAsync(string roleName);
//        Task<Result> DeletePermissionsAsync(string roleName, string claimType, string claimValue);
//    }
//}
using Backend.Application.Common.Models;


namespace Backend.Application.Abstractions
{
    public interface IAdminCommandRepository
    {
        Task<Result> CreateRoleAsync(string roleName);

        // Assign multiple permissions to a role
        Task<Result> AssignPermissionsToRoleAsync(string roleId, IEnumerable<string> permissionNames);

        // Assign multiple roles to a user
        Task<Result> AssignRolesToUserAsync(string userId, IEnumerable<string> roleNames);

        // Assign multiple permissions to a user
        Task<Result> AssignPermissionsToUserAsync(string userId, IEnumerable<string> permissionNames);

        Task<Result> DeleteRoleAsync(string roleName);

        Task<Result> DeletePermissionsFromRoleAsync(string roleName, IEnumerable<string> permissionNames);
    }
}
