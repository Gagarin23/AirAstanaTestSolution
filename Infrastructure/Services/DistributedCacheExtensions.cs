using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Services;

public static class DistributedCacheExtensions
{
    public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken)
        where T : ICacheable
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException(nameof(key));
        }
        
        var bytes = await cache.GetAsync(key, cancellationToken);
        return MessagePackSerializer.Deserialize<T>(bytes);
    }
    public static async Task SetAsync(this IDistributedCache cache, ICacheable value, CancellationToken cancellationToken)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        var bytes= MessagePackSerializer.Serialize(value);
        await cache.SetAsync(value.CacheKey, bytes, cancellationToken);
    }
}
