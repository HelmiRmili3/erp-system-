using Microsoft.AspNetCore.Builder;

namespace Backend.Infrastructure.Middleware;
public static class ContentTypeValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseContentTypeValidation(this IApplicationBuilder app, string contentType)
    {
        return app.UseMiddleware<ContentTypeValidationMiddleware>(contentType);
    }
}
