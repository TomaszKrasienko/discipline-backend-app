using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace discipline.centre.shared.infrastructure.Cache;

internal sealed class CacheFacade(
    IDistributedCache distributedCache,
    ISerializer serializer,
    IClock clock) : ICacheFacade
{
    public async Task Add<T>(string key, T value, TimeSpan expiration) where T : class
    {
        await distributedCache.SetAsync(key, serializer.ToByteJson(value), new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = clock.DateTimeNow().Add(expiration) 
        });
    }
}