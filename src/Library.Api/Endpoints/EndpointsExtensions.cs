using Library.Api.Endpoints.Books;
using Microsoft.AspNetCore.Routing;

namespace Library.Api.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapBookEndpoints();

        return app;
    }
}