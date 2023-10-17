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

app.MapGet("books", async (IBookService bookService) =>
{
    var books = await bookService.GetAllAsync();

    return Results.Ok(books);
});

app.MapGet("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var book = await bookService.GetByISBNAsync(isbn);

    return book is not null ? Results.Ok(book) : Results.NotFound();
});

app.Run();
