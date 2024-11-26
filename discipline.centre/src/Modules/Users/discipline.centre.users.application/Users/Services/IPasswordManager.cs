namespace discipline.centre.users.application.Users.Services;

public interface IPasswordManager
{
    string Secure(string password);
    bool VerifyPassword(string securedPassword, string password);
}