using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.application.Users.Services;

public interface IRefreshTokenFacade
{
    Task<string> GenerateAndSaveAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<UserId> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default);
}