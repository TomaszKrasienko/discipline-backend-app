using System.Text;
using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Auth.Configuration;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using Microsoft.Extensions.Options;

namespace discipline.centre.users.infrastructure.Users.RefreshToken;

internal sealed class RefreshTokenStorageFacade(
    ICacheFacade cacheFacade,
    IOptions<AuthOptions> options) : IRefreshTokenStorageFacade
{
    private readonly TimeSpan _expiry = options.Value.RefreshTokenExpiry;
    
    public async Task<string> GenerateAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = GenerateRandom(20);
        var dto = new RefreshTokenDto(refreshToken);
        await cacheFacade.AddAsync(userId.Value.ToString(), dto, _expiry);
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