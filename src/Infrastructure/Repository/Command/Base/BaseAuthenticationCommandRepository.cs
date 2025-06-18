using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Repository.Command.Base
{
    public class BaseAuthenticationCommandRepository : IBaseAuthenticationCommandRepository
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly IJwtTokenService _jwtTokenService;
        protected readonly ApplicationDbContext _context;

        public BaseAuthenticationCommandRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        public virtual async Task<Response<RegisterResultDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return new Response<RegisterResultDto>("User with this email already exists");
                }

                // Check if username already exists
                var existingUserByName = await _userManager.FindByNameAsync(registerDto.UserName);
                if (existingUserByName != null)
                {
                    return new Response<RegisterResultDto>("Username already exists");
                }

                var user = new ApplicationUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    EmailConfirmed = true // Set to false if you want email confirmation
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    var registerResult = new RegisterResultDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName
                    };

                    return new Response<RegisterResultDto>(registerResult, "User registered successfully");
                }

                // Collect all errors from Identity result
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Response<RegisterResultDto>($"Registration failed: {errors}");
            }
            catch (Exception ex)
            {
                return new Response<RegisterResultDto>($"Registration failed: {ex.Message}");
            }
        }

        public virtual async Task<Response<LoginResultDto>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new Response<LoginResultDto>("Invalid email or password");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordValid)
            {
                return new Response<LoginResultDto>("Invalid email or password");
            }

            List<Claim> authClaims = new List<Claim>
            {
                new(ClaimTypes.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var accessToken = _jwtTokenService.GenerateToken(authClaims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            //var tokenInfo = await _context.TokenInfos
            //    .FirstOrDefaultAsync(a => a.Username == user.UserName);

            //if (tokenInfo == null)
            //{
            //    tokenInfo = new TokenInfo
            //    {
            //        Username = user.UserName,
            //        RefreshToken = refreshToken,
            //        ExpiredAt = DateTime.UtcNow.AddDays(7)
            //    };
            //    await _context.TokenInfos.AddAsync(tokenInfo);
            //}
            //else
            //{
            //    tokenInfo.RefreshToken = refreshToken;
            //    tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
            //    _context.TokenInfos.Update(tokenInfo);
            //}

            await _context.SaveChangesAsync();

            var loginResult = new LoginResultDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty
            };

            return new Response<LoginResultDto>(loginResult, "Login successful");
        }

    //    public virtual async Task LogoutAsync(string userId)
    //    {
    //        await _signInManager.SignOutAsync();
    //    }

    //    public virtual Task<string> RefreshTokenAsync(string refreshToken)
    //    {
    //        // Placeholder: actual implementation depends on your refresh token validation logic
    //        throw new NotImplementedException();
    //    }

    //    public virtual async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    //    {
    //        var user = await _userManager.FindByIdAsync(userId);
    //        if (user == null) return false;

    //        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    //        return result.Succeeded;
    //    }

    //    public virtual async Task<bool> ResetPasswordAsync(string email, string newPassword, string token)
    //    {
    //        var user = await _userManager.FindByEmailAsync(email);
    //        if (user == null) return false;

    //        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
    //        return result.Succeeded;
    //    }

    //    public virtual Task<bool> RevokeTokenAsync(string userId)
    //    {
    //        // Placeholder: actual implementation depends on your token storage strategy
    //        throw new NotImplementedException();
    //    }
    }
}
