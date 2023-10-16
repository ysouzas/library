using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class ValidatorsConfigurations
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }
}