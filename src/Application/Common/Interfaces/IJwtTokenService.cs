using System.Security.Claims;

namespace Backend.Application.Common.Interfaces;
public interface IJwtTokenService
{
    string GenerateToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
