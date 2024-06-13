using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddBehaviours(this IServiceCollection services)
        => services
            .AddHandlingException()
            .AddValidationBehaviour()
            .AddCreatingTransaction();

    internal static WebApplication UseBehaviours(this WebApplication app)
        => app
            .UseHandlingException();
}