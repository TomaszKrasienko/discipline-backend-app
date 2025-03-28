using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users;
using discipline.centre.users.infrastructure.Users.Passwords;
using Microsoft.AspNetCore.Identity;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class PasswordsServicesConfigurationExtensions
{
    internal static IServiceCollection AddPasswordsSecure(this IServiceCollection services)
        => services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();
}