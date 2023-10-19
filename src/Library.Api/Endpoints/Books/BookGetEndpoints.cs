using System.Collections.Generic;
using FluentValidation.Results;
using Library.Api.Model;
using Library.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Library.Api.Endpoints.Books;

public static class BookGetEndpoints
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder app)
    {
        app.MapGet("books", async (IBookService bookService, string? searchTerm) =>
        {
            if (searchTerm is not null && !string.IsNullOrEmpty(searchTerm))
            {
                var matchedBookds = await bookService.SearchByTitleAsync(searchTerm);

                return Results.Ok(matchedBookds);
            }

            var books = await bookService.GetAllAsync();

            return Results.Ok(books);
        })
        .WithName("GetBooks")
        .Produces<IEnumerable<Book>>(StatusCodes.Status200OK)
        .Produces<IEnumerable<ValidationFailure>>(StatusCodes.Status400BadRequest);

        app.MapGet("books/{isbn}", async (string isbn, IBookService bookService) =>
        {
            var book = await bookService.GetByISBNAsync(isbn);

            return book is not null ? Results.Ok(book) : Results.NotFound();
        })
        .WithName("GetBook")
        .Produces<Book>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}
