using Microsoft.AspNetCore.Http;

namespace Backend.Infrastructure.Middleware;
public class ContentTypeValidationMiddleware(RequestDelegate next, string expectedContentType)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
    private readonly string _expectedContentType = expectedContentType ?? throw new ArgumentNullException(nameof(expectedContentType));

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.ContentType?.StartsWith(_expectedContentType, StringComparison.OrdinalIgnoreCase) ?? true)
        {
            context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            await context.Response.WriteAsync($"Unsupported Content-Type. Expected: {_expectedContentType}");
            return;
        }

        await _next(context);
    }
}
