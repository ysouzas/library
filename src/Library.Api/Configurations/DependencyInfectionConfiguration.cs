using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class DependencyInfectionConfiguration
{
    public static IServiceCollection AddSingletionServices(this IServiceCollection services)
    {
        return services;
    }
}
