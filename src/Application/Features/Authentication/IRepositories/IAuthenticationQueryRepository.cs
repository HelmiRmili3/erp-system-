
using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;

namespace Backend.Application.Features.Authentication.IRepositories;
public interface IAuthenticationQueryRepository
{
    Task<Response<RegisterResultDto>> GetUserById(string userId);

}
