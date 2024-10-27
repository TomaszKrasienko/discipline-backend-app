using System.Drawing;
using discipline.application.Behaviours.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddBehaviours(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddCommandHandlingBehaviour()
            .AddHandlingException()
            .AddValidationBehaviour()
            .AddClockBehaviour()
            .AddLoggingBehaviour()
            .AddPasswordSecureBehaviour()
            .AddAuthBehaviour(configuration)
            .AddTokenStorage()
            .AddIdentityFromContextBehaviour()
            .AddUserStateCheckingBehaviour()
            .AddCryptographyBehaviour(configuration)
            .AddRefreshTokenBehaviour();

    internal static WebApplication UseBehaviours(this WebApplication app)
        => app
            .UseHandlingException()
            .UseAuthBehaviour()
            .UseUserStateCheckingBehaviour();

    internal static WebApplicationBuilder UseBehaviours(this WebApplicationBuilder builder)
        => builder
            .UseLoggingBehaviour();

    internal static T GetOptions<T>(this IConfiguration configuration, string section) where T : class, new()
    {
        var t = new T();
        configuration.Bind(section, t);
        return t;
    }
}