using discipline.application.Behaviours;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestsPasswordManager : IPasswordManager
{
    public string Secure(string password)
        => password;

    public bool VerifyPassword(string securedPassword, string password)
        => securedPassword == password;
}