using Backend.Application.Features.Admin.Dto;
using Backend.Application.Common.Response;
using Backend.Application.Common.Models;


namespace Backend.Application.Features.Admin.IRepositories;

public interface IAdminQueryRepository
{
    Task<PagedResponse<List<PermissionDto>>> GetPermissionsWithPaginationAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task<PagedResponse<List<RoleDto>>> GetRolesAsync(int pageNumber,
        int pageSize, CancellationToken cancellationToken = default);
    Task<Response<RoleDto>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);
    Task<PagedResponse<List<RoleWithPermissionsDto>>> GetRolesWithPermissionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResponse<List<UserDto>>> GetUsersWithRolesAndPermissionsAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken);
}
