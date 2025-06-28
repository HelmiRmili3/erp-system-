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
                // Check if user already exists by email
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

                // Find admin by email (replace with your actual admin email)
                var adminEmail = "administrator@localhost"; // Or get it from config
                var adminUser = await _userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    return new Response<RegisterResultDto>("Admin user not found to set as supervisor");
                }

                var user = new ApplicationUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    EmailConfirmed = true,

                    // Personal Information
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    BirthDate = registerDto.BirthDate,
                    Address = registerDto.Address,

                    PhoneNumber = registerDto.Phone,

                    // Professional Information
                    JobTitle = registerDto.JobTitle,
                    Department = registerDto.Department,
                    HireDate = registerDto.HireDate,
                    ContractType = registerDto.ContractType,
                    Status = registerDto.Status,

                    // Set supervisor to admin user
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
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.GivenName, user.FirstName),
        new Claim(ClaimTypes.Surname, user.LastName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            //Add Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            //Add any custom claims stored with the user
            var existingClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in existingClaims)
            {
                // Avoid duplicates
                if (!authClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                {
                    authClaims.Add(claim);
                }
            }

            var accessToken = _jwtTokenService.GenerateToken(authClaims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

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
