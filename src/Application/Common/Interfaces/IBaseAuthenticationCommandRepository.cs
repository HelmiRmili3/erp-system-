using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;

namespace Backend.Application.Common.Interfaces;
public interface IBaseAuthenticationCommandRepository
{
    Task<Response<RegisterResultDto>> RegisterAsync(RegisterDto entity );
    Task<Response<LoginResultDto>> LoginAsync(string email, string password);
    //Task<T> RevokeTokenAsync(string userId);
    //Task<T> RefreshTokenAsync(string refreshToken);
    //Task<T> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    //Task<T> ResetPasswordAsync(string email, string newPassword, string token);
    //Task LogoutAsync(string userId);
}
