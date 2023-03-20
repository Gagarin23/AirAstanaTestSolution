using System;
using Application.Common.Interfaces;
using Domain.Entities.FlightAggregate;
using MediatR;

namespace Application.Flights.Commands.UpdateStatus;

public class UpdateFlightStatusCommand : ICommand<Unit>
{
    public Guid FlightId { get; init; }
    public FlightStatus Status { get; init; }
    public int DepartureOffsetInMinutes { get; init; }
    public int ArrivalOffsetInMinutes { get; init; }
}
