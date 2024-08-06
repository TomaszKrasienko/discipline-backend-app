using discipline.application.Domain.Users.Repositories;
using discipline.application.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class UserStateCheckingBehaviour
{
    internal static IServiceCollection AddUserStateCheckingBehaviour(this IServiceCollection services)
        => services
            .AddSingleton<UserStateMiddleware>();

    internal static WebApplication UseUserStateCheckingBehaviour(this WebApplication app)
    {
        app.UseMiddleware<UserStateMiddleware>();
        return app;
    }
}

internal sealed class UserStateMiddleware(
    IUserRepository userRepository) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User?.Identity?.IsAuthenticated ?? false)
        {
            await next(context);
        }

        if (Guid.TryParse(context?.User?.Identity?.Name, out var userId))
        {
            await next(context);
        }

        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else if (user?.Status == Status.Created())
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
    }
} 