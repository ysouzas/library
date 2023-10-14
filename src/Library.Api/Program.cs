using Library.Api;
using Library.Api.Configurations;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBuilderServices();
builder.Services.AddDatabaseConfiguration(builder.Configuration);

var app = builder.Build();

app.AddBuilderServices();

await app.AddDatabaseInitializerAysnc();

app.MapGet("/", () => "Hello World!");

app.Run();
