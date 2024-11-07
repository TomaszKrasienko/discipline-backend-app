using discipline.centre.users.application.Users.DTOs;

namespace discipline.centre.users.application.Users.Services;

public interface ITokenStorage
{
    void Set(TokensDto jwtDto);
    TokensDto Get();
}