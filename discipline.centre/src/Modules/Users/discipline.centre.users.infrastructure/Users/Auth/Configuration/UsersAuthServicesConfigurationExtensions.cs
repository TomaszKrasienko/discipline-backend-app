using discipline.centre.users.infrastructure.Users.Auth;
using discipline.centre.users.application.Users.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class UsersAuthServicesConfigurationExtensions
{
    internal static IServiceCollection AddUsersAuth(this IServiceCollection services)
        => services.AddSingleton<IAuthenticator, JwtAuthenticator>();
}