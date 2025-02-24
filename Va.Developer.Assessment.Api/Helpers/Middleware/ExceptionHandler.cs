using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text;

namespace Va.Developer.Assessment.Api.Helpers.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await HandleExceptionAsync(httpContext, exception, cancellationToken);
        return true;
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, CancellationToken cancellation)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Error: {exception.Message}");
        if (exception.InnerException is not null)
        {
            sb.AppendLine($"Detailed Error: {exception.InnerException.Message}");
        }
        var error = new ProblemDetails
        {
            Detail = $"An unexpected error occured. {sb} ",
            Instance = context.Request.Path.Value,
            Status = (int)HttpStatusCode.InternalServerError,
            Type = exception.GetType().FullName,
        };
        await context.Response.WriteAsJsonAsync(error, cancellation);
    }
}