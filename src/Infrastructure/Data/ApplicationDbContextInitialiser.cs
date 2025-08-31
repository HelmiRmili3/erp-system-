// using Backend.Domain.Constants;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;

// namespace Backend.Infrastructure.Data;

// public static class InitialiserExtensions
// {
//     public static void InitialiseDatabase(this WebApplication app)
//     {
//         using var scope = app.Services.CreateScope();
//         var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//         dbContext.Database.Migrate();
//     }

//     public static async Task InitialiseDatabaseAsync(this WebApplication app)
//     {
//         using var scope = app.Services.CreateScope();

//         var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

//         await initialiser.InitialiseAsync();
//         await initialiser.SeedAsync();
//     }
// }

// public class ApplicationDbContextInitialiser
// {
//     private readonly ILogger<ApplicationDbContextInitialiser> _logger;
//     private readonly ApplicationDbContext _context;
//     private readonly UserManager<ApplicationUser> _userManager;
//     private readonly RoleManager<ApplicationRole> _roleManager;

//     public ApplicationDbContextInitialiser(
//         ILogger<ApplicationDbContextInitialiser> logger,
//         ApplicationDbContext context,
//         UserManager<ApplicationUser> userManager,
//         RoleManager<ApplicationRole> roleManager)
//     {
//         _logger = logger;
//         _context = context;
//         _userManager = userManager;
//         _roleManager = roleManager;
//     }

//     public async Task InitialiseAsync()
//     {
//         try
//         {
//             await _context.Database.MigrateAsync();
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "An error occurred while initializing the database.");
//             throw;
//         }
//     }

//     public async Task SeedAsync()
//     {
//         try
//         {
//             await TrySeedAsync();
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "An error occurred while seeding the database.");
//             throw;
//         }
//     }

//     private async Task TrySeedAsync()
//     {
//         // ➤ Create default roles
//         var adminRole = new ApplicationRole(Roles.Administrator);

//         if (!_roleManager.Roles.Any(r => r.Name == adminRole.Name))
//         {
//             await _roleManager.CreateAsync(adminRole);
//         }

//         // ➤ Create default admin user
//         var adminEmail = "administrator@gmail.com";
//         var adminUser = new ApplicationUser
//         {
//             UserName = adminEmail,
//             Email = adminEmail,
//             EmailConfirmed = true
//         };

//         if (!_userManager.Users.Any(u => u.UserName == adminUser.UserName))
//         {
//             var result = await _userManager.CreateAsync(adminUser, "Password-123");

//             if (result.Succeeded)
//             {
//                 await _userManager.AddToRoleAsync(adminUser, adminRole.ToString());
//             }
//             else
//             {
//                 foreach (var error in result.Errors)
//                 {
//                     _logger.LogError("User creation error: {Error}", error.Description);
//                 }
//             }
//         }

//         // ➤ Seed configuration table if empty
//         if (!_context.Configurations.Any())
//         {
//             var defaultConfigs = new[]
//             {
//                 new Configuration("name", "Products Showcase App"),
//                 new Configuration("slogan", "Your slogan here"),
//                 new Configuration("logo", "logo.png"),
//                 new Configuration("phone", "+1234567890"),
//                 new Configuration("email", "email@example.com"),
//                 new Configuration("address", "123 Street, City, Country"),
//                 new Configuration("currency", "USD"),
//             };

//             _context.Configurations.AddRange(defaultConfigs);
//             await _context.SaveChangesAsync();
//         }
//         // ➤ Seed Permissions
//         if (!_context.Permissions.Any())
//         {
//             var permissionEntities = Permissions.All.Select(p => new Permission
//             {
//                 Name = p,
//                 Description = $"Allows {p.Replace(".", " ").ToLower()} access"
//             });

//             _context.Permissions.AddRange(permissionEntities);
//             await _context.SaveChangesAsync();
//         }

//     }
// }
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
        // ➤ Seed Permissions first
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

        // ➤ Create default roles
        var adminRole = new ApplicationRole(Roles.Administrator);
        var employeeRole = new ApplicationRole(Roles.Employee);

        // Create Admin role if it doesn't exist
        if (!_roleManager.Roles.Any(r => r.Name == adminRole.Name))
        {
            await _roleManager.CreateAsync(adminRole);
        }

        // Create Employee role if it doesn't exist
        if (!_roleManager.Roles.Any(r => r.Name == employeeRole.Name))
        {
            await _roleManager.CreateAsync(employeeRole);
        }

        // ➤ Add all permissions to Admin role
        var adminRoleEntity = await _roleManager.FindByNameAsync(adminRole.Name!);
        if (adminRoleEntity != null)
        {
            // Remove existing role permissions to avoid duplicates
            var existingAdminPermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == adminRoleEntity.Id);
            _context.RolePermissions.RemoveRange(existingAdminPermissions);

            // Add all permissions to admin role
            var allPermissions = await _context.Permissions.ToListAsync();
            var adminRolePermissions = allPermissions.Select(p => new RolePermission
            {
                RoleId = adminRoleEntity.Id,
                PermissionId = p.Id
            });

            _context.RolePermissions.AddRange(adminRolePermissions);
            await _context.SaveChangesAsync();
        }

        // ➤ Add specific permissions to Employee role
        var employeeRoleEntity = await _roleManager.FindByNameAsync(employeeRole.Name!);
        if (employeeRoleEntity != null)
        {
            // Remove existing role permissions to avoid duplicates
            var existingEmployeePermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == employeeRoleEntity.Id);
            _context.RolePermissions.RemoveRange(existingEmployeePermissions);

            // Define employee permissions (modify this list as needed)
            var employeePermissionNames = new List<string>
            {
                Permissions.Absences.View,
                Permissions.Absences.Create,
                Permissions.Absences.Edit,
                Permissions.Absences.Delete,

                Permissions.Attendances.Create,
                Permissions.Attendances.View,

                Permissions.Expanses.Create,
                Permissions.Expanses.Delete,
                Permissions.Expanses.Edit,
                Permissions.Expanses.View,

                Permissions.Certifications.View,

                Permissions.Payrolls.View,
                Permissions.Contracts.View,
                // Add more permissions as needed for employees
            };

            var employeePermissions = await _context.Permissions
                .Where(p => employeePermissionNames.Contains(p.Name))
                .ToListAsync();

            var employeeRolePermissions = employeePermissions.Select(p => new RolePermission
            {
                RoleId = employeeRoleEntity.Id,
                PermissionId = p.Id
            });

            _context.RolePermissions.AddRange(employeeRolePermissions);
            await _context.SaveChangesAsync();
        }

        // ➤ Create default admin user
        var adminEmail = "administrator@gmail.com";
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        if (!_userManager.Users.Any(u => u.UserName == adminUser.UserName))
        {
            var result = await _userManager.CreateAsync(adminUser, "Password-123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, adminRole.Name!);
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
    }
}
