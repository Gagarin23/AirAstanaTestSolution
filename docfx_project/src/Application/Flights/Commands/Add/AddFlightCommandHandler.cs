using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.FlightAggregate;
using Mapster;
using MediatR;

namespace Application.Flights.Commands.Add;

public class AddFlightCommandHandler : IRequestHandler<AddFlightCommand, Guid>
{
    private readonly IFlightManager _flightManager;

    public AddFlightCommandHandler(IFlightManager flightManager)
    {
        _flightManager = flightManager;
    }
    
    public async Task<Guid> Handle(AddFlightCommand request, CancellationToken cancellationToken)
    {
        var flight = request.Adapt<Flight>();
        return await _flightManager.AddAsync(flight, cancellationToken);
    }
}
