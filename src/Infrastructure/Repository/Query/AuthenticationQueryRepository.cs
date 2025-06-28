using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Repository.Query;

public class AuthenticationQueryRepository : IAuthenticationQueryRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationQueryRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<RegisterResultDto>> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return new Response<RegisterResultDto>("User not found");
        }

        var resultDto = new RegisterResultDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            BirthDate = user.BirthDate,
            Phone = user.PhoneNumber,
            Department = user.Department,
            JobTitle = user.JobTitle,
            HireDate = user.HireDate,
            ContractType = user.ContractType,
            Status = user.Status,
            SupervisorId = user.SupervisorId
        };

        return new Response<RegisterResultDto>(resultDto, "User fetched successfully");
    }
}
