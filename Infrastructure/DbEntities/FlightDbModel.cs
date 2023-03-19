using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Specifications;
using Infrastructure.Interfaces;
using Redis.OM.Modeling;

namespace Infrastructure.DbEntities;

[Document(StorageType = StorageType.Hash, Prefixes = new []{nameof(FlightDbModel)})]
public class FlightDbModel : ICacheable
{
    [RedisIdField, NotMapped] public Ulid CacheKey { get;  internal set; }
    [Indexed] public Guid Id { get; internal set; }
    [Indexed] public string Origin { get; internal set; }
    [Indexed] public string Destination { get; internal set; }
    [Indexed] public DateTimeOffset Departure { get; internal set; }
    [Indexed(Sortable = true)] public DateTimeOffset Arrival { get; internal set; }
    [Indexed] public Guid StatusId { get; internal set; }
    public FlightStatusDbModel Status { get; internal set; }

    public static Specification<FlightDbModel> OriginFilter(string value) => 
        new Specification<FlightDbModel>(flight => flight.Origin == value);

    public static Specification<FlightDbModel> DestinationFilter(string value) => 
        new Specification<FlightDbModel>(flight => flight.Destination == value);

    public void Hydrate(IDictionary<string, string> dict)
    {
        Id = new Guid(dict[nameof(Id)]);
        CacheKey = Ulid.Parse(dict[nameof(CacheKey)]);
        Origin = dict[nameof(Origin)];
        Destination = dict[nameof(Destination)];
        Departure = DateTimeOffset.Parse(dict[nameof(Departure)]);
        Arrival = DateTimeOffset.Parse(dict[nameof(Arrival)]);
        StatusId = new Guid(dict[nameof(StatusId)]);
    }

    public IDictionary<string, string> BuildHashSet()
    {
        CacheKey = CacheKey = default ?
            Ulid.NewUlid() :
            CacheKey;
        
        return new Dictionary<string, string>()
        {
            { nameof(Id), Id.ToString() },
            { nameof(CacheKey), CacheKey.ToString() },
            { nameof(Origin), Origin },
            { nameof(Destination), Destination },
            { nameof(Departure), Departure.ToString() },
            { nameof(Arrival), Arrival.ToString() },
            { nameof(StatusId), StatusId.ToString() },
        };
    }
}
