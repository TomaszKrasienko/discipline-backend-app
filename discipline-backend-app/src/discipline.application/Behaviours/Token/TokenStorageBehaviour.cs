using discipline.application.DTOs;

namespace discipline.application.Behaviours.Token;

public interface ITokenStorage
{
    void Set(TokensDto jwtDto);
    TokensDto? Get();
}

