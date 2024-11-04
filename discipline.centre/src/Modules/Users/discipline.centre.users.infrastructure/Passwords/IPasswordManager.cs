namespace discipline.centre.users.infrastructure.Passwords;

public interface IPasswordManager
{
    string Secure(string password);
    bool VerifyPassword(string securedPassword, string password);
}