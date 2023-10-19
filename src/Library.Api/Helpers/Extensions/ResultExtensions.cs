using Library.Api.Helpers.Models;
using Microsoft.AspNetCore.Http;

namespace Library.Api.Helpers.Extensions;

public static class ResultExtensions
{
    public static IResult Html(this IResultExtensions extensions, string html)
    {
        return new HtmlResult(html);
    }
}
