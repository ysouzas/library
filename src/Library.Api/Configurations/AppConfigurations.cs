using System.Threading.Tasks;
using Library.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class AppConfigurations
{
    public static WebApplication AddBuilderServices(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static async Task<WebApplication> AddDatabaseInitializerAysnc(this WebApplication app)
    {
        var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.InitializeAsync();

        return app;
    }
}
