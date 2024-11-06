namespace discipline.centre.users.application.Users.Services;

public interface IAuthenticator
{
    string CreateToken(string userId, string email, string status);
}