using System;
using Domain.Entities.FlightAggregate;
using MediatR;

namespace Application.Flights.Commands.Add;

public class AddFlightCommand : IRequest<Guid>
{
    public string Origin { get; init; }
    public string Destination { get; init; }
    public DateTimeOffset Departure { get; init; }
    public DateTimeOffset Arrival { get; init; }
    public FlightStatus Status { get; init; }
}
