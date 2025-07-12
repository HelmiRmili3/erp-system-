using Backend.Application.Common.Response;
using Backend.Application.Features.Admin.Dto;
using Backend.Application.Features.Admin.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Repository.Query;

public class AdminQueryRepository : IAdminQueryRepository
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AdminQueryRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Response<ClaimDto>> GetClaimByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var allClaims = await GetAllClaimsAsync();
        var claim = allClaims.FirstOrDefault(c => c.Id == id);

        return claim == null
            ? new Response<ClaimDto>("Claim not found")
            : new Response<ClaimDto>(claim);
    }

    public async Task<Response<List<ClaimDto>>> GetClaimsAsync(CancellationToken cancellationToken = default)
    {
        var claims = await GetAllClaimsAsync();
        return new Response<List<ClaimDto>>(claims);
    }

    public async Task<Response<List<ClaimWithRolesDto>>> GetClaimsWithRolesAsync(CancellationToken cancellationToken = default)
{
    var roles = _roleManager.Roles.ToList();
    var result = new List<ClaimWithRolesDto>();

    // Get all known claims with generated IDs
    var allClaims = await GetAllClaimsAsync();

    foreach (var role in roles)
    {
        var claims = await _roleManager.GetClaimsAsync(role);

        foreach (var claim in claims)
        {
            // Find matching claim with ID
            var matchingClaim = allClaims.FirstOrDefault(c => c.Type == claim.Type && c.Value == claim.Value);

            var existing = result.FirstOrDefault(r => r.Type == claim.Type && r.Value == claim.Value);
            if (existing == null)
            {
                result.Add(new ClaimWithRolesDto
                {
                    ClaimId = matchingClaim?.Id ?? 0,
                    Type = claim.Type,
                    Value = claim.Value,
                    Roles = new List<RoleDto> 
                    { 
                        new RoleDto 
                        { 
                            Id = role.Id, 
                            Name = role.ToString() 
                        } 
                    }
                });
            }
            else
            {
                existing.Roles.Add(new RoleDto 
                { 
                    Id = role.Id,
                    Name = role.ToString()
                });
            }
        }
    }

    return new Response<List<ClaimWithRolesDto>>(result);
}


    public async Task<Response<RoleDto>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        return role == null
            ? new Response<RoleDto>("Role not found")
            : new Response<RoleDto>(new RoleDto { Id = role.Id, Name = role.ToString() });
    }

 
    public async Task<Response<List<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await Task.Run(() =>
            _roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.ToString() })
                .ToList(), cancellationToken);

        return new Response<List<RoleDto>>(roles);
    }

    public async Task<Response<List<RoleWithClaimsDto>>> GetRolesWithClaimsAsync(CancellationToken cancellationToken = default)
    {
        var roles = _roleManager.Roles.ToList();
        var result = new List<RoleWithClaimsDto>();

        // Get all claims with generated IDs
        var allClaims = await GetAllClaimsAsync();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);

            var claimDtos = claims.Select(c =>
            {
                var matchingClaim = allClaims.FirstOrDefault(ac => ac.Type == c.Type && ac.Value == c.Value);
                return new ClaimDto
                {
                    Id = matchingClaim?.Id ?? 0,
                    Type = c.Type,
                    Value = c.Value
                };
            }).ToList();

            result.Add(new RoleWithClaimsDto
            {
                RoleId = role.Id,
                RoleName = role.ToString(),
                Claims = claimDtos
            });
        }

        return new Response<List<RoleWithClaimsDto>>(result);
    }


    private async Task<List<ClaimDto>> GetAllClaimsAsync()
    {
        var result = new List<ClaimDto>();
        var roles = _roleManager.Roles.ToList();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                if (!result.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                {
                    result.Add(new ClaimDto { Type = claim.Type, Value = claim.Value, Id = result.Count + 1 });
                }
            }
        }

        return result;
    }
}
