using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.FlightAggregate;
using Infrastructure.DbEntities;
using Infrastructure.Interfaces;
using Mapster;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Infrastructure.Services.Flights;

public class FlightManager : IFlightManager
{
    private readonly IDatabaseContext _context;
    private readonly IRedisCollection<FlightDbModel> _flightCache;

    public FlightManager(IDatabaseContext context, IRedisConnectionProvider redisConnectionProvider)
    {
        _context = context;
        _flightCache = redisConnectionProvider.RedisCollection<FlightDbModel>();
    }

    public async ValueTask AddAsync(Flight flight, CancellationToken cancellationToken = default)
    {
        var flightDbModel = flight.Adapt<FlightDbModel>();

        _context.Flights.Add(flightDbModel);

        await _context.SaveChangesAsync(cancellationToken);

        await ActualizeCacheAsync(flightDbModel);
    }

    public async ValueTask UpdateAsync(Flight flight, CancellationToken cancellationToken = default)
    {
        var flightDbModel = new FlightDbModel()
        {
            Id = flight.Id
        };

        _context.Flights.Attach(flightDbModel);

        flight.Adapt(flightDbModel);

        await _context.SaveChangesAsync(cancellationToken);

        await ActualizeCacheAsync(flightDbModel);
    }

    public async ValueTask RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var flightDbModel = new FlightDbModel()
        {
            Id = id
        };

        _context.Flights.Remove(flightDbModel);

        await _context.SaveChangesAsync(cancellationToken);

        await RemoveFromCacheAsync(id);
    }

    private async ValueTask ActualizeCacheAsync(FlightDbModel flightDbModel)
    {
        var current = await _flightCache.FirstOrDefaultAsync(x => x.Id == flightDbModel.Id) ?? new FlightDbModel();

        flightDbModel.CacheKey = current.CacheKey;
        
        await _flightCache.InsertAsync(flightDbModel);
    }

    private async ValueTask RemoveFromCacheAsync(Guid id)
    {
        var current = await _flightCache.FirstOrDefaultAsync(x => x.Id == id) ?? new FlightDbModel();
        
        await _flightCache.DeleteAsync(current);
    }
}
