using System.Text;
using discipline.application.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class RefreshTokenBehaviour
{
    internal static IServiceCollection AddRefreshTokenBehaviour(this IServiceCollection services)
        => services
            .AddSingleton<IRefreshTokenFacade, RefreshTokenFacade>()
            .AddSingleton<IRefreshTokenService, RefreshTokenService>();
}

internal interface IRefreshTokenFacade
{
    Task<string> GenerateAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default);
}

internal sealed class RefreshTokenFacade(
    ICryptographer cryptographer,
    IRefreshTokenService refreshTokenService) : IRefreshTokenFacade {

    public async Task<string> GenerateAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            throw new EmptyUserIdException();
        }
        
        //TODO: To appsettings
        var refreshToken = GenerateRandom(20);
        await refreshTokenService.SaveOrReplaceAsync(refreshToken, userId, cancellationToken);
        return await cryptographer.EncryptAsync(refreshToken, cancellationToken);
    }

    private string GenerateRandom(int length)
    {
        Random random = new Random();
        StringBuilder refreshTokenSb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            refreshTokenSb.Append((char)random.Next('A', 'z'));
        }
        return refreshTokenSb.ToString();
    }

    public async Task<Guid> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var decryptedRefreshToken = await cryptographer.DecryptAsync(refreshToken, cancellationToken);
        var userId = await refreshTokenService.GetAsync(decryptedRefreshToken, cancellationToken);
        if (userId is null)
        {
            throw new RefreshTokenForUserNotFoundException();
        }

        return userId.Value;
    }
}

internal interface IRefreshTokenService
{
    Task SaveOrReplaceAsync(string refreshToken, Guid userId, CancellationToken cancellationToken = default);
    Task<Guid?> GetAsync(string refreshToken, CancellationToken cancellationToken = default);
}

internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly Dictionary<Guid, string> _dictionary = new Dictionary<Guid, string>();
    
    public Task SaveOrReplaceAsync(string refreshToken, Guid userId, CancellationToken cancellationToken = default)
    {
        if (_dictionary.Any(x => x.Key == userId))
        {
            _dictionary.Remove(userId);
        }
        _dictionary.Add(userId, refreshToken);
        return Task.CompletedTask;
    }

    public Task<Guid?> GetAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (_dictionary.All(x => x.Value != refreshToken))
        {
            return null;
        }

        return Task.FromResult((Guid?)_dictionary.First(x => x.Value == refreshToken).Key);
    }
}
