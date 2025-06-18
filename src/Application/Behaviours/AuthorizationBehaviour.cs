﻿using Microsoft.AspNetCore.Http;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Services;
using System.Reflection;
using Backend.Application.Common.Security;

namespace Backend.Application.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehaviour(
        IUser user,
        IIdentityService identityService,
        IHttpContextAccessor httpContextAccessor)
    {
        _user = user;
        _identityService = identityService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            // Must be authenticated user
            if (_user.Id == null)
            {
                throw new UnauthorizedAccessException();
            }

            // // Role-based authorization
            // var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

            // if (authorizeAttributesWithRoles.Any())
            // {
            //     var authorized = false;

            //     foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles!.Split(',')))
            //     {
            //         foreach (var role in roles)
            //         {
            //             var isInRole = await _identityService.IsInRoleAsync(_user.Id, role.Trim());
            //             if (isInRole)
            //             {
            //                 authorized = true;
            //                 break;
            //             }
            //         }
            //     }

            //     // Must be a member of at least one role in roles
            //     if (!authorized)
            //     {
            //         throw new ForbiddenAccessException();
            //     }
            // }

            // // Policy-based authorization
            // var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
            // if (authorizeAttributesWithPolicies.Any())
            // {
            //     foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
            //     {
            //         // Vérifiez explicitement que policy n'est ni null ni une chaîne vide
            //         if (string.IsNullOrWhiteSpace(policy))
            //         {
            //             throw new InvalidOperationException("Policy name cannot be null or empty.");
            //         }

            //         var authorized = await _identityService.AuthorizeAsync(_user.Id, policy);

            //         if (!authorized)
            //         {
            //             throw new ForbiddenAccessException();
            //         }
            //     }
            // }
        }

        // User is authorized / authorization not required
        return await next();
    }
}
