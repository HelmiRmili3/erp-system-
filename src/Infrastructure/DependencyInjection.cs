using Backend.Application.Common.Interfaces;
using Backend.Domain.Constants;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Data.Interceptors;
using Backend.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Backend.Infrastructure.Services;
using Backend.Application.Services;
using Backend.Infrastructure.Repository.Command.Base;
using Backend.Infrastructure.Repository.Query.Base;
using Backend.Application.Features.Categories.IRepositories;
using Backend.Infrastructure.Repository.Query;
using Backend.Infrastructure.Repository.Command;
using Microsoft.Extensions.DependencyInjection;
using Backend.Application.Features.Configurations.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Application.Common.Settings;
using Backend.Application.Features.Authentication.IRepositories;
using Backend.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Backend.Application.Features.Admin.IRepositories;
using Backend.Application.Features.Absences.IRepositories;
using Backend.Application.Features.Certifications.IRepositories;
using Backend.Application.Features.Contracts.IRepositories;
using Backend.Application.Features.Expenses.IRepositories;

namespace Backend.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if (jwtSettings == null)
        {
            throw new InvalidOperationException("JWT settings not found.");
        }

        Guard.Against.Null(connectionString, message: "Connection string 'BackendDb' not found.");
        if (string.IsNullOrEmpty(jwtSettings.SecretKey) || string.IsNullOrEmpty(jwtSettings.Issuer) || string.IsNullOrEmpty(jwtSettings.Audience))
        {
            throw new InvalidOperationException("JWT settings are incomplete.");
        }

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, CleanupImagesInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();




        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ClockSkew = TimeSpan.FromMinutes(5),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    // Prevent the default behavior
                    context.HandleResponse();

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        succeeded = false,
                        message = "Unauthorized access. Please log in."
                    };

                    var json = System.Text.Json.JsonSerializer.Serialize(response);
                    return context.Response.WriteAsync(json);
                }
            };
        });

        builder.Services.AddAuthorizationBuilder();



        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();
        builder.Services.AddTransient<IFileService, FileService>();
        //JwtTokenService
        builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();

        //Types Register
        builder.Services.AddTransient(typeof(ICommandRepository<>), typeof(CommandRepository<>));
        builder.Services.AddTransient(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        builder.Services.AddTransient(typeof(IBaseAuthenticationCommandRepository), typeof(BaseAuthenticationCommandRepository));
        // Add your repositories here
        // Admin
        builder.Services.AddTransient<IAdminCommandRepository, AdminCommandRepository>();
        builder.Services.AddScoped<IAdminQueryRepository, AdminQueryRepository>();
        // Category
        builder.Services.AddTransient<ICategoryCommandRepository, CategoryCommandRepository>();
        builder.Services.AddTransient<ICategoryQueryRepository, CategoryQueryRepository>();
        // Configuration
        builder.Services.AddTransient<IConfigurationQueryRepository, ConfigurationQueryRepository>();
        builder.Services.AddTransient<IConfigurationCommandRepository, ConfigurationCommandRepository>();
        //Authentication
        builder.Services.AddTransient<IAuthenticationCommandRepository, AuthenticationCommandRepository>();
        builder.Services.AddScoped<IAuthenticationQueryRepository, AuthenticationQueryRepository>();
        // Absence
        builder.Services.AddTransient<IAbsenceCommandRepository, AbsenceCommandRepository>();
        builder.Services.AddTransient<IAbsenceQueryRepository, AbsenceQueryRepository>();
        // Attendance 
        builder.Services.AddTransient<IAttendanceCommandRepository, AttendanceCommandRepository>();
        builder.Services.AddTransient<IAttendanceQueryRepository, AttendanceQueryRepository>();
        // Certification
        builder.Services.AddTransient<ICertificationCommandRepository, CertificationCommandRepository>();
        builder.Services.AddTransient<ICertificationQueryRepository, CertifiactionQueryRepository>();
        // Contract 
        builder.Services.AddTransient<IContractCommandRepository, ContractCommandRepository>();
        builder.Services.AddTransient<IContractQueryRepository, ContractQueryRepository>();
        // Expenses
        builder.Services.AddTransient<IExpenseCommandRepository, ExpenseCommandRepository>();
        builder.Services.AddTransient<IExpenseQueryRepository, ExpenseQueryRepository>();
        // Payroll


        builder.Services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
    }
}
