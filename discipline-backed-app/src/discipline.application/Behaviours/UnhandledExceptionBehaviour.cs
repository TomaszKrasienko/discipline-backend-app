using discipline.application.Exceptions;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace discipline.application.Behaviours;

internal static class HandlingExceptionBehaviour
{
    internal static IServiceCollection AddHandlingException(this IServiceCollection services)
        => services.AddSingleton<HandlingExceptionMiddleware>();

    internal static WebApplication UseHandlingException(this WebApplication app)
    {
        app.UseMiddleware<HandlingExceptionMiddleware>();
        return app;
    }
}

internal sealed class HandlingExceptionMiddleware(
    ILogger<HandlingExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleErrorAsync(context, ex);
        }
    }


    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = exception switch
        {
            DisciplineException => (StatusCodes.Status400BadRequest, new ErrorDto(
                GetException(exception.GetType().Name), exception.Message)),
            _ => (StatusCodes.Status500InternalServerError, new ErrorDto("server_error",
                "There was an error"))
        };
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }

    private string GetException(string name)
        => name.Underscore().ToLower().Replace("_exception", "");
}

public sealed record ErrorDto(string Type, string Message);