using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.FlightAggregate;
using MediatR;

namespace Application.Flights.Commands.UpdateStatus;

public class UpdateFlightStatusCommandHandler : IRequestHandler<UpdateFlightStatusCommand, Unit>
{
    private readonly IReadonlyFlightManager _readonlyFlightManager;
    private readonly IFlightManager _flightManager;

    public UpdateFlightStatusCommandHandler(IReadonlyFlightManager readonlyFlightManager, IFlightManager flightManager)
    {
        _readonlyFlightManager = readonlyFlightManager;
        _flightManager = flightManager;
    }
    
    public async Task<Unit> Handle(UpdateFlightStatusCommand request, CancellationToken cancellationToken)
    {
        var flights = await _readonlyFlightManager.GetSingleOrDefaultAsync(request.FlightId, cancellationToken);

        switch (request.Status)
        {
            case FlightStatus.Delayed:
                flights.Delayed
                (
                    TimeSpan.FromMinutes(request.DepartureOffsetInMinutes),
                    TimeSpan.FromMinutes(request.ArrivalOffsetInMinutes)
                );
                break;
            
            case FlightStatus.OnTime:
                flights.OnTime
                (
                    TimeSpan.FromMinutes(request.DepartureOffsetInMinutes),
                    TimeSpan.FromMinutes(request.ArrivalOffsetInMinutes)
                );
                break;
            
            case FlightStatus.Cancelled:
                flights.Cancel();
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(request.Status));
        }

        await _flightManager.UpdateAsync(flights, cancellationToken);
        
        return Unit.Value;
    }
}
