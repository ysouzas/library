using Library.Api.Configurations;
using Library.Api.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddJsonOptions();
builder.Services.AddAuthentication();
builder.Services.AddBuilderServices();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddSingletonServices();
builder.Services.AddValidators();

var app = builder.Build();

app.AddUseCors();
app.AddSwaggerConfiguration();
app.AddAuthorizationConfiguration();
app.AddApiEndpoints();

await app.AddDatabaseInitializerAysnc();

app.MapGet("status", [EnableCors("AnyOrigin")] () =>
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
