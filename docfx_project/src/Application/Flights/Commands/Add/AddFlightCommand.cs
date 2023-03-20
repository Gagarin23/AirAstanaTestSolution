using System;
using Application.Common.Interfaces;
using Domain.Entities.FlightAggregate;
using MediatR;

namespace Application.Flights.Commands.Add;

public class AddFlightCommand : ICommand<Guid>
{
    public string Origin { get; init; }
    public string Destination { get; init; }
    public DateTimeOffset Departure { get; init; }
    public DateTimeOffset Arrival { get; init; }
    public FlightStatus Status { get; init; }
}
