using System.Collections.Generic;
using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using Library.Api.Auth;
using Library.Api.Model;
using Library.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Library.Api.Endpoints.Books;

public static class BookPostEndpoint
{
    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder app)
    {
        app.MapPost("book", [Authorize(AuthenticationSchemes = ApiKeySchemeConstants.SchemeName)] async (Book book, IBookService bookService, IValidator<Book> validator) =>
        {
            var validationResult = await validator.ValidateAsync(book);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var created = await bookService.CreateAsync(book);

            if (!created)
            {
                return Results.BadRequest(new { errorMessage = "A book with this ISBN-13 already exist" });
            }

            return Results.CreatedAtRoute("GetBook", new { isbn = book.ISBN }, book);
        })
        .Accepts<Book>(MediaTypeNames.Application.Json)
        .Produces<Book>(StatusCodes.Status201Created)
        .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest);

        return app;
    }
}