using Backend.Domain.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static void InitialiseDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }

    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // ➤ Create default roles
        var adminRole = new ApplicationRole(Roles.Administrator);

        if (!_roleManager.Roles.Any(r => r.Name == adminRole.Name))
        {
            await _roleManager.CreateAsync(adminRole);
        }

        // ➤ Create default admin user
        var adminEmail = "administrator@localhost";
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        if (!_userManager.Users.Any(u => u.UserName == adminUser.UserName))
        {
            var result = await _userManager.CreateAsync(adminUser, "Administrator1!");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, adminRole.ToString());
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("User creation error: {Error}", error.Description);
                }
            }
        }

        // ➤ Seed configuration table if empty
        if (!_context.Configurations.Any())
        {
            var defaultConfigs = new[]
            {
                new Configuration("name", "Products Showcase App"),
                new Configuration("slogan", "Your slogan here"),
                new Configuration("logo", "logo.png"),
                new Configuration("phone", "+1234567890"),
                new Configuration("email", "email@example.com"),
                new Configuration("address", "123 Street, City, Country"),
                new Configuration("currency", "USD"),
            };

            _context.Configurations.AddRange(defaultConfigs);
            await _context.SaveChangesAsync();
        }
        // ➤ Seed Permissions
        if (!_context.Permissions.Any())
        {
            var permissionEntities = Permissions.All.Select(p => new Permission
            {
                Name = p,
                Description = $"Allows {p.Replace(".", " ").ToLower()} access"
            });

            _context.Permissions.AddRange(permissionEntities);
            await _context.SaveChangesAsync();
        }

    }
}
