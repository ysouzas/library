using System.Threading.Tasks;
using Library.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class DatabaseConfigurations
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Database:ConnectionString");

        services.AddSingleton<IDbConnectionFactory>
            (_ => new SqlLiteConnectionFactory(connectionString));

        services.AddSingleton<DatabaseInitializer>();

        return services;
    }

    public static async Task<IApplicationBuilder> AddDatabaseInitializerAysnc(this WebApplication app)
    {
        var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.InitializeAsync();

        return app;
    }
}
