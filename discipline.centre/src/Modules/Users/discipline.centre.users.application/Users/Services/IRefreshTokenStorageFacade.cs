using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.application.Users.Services;

public interface IRefreshTokenStorageFacade
{
    Task<string> GenerateAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<UserId> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default);
}