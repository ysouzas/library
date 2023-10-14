using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class BuilderConfigurations
{
    public static IServiceCollection AddBuilderServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
