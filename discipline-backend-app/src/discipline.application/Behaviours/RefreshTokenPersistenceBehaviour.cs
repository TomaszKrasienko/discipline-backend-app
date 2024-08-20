namespace discipline.application.Behaviours;

internal static class RefreshTokenPersistenceBehaviour
{
    
}

internal interface IRefreshToken
{
    Task Save(string refreshToken, Guid userId);
    Task<string> Is
}