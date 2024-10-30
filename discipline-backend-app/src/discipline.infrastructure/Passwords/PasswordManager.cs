using discipline.application.Behaviours;
using discipline.application.Behaviours.Passwords;
using discipline.domain.Users;
using Microsoft.AspNetCore.Identity;

namespace discipline.infrastructure.Passwords;

internal sealed class PasswordManager(
    IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(default!, password);

    public bool VerifyPassword(string securedPassword, string password)
        => passwordHasher
            .VerifyHashedPassword(default!, securedPassword, password) == PasswordVerificationResult.Success;
}