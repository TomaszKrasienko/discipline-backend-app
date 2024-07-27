using discipline.application.Domain.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class PasswordSecureBehaviour
{
    internal static IServiceCollection AddPasswordSecureBehaviour(this IServiceCollection services)
        => services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();
}

internal interface IPasswordManager
{
    string Secure(string password);
    bool VerifyPassword(string securedPassword, string password);
}

internal sealed class PasswordManager(
    IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(default!, password);

    public bool VerifyPassword(string securedPassword, string password)
        => passwordHasher
            .VerifyHashedPassword(default!, securedPassword, password) == PasswordVerificationResult.Success;
}