namespace discipline.application.Behaviours.Passwords;

public interface IPasswordManager
{
    string Secure(string password);
    bool VerifyPassword(string securedPassword, string password);
}
