using discipline.application.DTOs;

namespace discipline.application.Behaviours;

public class TokenStorageBehaviour
{
    
}

public interface ITokenStorage
{
    void Set(JwtDto jwtDto);
}