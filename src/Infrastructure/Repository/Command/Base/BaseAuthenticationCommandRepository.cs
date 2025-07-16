using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Response;
using Backend.Application.Common.Settings;
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
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                    return new Response<RegisterResultDto>("User with this email already exists");

                var existingUserByName = await _userManager.FindByNameAsync(registerDto.UserName);
                if (existingUserByName != null)
                    return new Response<RegisterResultDto>("Username already exists");

                var adminEmail = "administrator@localhost"; // Or get from config
                var adminUser = await _userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                    return new Response<RegisterResultDto>("Admin user not found to set as supervisor");

                var user = new ApplicationUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    EmailConfirmed = true,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    BirthDate = registerDto.BirthDate,
                    Address = registerDto.Address,
                    PhoneNumber = registerDto.Phone,
                    JobTitle = registerDto.JobTitle,
                    Department = registerDto.Department,
                    HireDate = registerDto.HireDate,
                    ContractType = registerDto.ContractType,
                    Status = registerDto.Status,
                    SupervisorId = adminUser.Id,
                    CreatedBy = adminUser.Id,
                    UpdatedBy = adminUser.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    var registerResult = new RegisterResultDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate,
                        Address = user.Address,
                        Phone = user.PhoneNumber,
                        JobTitle = user.JobTitle,
                        Department = user.Department,
                        HireDate = user.HireDate,
                        ContractType = user.ContractType,
                        Status = user.Status,
                        SupervisorId = user.SupervisorId
                    };
                    return new Response<RegisterResultDto>(registerResult, "User registered successfully");
                }

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
                return new Response<LoginResultDto>("Invalid email or password");

            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordValid)
                return new Response<LoginResultDto>("Invalid email or password");

            // Build base claims
            List<Claim> authClaims = new()
    {
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.GivenName, user.FirstName),
        new Claim(ClaimTypes.Surname, user.LastName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Add user roles as claims
            var userRoles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Add additional claims if not already present
            var existingClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in existingClaims)
            {
                if (!authClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                    authClaims.Add(claim);
            }

            // Generate tokens
            var accessToken = _jwtTokenService.GenerateAccessToken(authClaims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(authClaims);

            await _context.SaveChangesAsync();

            // Build LoginResultDto
            var expiresInSeconds = 60 * 60;

            var loginResult = new LoginResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,

                TokenType = "Bearer",
                ExpiresIn = expiresInSeconds
            };


            return new Response<LoginResultDto>(loginResult, "Login successful");
        }

        public virtual async Task<Response<string>> ChangePasswordAsync(ChangePasswordDataDto request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return new Response<string>("User not found")
                    .WithError("The specified user does not exist.");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                var response = new Response<string>("Failed to change password");
                foreach (var error in result.Errors)
                    response.WithError(error.Description, error.Code);
                return response;
            }

            return new Response<string>
            {
                Succeeded = true,
                Message = "Password changed successfully",
                Data = "PasswordChanged"
            };
        }

        public async Task<Response<LoginResultDto>> RefreshTokenAsync(string refreshToken)
        {
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(refreshToken);
            if (principal == null)
                return new Response<LoginResultDto>("Invalid refresh token")
                    .WithError("REFRESH_TOKEN_INVALID");

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return new Response<LoginResultDto>("User ID not found in token")
                    .WithError("INVALID_CLAIMS");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new Response<LoginResultDto>("User not found")
                    .WithError("USER_NOT_FOUND");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? ""),
                new Claim(ClaimTypes.Surname, user.LastName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var newAccessToken = _jwtTokenService.GenerateAccessToken(authClaims);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(authClaims);

            return new Response<LoginResultDto>(new LoginResultDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            }, "Token refreshed successfully");
        }
    }
}
