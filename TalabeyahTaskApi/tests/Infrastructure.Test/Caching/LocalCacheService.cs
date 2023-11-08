using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;
public class LocalCacheService : CacheService<TalabeyahTaskApi.Infrastructure.Caching.LocalCacheService>
{
    protected override TalabeyahTaskApi.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<TalabeyahTaskApi.Infrastructure.Caching.LocalCacheService>.Instance);
}