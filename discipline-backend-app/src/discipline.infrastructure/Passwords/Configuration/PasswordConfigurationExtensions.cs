using discipline.application.Behaviours.Passwords;
using discipline.domain.Users;
using discipline.infrastructure.Passwords;
using Microsoft.AspNetCore.Identity;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class PasswordConfigurationExtensions
{
    internal static IServiceCollection AddPasswordManager(this IServiceCollection services)
        => services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();
}