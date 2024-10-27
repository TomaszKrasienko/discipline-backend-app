namespace discipline.application.Behaviours;

public interface IAuthenticator
{
    string CreateToken(string userId, string status);
}