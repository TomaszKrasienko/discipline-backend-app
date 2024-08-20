namespace discipline.application.Behaviours;

internal static class RefreshTokenPersistenceBehaviour
{
    
}

internal interface IRefreshTokenFacade
{
    Task Save(string refreshToken, Guid userId);
    Task<Guid?> GetUserId(string refreshToken);
}

internal sealed class RefreshTokenFacade(
    ) : IRefreshTokenFacade{
    public Task Save(string refreshToken, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Guid?> GetUserId(string refreshToken)
    {
        throw new NotImplementedException();
    }
}

internal interface IRefreshTokenService
{
    Task Save(string refreshToken, Guid userId);
}