using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Backend.Application.Services;
using Backend.Domain.Entities;

namespace Backend.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options), IApplicationDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<Absence> Absences { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
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
