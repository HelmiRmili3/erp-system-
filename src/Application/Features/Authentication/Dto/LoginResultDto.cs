
//namespace Backend.Application.Features.Authentication.Dto;

//public class LoginResultDto
//{

//    public string UserId { get; set; } = string.Empty;
//    public string Email { get; set; } = string.Empty;
//    public string UserName { get; set; } = string.Empty;
//    public string Token { get; set; } = string.Empty;
//    public string RefreshToken { get; set; } = string.Empty;
//}

using Backend.Domain.Enums;

namespace Backend.Application.Features.Authentication.Dto;

public class LoginResultDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<ClaimDto> Claims { get; set; } = new();

    // Tokens
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class ClaimDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
