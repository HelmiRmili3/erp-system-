using System.Reflection;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Backend.Application.Services;
using Microsoft.AspNetCore.Identity;
using Backend.Infrastructure.Identity;

namespace Backend.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options), IApplicationDbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Configuration> Configurations => Set<Configuration>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        builder.Entity<ApplicationRole>().ToTable("AspNetRoles");

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    public async Task<bool> ItemExists<T>(int id, CancellationToken cancellationToken) where T : class
    {
        return await Set<T>().FindAsync(id, cancellationToken) != null;
    }
}
