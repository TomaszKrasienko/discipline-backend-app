using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.infrastructure.Cache;
using discipline.centre.shared.infrastructure.Cache.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DistributedCacheServicesConfigurationExtensions
{
    internal static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndBind<RedisCacheOptions, RedisCacheOptionsValidator>(configuration)
            .AddSingleton<ICacheFacade, CacheFacade>()
            .AddStackExchangeRedisCache(redisOptions =>
            {
                var redisCacheOptions = services.GetOptions<RedisCacheOptions>();
                redisOptions.Configuration = redisCacheOptions.ConnectionString;
            });
}