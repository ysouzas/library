using Library.Api.Endpoints;
using Microsoft.AspNetCore.Builder;

namespace Library.Api.Configurations;

public static class ApiConfiguration
{
    public static void AddApiEndpoints(this WebApplication app)
    {
        app.MapApiEndpoints();
    }
}
