namespace discipline.application.Behaviours.Auth;

public interface IAuthenticator
{
    string CreateToken(string userId, string status);
}