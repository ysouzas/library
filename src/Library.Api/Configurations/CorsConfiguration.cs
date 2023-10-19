using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class CorsConfiguration
{
    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AnyOrigin", x => x.AllowAnyOrigin());
        });

        return services;
    }

    public static WebApplication AddUseCors(this WebApplication app)
    {
        app.UseCors();

        return app;
    }

}
