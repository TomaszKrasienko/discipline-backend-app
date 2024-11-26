using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.application.Behaviours.RefreshToken;

public interface IRefreshTokenFacade
{
    Task<string> GenerateAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<UserId> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default);
}