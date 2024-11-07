using discipline.centre.users.application.Users.Services;
using discipline.centre.users.infrastructure.Users.TokenStorage;

// ReSharper disable once CheckNamespace
namespace  Microsoft.Extensions.DependencyInjection;

internal static class TokenStorageServicesConfigurationExtensions
{
    internal static IServiceCollection AddTokenStorage(this IServiceCollection services)
        => services
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddHttpContextAccessor();
}