
namespace Backend.Application.Features.Authentication.Dto;

public class LoginResultDto
{
    
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
