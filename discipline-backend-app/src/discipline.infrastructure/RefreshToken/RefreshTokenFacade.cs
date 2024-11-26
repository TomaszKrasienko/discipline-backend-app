using System.Text;
using discipline.application.Behaviours;
using discipline.application.Behaviours.Cryptography;
using discipline.application.Behaviours.RefreshToken;
using discipline.application.Exceptions;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.infrastructure.RefreshToken;

internal sealed class RefreshTokenFacade : IRefreshTokenFacade
{
    private readonly ICryptographer _cryptographer;
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenFacade(
        ICryptographer cryptographer,
        IRefreshTokenService refreshTokenService)
    {
        _cryptographer = cryptographer;
        _refreshTokenService = refreshTokenService;
    }   
    
    public async Task<string> GenerateAsync(UserId userId, CancellationToken cancellationToken)
    {
        if (userId.IsEmpty())
        {
            throw new EmptyUserIdException();
        }
        
        //TODO: To appsettings
        var refreshToken = GenerateRandom(20);
        await _refreshTokenService.SaveOrReplaceAsync(refreshToken, userId, cancellationToken);
        return await _cryptographer.EncryptAsync(refreshToken, cancellationToken);
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

    public async Task<UserId> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var decryptedRefreshToken = await _cryptographer.DecryptAsync(refreshToken, cancellationToken);
        if (decryptedRefreshToken is null)
        {
            throw new InvalidRefreshTokenException();
        }
        var userId = await _refreshTokenService.GetAsync(decryptedRefreshToken, cancellationToken);
        if (userId is null)
        {
            throw new RefreshTokenForUserNotFoundException();
        }

        return userId;
    }
}