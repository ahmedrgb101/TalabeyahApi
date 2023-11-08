using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TalabeyahTaskApi.Infrastructure.Common.Services;

namespace Infrastructure.Test.Caching;
public class DistributedCacheService : CacheService<TalabeyahTaskApi.Infrastructure.Caching.DistributedCacheService>
{
    protected override TalabeyahTaskApi.Infrastructure.Caching.DistributedCacheService CreateCacheService() =>
        new(
            new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())),
            new NewtonSoftService(),
            NullLogger<TalabeyahTaskApi.Infrastructure.Caching.DistributedCacheService>.Instance);
}