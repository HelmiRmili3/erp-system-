using Backend.Application.Common.Response;
using Backend.Application.Features.Authentication.Dto;
using Backend.Application.Features.Authentication.IRepositories;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Backend.Infrastructure.Data;

namespace Backend.Infrastructure.Repository.Query;

public class AuthenticationQueryRepository : IAuthenticationQueryRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AuthenticationQueryRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Response<RegisterResultDto>> GetUserById(string userId)
    {
        var user = await _userManager.Users
            .Include(u => u.Subordinates)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new Response<RegisterResultDto>("User not found");

        // Get role names
        var roleNames = await _userManager.GetRolesAsync(user);

        // Get role IDs by name
        var roleIds = await _context.ApplicationRoles
            .Where(r => roleNames.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();

        // Get permissions from roles
        var rolePermissions = await _context.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission.Name)
            .ToListAsync();

        // Get direct user permissions
        var userPermissions = await _context.UserPermissions
            .Where(up => up.UserId == userId)
            .Include(up => up.Permission)
            .Select(up => up.Permission.Name)
            .ToListAsync();

        // Merge and deduplicate
        var allPermissions = rolePermissions
            .Concat(userPermissions)
            .Distinct()
            .ToList();

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
            SupervisorId = user.SupervisorId,
            Roles = roleNames.ToList(),
            Permissions = allPermissions
        };

        return new Response<RegisterResultDto>(resultDto, "User fetched successfully");
    }
}
