using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.infrastructure.Cache.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Cache;

internal sealed class CacheFacade(IDistributedCache distributedCache,
    IOptions<CacheOptions> options) : ICacheFacade
{
    public async Task Add<T>(string key, T value) where T : class
    {
        ///await distributedCache.SetAsync(key, )
    }
}