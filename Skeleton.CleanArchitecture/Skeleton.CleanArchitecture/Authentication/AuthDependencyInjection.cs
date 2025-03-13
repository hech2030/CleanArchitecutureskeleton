using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Skeleton.CleanArchitecture.Constants;

namespace Skeleton.CleanArchitecture.Authentication;

public static class AuthDependencyInjection
{
    public static IServiceCollection AddAuthInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftIdentityWebApiAuthentication(configuration, subscribeToJwtBearerMiddlewareDiagnosticsEvents: true);
        services
                .AddAuthorization(PoliciesRegistration);
        return services;
    }

    /// <summary>
    /// Authorization Policies registration.
    /// </summary>
    /// <param name="options"><see cref="AuthorizationOptions"/>.</param>
    public static void PoliciesRegistration(AuthorizationOptions options)
    {
        options.AddPolicy(AuthConstants.Policies.Read, builder =>
        {
            builder
                .RequireAuthenticatedUser()
                .RequireScopeOrAppPermission([AuthConstants.Roles.ScpRead, AuthConstants.Roles.ScpReadWrite], [AuthConstants.Roles.AppRead, AuthConstants.Roles.AppReadWrite]);
        });

        options.AddPolicy(AuthConstants.Policies.ReadWrite, builder =>
        {
            builder
                .RequireAuthenticatedUser()
                .RequireScopeOrAppPermission([AuthConstants.Roles.ScpReadWrite], [AuthConstants.Roles.AppReadWrite]);
        });
    }
}
