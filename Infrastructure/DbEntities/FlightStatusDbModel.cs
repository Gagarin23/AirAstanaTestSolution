using System;

namespace Infrastructure.DbEntities;

public class FlightStatusDbModel
{
    public Guid Id { get; internal set; }
    public string Name { get; internal set; }
}
