using Library.Api.Configurations;
using Library.Api.Model;
using Library.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBuilderServices();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddSingletonServices();


var app = builder.Build();

app.AddSwaggerConfiguration();

await app.AddDatabaseInitializerAysnc();

app.MapPost("book", async (Book book, IBookService bookService) =>
{
    var created = await bookService.CreateAsync(book);

    if (!created)
    {
        return Results.BadRequest(new { errorMessage = "A book with this ISBN-13 already exist" });
    }

    return Results.Created($"/books/{book.ISBN}", book);
});



app.Run();
