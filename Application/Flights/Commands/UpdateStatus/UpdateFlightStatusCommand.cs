using System;
using Domain.Entities.FlightAggregate;
using MediatR;

namespace Application.Flights.Commands.UpdateStatus;

public class UpdateFlightStatusCommand : IRequest<Unit>
{
    public Guid FlightId { get; init; }
    public FlightStatus Status { get; init; }
    public int DepartureOffsetInMinutes { get; init; }
    public int ArrivalOffsetInMinutes { get; init; }
}
