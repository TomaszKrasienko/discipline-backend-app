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
    void Set(JwtDto jwtDto);
    JwtDto Get();
}

public sealed class HttpContextTokenStorage(
    IHttpContextAccessor httpContextAccessor) : ITokenStorage
{
    private const string TokenKey = "user_jwt_token";

    public void Set(JwtDto jwtDto)
        => httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwtDto);

    public JwtDto Get()
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwtDto))
        {
            return jwtDto as JwtDto;
        }

        return null;
    }
}