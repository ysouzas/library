using FluentValidation;
using Library.Api.Configurations;
using Library.Api.Model;
using Library.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBuilderServices();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddSingletonServices();
builder.Services.AddValidators();

var app = builder.Build();

app.AddSwaggerConfiguration();

await app.AddDatabaseInitializerAysnc();

app.MapPost("book", async (Book book, IBookService bookService, IValidator<Book> validator) =>
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

    return Results.Created($"/books/{book.ISBN}", book);
});

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
});

app.MapGet("books", async (IBookService bookService, string? searchTerm) =>
{
    if (searchTerm is not null && !string.IsNullOrEmpty(searchTerm))
    {
        var matchedBookds = await bookService.SearchByTitleAsync(searchTerm);

        return Results.Ok(matchedBookds);
    }

    var books = await bookService.GetAllAsync();

    return Results.Ok(books);
});

app.MapGet("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var book = await bookService.GetByISBNAsync(isbn);

    return book is not null ? Results.Ok(book) : Results.NotFound();
});

app.Run();
