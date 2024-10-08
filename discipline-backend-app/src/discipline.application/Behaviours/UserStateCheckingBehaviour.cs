using discipline.domain.Users.Repositories;
using discipline.domain.Users.ValueObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class UserStateCheckingBehaviour
{
    internal const string UserStatePolicyName = "UserStatePolicy";
    internal static IServiceCollection AddUserStateCheckingBehaviour(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(UserStatePolicyName, policy =>
            {
                policy.Requirements.Add(new UserStateRequirement());
            });
        });
        services
            .AddSingleton<IAuthorizationHandler, UserStateAuthorizationHandler>();
        return services;
    }

    internal static WebApplication UseUserStateCheckingBehaviour(this WebApplication app)
    {
        //app.UseMiddleware<UserStateMiddleware>();
        return app;
    }
}

public class UserStateRequirement : IAuthorizationRequirement;

internal sealed class UserStateAuthorizationHandler(
    IServiceProvider serviceProvider) : AuthorizationHandler<UserStateRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserStateRequirement requirement)
    {
        if (!(Guid.TryParse(context?.User?.Identity?.Name, out var userId)))
        {
            context.Succeed(requirement);
            return;
        }

        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var user = await userRepository.GetByIdAsync(userId);
        if (!user.IsUserActive())
        {
            context.Fail();
            return;
        }
        context.Succeed(requirement);
    }
}

internal sealed class UserStateMiddleware(
    IServiceProvider serviceProvider) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!(context.User?.Identity?.IsAuthenticated) ?? true)
        { 
            await next(context);
            return;
        }

        if (!(Guid.TryParse(context?.User?.Identity?.Name, out var userId)))
        {
            await next(context);
            return;
        }

        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        
        //TODO: Tests + remove magic string
        if (user?.Status.Value == Status.Created().Value 
            && context.Request.Path != "/users/subscriptions"
            && context.Request.Path != "/users/subscriptions")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            var errorDto = new ErrorDto("create_user_state", "User have to have picked subscription");
            return;
        }
        await next(context);
    }
}

