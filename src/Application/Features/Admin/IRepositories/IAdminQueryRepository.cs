using Backend.Application.Features.Admin.Dto;
using Backend.Application.Common.Response;

namespace Backend.Application.Features.Admin.IRepositories;

public interface IAdminQueryRepository
{
    Task<Response<List<PermissionDto>>> GetPermissionsAsync(CancellationToken cancellationToken = default);

    Task<Response<List<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<Response<RoleDto>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);

    Task<Response<List<RoleWithPermissionsDto>>> GetRolesWithPermissionsAsync(CancellationToken cancellationToken = default);
}
