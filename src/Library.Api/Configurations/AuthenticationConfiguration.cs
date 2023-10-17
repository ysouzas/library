using Library.Api.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(ApiKeySchemeConstants.SchemeName)
                .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeySchemeConstants.SchemeName, _ => { });

        services.AddAuthorization();

        return services;
    }
}
