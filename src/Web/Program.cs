using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Backend.Infrastructure;
using Backend.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.AddKeyVaultIfConfigured();   //  Load secrets/config first
builder.AddWebServices();
builder.AddInfrastructureServices(); // If it uses configs from KeyVault
builder.AddApplicationServices();
builder.Services.AddHttpContextAccessor();

string localIp = GetLocalIPAddress();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost", policy => policy.WithOrigins(
            "http://localhost:4000",
            "http://localhost:3000",
            "http://localhost:3000",
            "http://localhost",
            // "https://localhost:4000",
            "http://10.0.2.2:5001",
            $"http://{localIp}:3000",
            "http://localhost:5001",
            // $"https://{localIp}:5001",
            "http://localhost:8081",
            "http://172.190.236.125:3000"
        )
        .AllowAnyHeader()
        .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
        .AllowCredentials());
    });
}
else
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowProduction", policy => policy
            .WithOrigins(
              "http://localhost:4000",
                "http://localhost:8081",
                "https://localhost:4000",
                "http://localhost:3000",
                "http://10.0.2.2:5001",
                $"http://{localIp}:3000",
                $"https://{localIp}:5001",
                "http://localhost:5001",
                "http://51.195.116.184:8081",
                "http://helmirmili.tn",
                "https://helmirmili.tn"
            )
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
            .AllowCredentials());
    });
}

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls(
        $"http://{localIp}:5000",
        // $"https://{localIp}:5001",
        "http://localhost:5000"
    // "https://localhost:5001"
    );
}

var app = builder.Build();


// Exception handling should be first
app.UseExceptionHandler("/error");

app.UseCors(app.Environment.IsDevelopment() ? "AllowLocalhost" : "AllowProduction");
if (builder.Environment.IsDevelopment())
{
    app.UseCors("AllowLocalhost");
}
else
{
    app.UseCors("AllowProduction");
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.InitialiseDatabase();
    app.UseHsts();
}

app.UseHealthChecks("/health");

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/api";
        settings.DocumentPath = "/api/specification.json";
    });
}

// Map controllers
app.MapControllers();

app.Map("/error", (HttpContext context) =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    if (exception is FluentValidation.ValidationException fluentValidationException)
    {
        var errors = fluentValidationException.Errors
            .GroupBy(e => e.PropertyName)
            .Select(group => new
            {
                Field = group.Key,
                Messages = group.Select(e => e.ErrorMessage).ToList()
            });

        return Results.Json(new
        {
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest,
            Errors = errors
        }, statusCode: StatusCodes.Status400BadRequest);
    }

    if (exception is Backend.Application.Common.Exceptions.ValidationException customValidationException)
    {
        var errors = customValidationException.Errors
            .GroupBy(e => e.Value)
            .Select(group => new
            {
                Field = group.Key,
                Messages = group.Select(e => e.Value).ToList()
            });

        return Results.Json(new
        {
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest,
            Errors = errors
        }, statusCode: StatusCodes.Status400BadRequest);
    }

    return Results.Problem(
        detail: exception?.Message,
        title: "An error occurred",
        statusCode: StatusCodes.Status500InternalServerError);
});

app.Map("/", () => Results.Redirect("/api"));
//app.MapEndpoints();

app.Run();

static string GetLocalIPAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return ip.ToString();
        }
    }
    throw new Exception("No network adapters with an IPv4 address found.");
}

public partial class Program { }
