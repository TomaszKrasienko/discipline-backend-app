using System.Text;
using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Auth.Configuration;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;
using discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;
using Microsoft.Extensions.Options;

namespace discipline.centre.users.infrastructure.Users.RefreshToken;

internal sealed class RefreshTokenFacade(
    ICacheFacade cacheFacade,
    IOptions<RefreshTokenOptions> options) : IRefreshTokenFacade
{
    private readonly TimeSpan _expiry = options.Value.Expiry;
    private readonly int _refreshTokenLength = options.Value.Length;
    
    public async Task<string> GenerateAndSaveAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = GenerateRandom(_refreshTokenLength);
        var dto = new RefreshTokenDto(refreshToken);
        await cacheFacade.AddOrUpdateAsync(userId.Value.ToString(), dto, _expiry, cancellationToken);
        return refreshToken;
    }

    private static string GenerateRandom(int length)
    {
        Random random = new Random();
        StringBuilder refreshTokenSb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            refreshTokenSb.Append((char)random.Next('A', 'z'));
        }
        return refreshTokenSb.ToString();
    }
    
    public Task<UserId> GetUserIdAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}