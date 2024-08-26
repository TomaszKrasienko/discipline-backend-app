using discipline.application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class TokenStorageBehaviour
{
    internal static IServiceCollection AddTokenStorage(this IServiceCollection services)
        => services
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddHttpContextAccessor();
}

public interface ITokenStorage
{
    void Set(TokensDto jwtDto);
    TokensDto Get();
}

public sealed class HttpContextTokenStorage(
    IHttpContextAccessor httpContextAccessor) : ITokenStorage
{
    private const string TokenKey = "user_jwt_token";

    public void Set(TokensDto jwtDto)
        => httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwtDto);

    public TokensDto Get()
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