using Library.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class DependencyInfectionConfiguration
{
    public static IServiceCollection AddSingletonServices(this IServiceCollection services)
    {
        services.AddSingleton<IBookService, BookService>();

        return services;
    }
}
