
namespace Backend.Application.Features.Authentication.Dto;

public class LoginResultDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = "Bearer";      // constant value
    public int ExpiresIn { get; set; }                     // in seconds
}

public class ClaimDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

