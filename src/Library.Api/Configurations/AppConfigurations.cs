using Microsoft.AspNetCore.Builder;

namespace Library.Api.Configurations;

public static class AppConfigurations
{
    public static IApplicationBuilder AddSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static IApplicationBuilder AddAuthorizationConfiguration(this IApplicationBuilder app)
    {
        app.UseAuthorization();

        return app;
    }
}
