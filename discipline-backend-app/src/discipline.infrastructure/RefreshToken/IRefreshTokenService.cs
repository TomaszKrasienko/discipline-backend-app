using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.infrastructure.RefreshToken;

internal interface IRefreshTokenService
{
    Task SaveOrReplaceAsync(string refreshToken, UserId userId, CancellationToken cancellationToken = default);
    Task<UserId> GetAsync(string refreshToken, CancellationToken cancellationToken = default);
}