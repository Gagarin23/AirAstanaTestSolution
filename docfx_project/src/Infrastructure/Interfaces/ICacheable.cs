using System;
using Redis.OM.Contracts;
using Redis.OM.Modeling;

namespace Infrastructure.Interfaces;

public interface ICacheable : IRedisHydrateable
{
    public Ulid CacheKey { get; }
}
