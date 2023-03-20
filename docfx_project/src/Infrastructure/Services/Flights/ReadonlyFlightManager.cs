using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities.FlightAggregate;
using Domain.Specifications;
using Infrastructure.DbEntities;
using Infrastructure.Interfaces;
using JetBrains.Annotations;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Infrastructure.Services.Flights;

public class ReadonlyFlightManager : IReadonlyFlightManager
{
    private readonly IReadonlyDatabaseContext _context;
    private readonly IRedisCollection<FlightDbModel> _flightCache;

    public ReadonlyFlightManager(IReadonlyDatabaseContext context, IRedisConnectionProvider redisConnectionProvider)
    {
        _context = context;
        _flightCache = redisConnectionProvider.RedisCollection<FlightDbModel>();
    }

    public async ValueTask<Flight> GetSingleOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbModel = await _flightCache.FirstOrDefaultAsync(x => x.Id == id);

        if (dbModel != null)
        {
            return dbModel.Adapt<Flight>();
        }

        dbModel = await _context.Flights.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return dbModel?.Adapt<Flight>();
    }

    public async ValueTask<List<Flight>> GetAsync(IDictionary<string, string> filters, CancellationToken cancellationToken = default)
    {
        var isFilterBuilt = TryBuildFilter(filters, out var expression);

        var cacheIterator = 
            (isFilterBuilt ? _flightCache.Where(expression) : _flightCache)
            .OrderByDescending(x => x.Arrival)
            .AsAsyncEnumerable();

        var flights = await GetFlightsAsync(cacheIterator, cancellationToken);

        if (flights.Any())
        {
            return flights;
        }

        var dbIterator = 
            (isFilterBuilt ? _context.Flights.Where(expression) : _context.Flights)
            .OrderByDescending(x => x.Arrival)
            .AsAsyncEnumerable();

        return await GetFlightsAsync(dbIterator, cancellationToken);
    }

    private bool TryBuildFilter(IDictionary<string, string> filters, out Specification<FlightDbModel> filter)
    {
        filter = default;

        const string originKey = "origin";
        const string destinationKey = "destination";

        var isOriginFilterExists = filters.TryGetValue(originKey, out var originFilterValue);
        var isDestinationFilterExists = filters.TryGetValue(destinationKey, out var destinationFilterValue);
        
        if (isOriginFilterExists)
        {
            filter = FlightDbModel.OriginFilter(originFilterValue);
        }
        
        if (isDestinationFilterExists)
        {
            filter = filter.Expression == null ?
                FlightDbModel.DestinationFilter(destinationFilterValue) :
                filter && FlightDbModel.DestinationFilter(destinationFilterValue);
        }

        return isOriginFilterExists || isDestinationFilterExists;
    }

    private async ValueTask<List<Flight>> GetFlightsAsync(IAsyncEnumerable<FlightDbModel> asyncIterator, CancellationToken cancellationToken)
    {
        var flights = new List<Flight>();

        await foreach (var flightModel in asyncIterator.WithCancellation(cancellationToken))
        {
            flights.Add(flightModel.Adapt<Flight>());
        }

        return flights;
    }
}
