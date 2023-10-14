using Library.Api.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class DatabaseConfigurations
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetValue<string>("Database:ConnectionString");

        services.AddSingleton<IDbConnectionFactory>
            (_ => new SqlLiteConnectionFactory(connectionString));

        services.AddSingleton<DatabaseInitializer>();

        return services;
    }
}
