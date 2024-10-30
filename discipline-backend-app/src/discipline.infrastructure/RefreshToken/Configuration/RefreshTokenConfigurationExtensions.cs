using discipline.application.Behaviours.RefreshToken;
using discipline.infrastructure.RefreshToken;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class RefreshTokenConfigurationExtensions
{
    internal static IServiceCollection AddRefreshToken(this IServiceCollection services)
        => services
            .AddSingleton<IRefreshTokenFacade, RefreshTokenFacade>()
            .AddSingleton<IRefreshTokenService, RefreshTokenService>();
}