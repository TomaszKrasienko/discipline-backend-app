using discipline.centre.users.application.Users.Services;

namespace discipline.centre.users.e2e_tests.Helpers;

internal sealed class TestsPasswordManager : IPasswordManager
{
    public string Secure(string password)
        => password;

    public bool VerifyPassword(string securedPassword, string password)
        => securedPassword == password;
}