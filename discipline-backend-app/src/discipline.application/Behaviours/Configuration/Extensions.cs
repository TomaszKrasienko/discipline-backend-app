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
            .AddCreatingTransaction()
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
}