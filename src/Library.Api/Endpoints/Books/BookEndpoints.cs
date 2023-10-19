using Microsoft.AspNetCore.Routing;

namespace Library.Api.Endpoints.Books;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost();
        app.MapGet();
        app.MapPut();
        app.MapDelete();

        return app;
    }
}
