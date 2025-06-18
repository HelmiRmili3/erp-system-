using Backend.Application.Common.Interfaces;
using Backend.Application.Features.Authentication.IRepositories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository.Command.Base;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Repository.Command;

public class AuthenticationCommandRepository(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtTokenService jwtTokenService,
    ApplicationDbContext context
    ): BaseAuthenticationCommandRepository(
        userManager,
        signInManager,
        jwtTokenService,
        context
        ),IAuthenticationCommandRepository
{

}
