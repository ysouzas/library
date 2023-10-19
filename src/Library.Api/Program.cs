using System.Collections.Generic;
using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using Library.Api.Auth;
using Library.Api.Configurations;
using Library.Api.Helpers.Extensions;
using Library.Api.Model;
using Library.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJsonOptions();
builder.Services.AddAuthentication();
builder.Services.AddBuilderServices();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddSingletonServices();
builder.Services.AddValidators();

var app = builder.Build();

app.AddSwaggerConfiguration();
app.AddAuthorizationConfiguration();

await app.AddDatabaseInitializerAysnc();

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

app.MapDelete("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var deleted = await bookService.DeleteAsync(isbn);

    return deleted ? Results.NoContent() : Results.NotFound();
})
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);



app.MapGet("status", () =>
{
    return Results.Extensions.Html(@"<!DOCTYPE html>
<html>
<head>
    <title>Status Page</title>
    <style>
        body {
            font-family: Arial, sans-serif;
        }
        #status {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <h1>Status</h1>
    <div>Working</div>
</body>
</html>
");
})
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);


app.Run();
