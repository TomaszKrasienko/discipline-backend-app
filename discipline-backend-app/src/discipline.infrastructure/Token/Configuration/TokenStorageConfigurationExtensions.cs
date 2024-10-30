using discipline.application.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.infrastructure.Token.Configuration;

internal static class TokenStorageConfigurationExtensions
{
    internal static IServiceCollection AddTokenStorage(this IServiceCollection services)
        => services
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddHttpContextAccessor();
}