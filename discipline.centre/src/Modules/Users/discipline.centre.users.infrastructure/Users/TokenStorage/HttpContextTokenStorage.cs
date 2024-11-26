using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using Microsoft.AspNetCore.Http;

namespace discipline.centre.users.infrastructure.Users.TokenStorage;

public sealed class HttpContextTokenStorage(
    IHttpContextAccessor httpContextAccessor) : ITokenStorage
{
    private const string TokenKey = "user_jwt_token";

    public void Set(TokensDto jwtDto)
        => httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwtDto);

    public TokensDto? Get()
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwtDto))
        {
            return jwtDto as TokensDto;
        }

        return null;
    }
}