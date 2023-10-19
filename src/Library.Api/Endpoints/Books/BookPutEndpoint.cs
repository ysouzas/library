using System.Collections.Generic;
using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using Library.Api.Model;
using Library.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Library.Api.Endpoints.Books;

public static class BookPutEndpoint
{
    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder app)
    {

        app.MapPut("book/{isbn}", async (string isbn, Book book, IBookService bookService, IValidator<Book> validator) =>
        {
            book.ISBN = isbn;

            var validationResult = await validator.ValidateAsync(book);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var updated = await bookService.UpdateAsync(book);

            return updated ? Results.Ok(book) : Results.NotFound();
        })
        .WithName("UpdateBook")
        .Accepts<Book>(MediaTypeNames.Application.Json)
        .Produces<Book>(StatusCodes.Status200OK)
        .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest);


        return app;
    }
}