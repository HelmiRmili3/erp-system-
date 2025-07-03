using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // General token generation with customizable expiration
    public string GenerateToken(IEnumerable<Claim> claims, int expiryMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Generate Access Token (short-lived)
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        int expiry = _configuration.GetValue<int>("JwtSettings:ExpiryInMinutes", 15);
        var claimsWithType = claims.Append(new Claim("typ", "access"));
        return GenerateToken(claimsWithType, expiry);
    }

    // Generate Refresh Token (long-lived)
    public string GenerateRefreshToken(IEnumerable<Claim> claims)
    {
        int expiry = _configuration.GetValue<int>("JwtSettings:ExpiryInDays", 7) * 24 * 60; // days to minutes
        var claimsWithType = claims.Append(new Claim("typ", "refresh"));
        return GenerateToken(claimsWithType, expiry);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = false // ignore expiration here
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
