using System;
using Domain.Constants;
using Domain.Entities.FlightAggregate.Events;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Entities.FlightAggregate;

public class Flight : AggregateBase
{
    public string Origin { get; }
    public string Destination { get; }
    public DateTimeOffset Departure { get; private set; }
    public DateTimeOffset Arrival { get; private set; }
    public FlightStatus Status { get; private set;  }

    public Flight(string origin, string destination, DateTimeOffset departure, DateTimeOffset arrival, FlightStatus status)
    {
        Origin = string.IsNullOrWhiteSpace(origin) ?
                throw new DomainInvalidStateException(nameof(origin)) :
                origin;
        
        Destination = string.IsNullOrWhiteSpace(destination) ?
                throw new DomainInvalidStateException(nameof(destination)) :
                destination;
        
        Departure = departure == default ?
                throw new DomainInvalidStateException(nameof(departure)) :
                departure;
        
        Arrival = arrival == default ?
                throw new DomainInvalidStateException(nameof(arrival)) :
                arrival;
        
        Status = status == FlightStatus.Undefined ?
                throw new DomainInvalidStateException(nameof(status)) :
                status;

        if (Arrival > Departure)
        {
            throw new DomainInvalidStateException(ValidationMessages.ArrivalGreaterOrEqualsThanDeparture);
        }

        if (Origin == Destination)
        {
            throw new DomainInvalidStateException(ValidationMessages.OriginEqualsDestination);
        }
    }

    public void Delay(TimeSpan delayBy)
    {
        Departure = Departure.Add(delayBy);
        Arrival = Arrival.Add(delayBy);

        var previousStatus = Status;
        Status = FlightStatus.Delayed;
        
        NotificationsInternal.Enqueue(new FlightStatusChangedNotification(Status, previousStatus, this));
    }

    public void Cancel()
    {
        var previousStatus = Status;
        Status = FlightStatus.Cancelled;
        
        NotificationsInternal.Enqueue(new FlightStatusChangedNotification(Status, previousStatus, this));
    }
}
