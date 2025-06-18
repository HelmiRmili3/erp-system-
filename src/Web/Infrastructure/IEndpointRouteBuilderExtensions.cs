using System.Diagnostics.CodeAnalysis;

namespace Backend.Web.Infrastructure;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name)
            .DisableAntiforgery();

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name)
            .DisableAntiforgery();

        return builder;
    }

    public static IEndpointRouteBuilder MapPost<TRequest>(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern, [StringSyntax("ContentType")] string contentType) where TRequest : notnull
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name)
            .Accepts<TRequest>(contentType)
            .DisableAntiforgery();


        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name)
            .DisableAntiforgery();

        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern, [StringSyntax("ContentType")] string contentType)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name)
            .Accepts<object>(contentType)
            .DisableAntiforgery();

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name)
            .DisableAntiforgery();


        return builder;
    }
}
