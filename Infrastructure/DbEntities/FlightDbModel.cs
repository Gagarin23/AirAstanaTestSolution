using System;

namespace Infrastructure.DbEntities;

public class FlightDbModel
{
    public Guid Id { get; internal set; }
    public string Origin { get; internal set; }
    public string Destination { get; internal set; }
    public DateTimeOffset Departure { get; internal set; }
    public DateTimeOffset Arrival { get; internal set; }
    public Guid StatusId { get; internal set; }
    public FlightStatusDbModel Status { get; internal set; }

    internal FlightDbModel()
    {
        
    }
}
