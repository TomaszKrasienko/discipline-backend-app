using discipline.application.DTOs;

namespace discipline.application.Behaviours;

public interface ITokenStorage
{
    void Set(TokensDto jwtDto);
    TokensDto? Get();
}

