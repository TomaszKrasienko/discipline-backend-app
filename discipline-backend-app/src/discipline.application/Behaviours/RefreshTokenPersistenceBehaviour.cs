namespace discipline.application.Behaviours;

internal static class RefreshTokenPersistenceBehaviour
{
    
}

internal interface IRefreshTokenFacade
{
    Task Save(string refreshToken, Guid userId);
    Task<Guid?> GetUserId(string refreshToken);
}