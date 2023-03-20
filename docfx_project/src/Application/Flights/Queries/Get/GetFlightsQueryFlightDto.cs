using System;
using Domain.Entities.FlightAggregate;

namespace Application.Flights.Queries.Get;

public class GetFlightsQueryFlightDto
{
    public Guid Id { get; init; }
    public string Origin { get; init; }
    public string Destination { get; init; }
    public DateTimeOffset Departure { get; init; }
    public DateTimeOffset Arrival { get; init; }
    public FlightStatus Status { get; init; }
}
