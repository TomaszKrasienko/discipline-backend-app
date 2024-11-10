using discipline.centre.users.application.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;

internal static class RefreshTokenStorageServicesConfigurationExtensions
{
    internal static IServiceCollection AddRefreshTokenStorage(this IServiceCollection services)
        => services.AddSingleton<IRefreshTokenFacade, RefreshTokenFacade>();
}