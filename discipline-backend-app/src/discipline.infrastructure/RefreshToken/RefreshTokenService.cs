using discipline.application.Exceptions;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.infrastructure.RefreshToken;

internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly Dictionary<UserId, string> _dictionary = new Dictionary<UserId, string>();
    
    public Task SaveOrReplaceAsync(string refreshToken, UserId userId, CancellationToken cancellationToken = default)
    {
        if (_dictionary.Any(x => x.Key == userId))
        {
            _dictionary.Remove(userId);
        }
        _dictionary.Add(userId, refreshToken);
        return Task.CompletedTask;
    }

    public Task<UserId> GetAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (_dictionary.All(x => x.Value != refreshToken))
        {
            return null;
        }

        var userId = _dictionary.First(x => x.Value == refreshToken).Key;
        if (userId.IsEmpty())
        {
            throw new EmptyUserIdException();
        }
        
        return Task.FromResult(userId);
    }
}