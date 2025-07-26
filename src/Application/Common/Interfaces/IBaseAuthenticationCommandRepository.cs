using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;

namespace Backend.Application.Common.Interfaces;
public interface IBaseAuthenticationCommandRepository
{
    Task<Response<RegisterResultDto>> RegisterAsync(RegisterDto data);
    Task<Response<LoginResultDto>> LoginAsync(string email, string password);
    Task<Response<string>> ChangePasswordAsync(ChangePasswordDataDto data);
    //Task<T> RevokeTokenAsync(string userId);
    Task<Response<LoginResultDto>> RefreshTokenAsync(string refreshToken);

    //Task<Response<RegisterResultDto>> GetProfile(string userId);
    //Task<T> ResetPasswordAsync(string email, string newPassword, string token);
    //Task LogoutAsync(string userId);
}
