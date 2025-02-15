using Microsoft.AspNetCore.Diagnostics;

namespace Va.Developer.Assessment.Api.Helpers.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await HandleExceptionAsync(httpContext, exception, cancellationToken);
        return true;
    }
    private Task HandleExceptionAsync(HttpContext context, Exception exception, CancellationToken cancellation)
    {
        return Task.CompletedTask;
    }
}