using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users;
using Microsoft.AspNetCore.Identity;

namespace discipline.centre.users.infrastructure.Users.Passwords;

internal sealed class PasswordManager(
    IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(default!, password);

    public bool VerifyPassword(string securedPassword, string password)
        => passwordHasher
            .VerifyHashedPassword(default!, securedPassword, password) == PasswordVerificationResult.Success;
}