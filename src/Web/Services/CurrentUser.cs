using System.Security.Claims;
using Backend.Application.Common.Interfaces;

namespace Backend.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string Email =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    public string UserName =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
}
