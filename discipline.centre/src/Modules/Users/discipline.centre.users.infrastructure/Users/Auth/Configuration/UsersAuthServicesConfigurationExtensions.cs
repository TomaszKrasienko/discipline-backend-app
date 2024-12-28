using discipline.centre.users.infrastructure.Users.Auth;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.infrastructure.Users.Auth.Configuration;
using discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;
using discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class UsersAuthServicesConfigurationExtensions
{
    internal static IServiceCollection AddUsersAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<IAuthenticator, JwtAuthenticator>()
            .AddSingleton<IAuthorizationHandler, UserStateAuthorizationHandler>()
            .ValidateAndBind<JwtOptions, JwtOptionsValidator>(configuration);
}