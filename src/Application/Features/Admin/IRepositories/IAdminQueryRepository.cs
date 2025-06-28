using Backend.Application.Features.Admin.Dto;
using Backend.Application.Common.Response;

namespace Backend.Application.Features.Admin.IRepositories;

public interface IAdminQueryRepository
{
    Task<Response<List<ClaimDto>>> GetClaimsAsync(CancellationToken cancellationToken = default);
    Task<Response<ClaimDto>> GetClaimByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Response<List<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<Response<RoleDto>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);

    Task<Response<List<RoleWithClaimsDto>>> GetRolesWithClaimsAsync(CancellationToken cancellationToken = default);
    Task<Response<List<ClaimWithRolesDto>>> GetClaimsWithRolesAsync(CancellationToken cancellationToken = default);
}
