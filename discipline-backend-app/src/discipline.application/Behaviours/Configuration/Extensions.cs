using System.Drawing;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Behaviours.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddBehaviours(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddCqrs()
            .AddValidationBehaviour()
            .AddLoggingBehaviour()
            .AddPasswordSecureBehaviour()
            .AddTokenStorage()
            .AddIdentityFromContextBehaviour()
            .AddUserStateCheckingBehaviour()
            .AddRefreshTokenBehaviour();

    internal static WebApplication UseBehaviours(this WebApplication app)
        => app
            .UseUserStateCheckingBehaviour();

    internal static WebApplicationBuilder UseBehaviours(this WebApplicationBuilder builder)
        => builder
            .UseLoggingBehaviour();
}