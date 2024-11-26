using discipline.application.Behaviours;
using discipline.application.Behaviours.Passwords;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestsPasswordManager : IPasswordManager
{
    public string Secure(string password)
        => password;

    public bool VerifyPassword(string securedPassword, string password)
        => securedPassword == password;
}