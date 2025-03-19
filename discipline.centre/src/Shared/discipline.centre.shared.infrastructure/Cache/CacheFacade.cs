using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.Serialization;
using discipline.centre.shared.infrastructure.Cache.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Cache;

internal sealed class CacheFacade(
    IDistributedCache distributedCache,
    ISerializer serializer,
    IClock clock,
    IOptions<CacheOptions> options) : ICacheFacade
{
    private readonly TimeSpan _expire = options.Value.Expire;
    
    public async Task AddOrUpdateAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken) 
        where T : class
    {
        await distributedCache.SetAsync(key, serializer.ToByteJson(value), new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = clock.DateTimeNow().Add(expiration) 
        }, cancellationToken);
    }

    public Task AddOrUpdateAsync<T>(string key, T value, CancellationToken cancellationToken) where T : class
        => AddOrUpdateAsync(key, value, _expire, cancellationToken);

    public Task DeleteAsync(string key, CancellationToken cancellationToken)
        => distributedCache.RemoveAsync(key, cancellationToken);

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class
    {
        var result = await distributedCache.GetStringAsync(key, cancellationToken);

        return result is null
            ? null
            : serializer.ToObject<T>(result);
    }
}