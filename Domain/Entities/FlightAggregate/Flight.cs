using System;
using Domain.Constants;
using Domain.Entities.FlightAggregate.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation.Results;

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
                throw new DomainInvalidStateException(nameof(Origin), ValidationMessages.DefaultValue) :
                origin;
        
        Destination = string.IsNullOrWhiteSpace(destination) ?
                throw new DomainInvalidStateException(nameof(Destination), ValidationMessages.DefaultValue) :
                destination;
        
        Departure = departure == default ?
                throw new DomainInvalidStateException(nameof(Departure), ValidationMessages.DefaultValue) :
                departure;
        
        Arrival = arrival == default ?
                throw new DomainInvalidStateException(nameof(Arrival), ValidationMessages.DefaultValue) :
                arrival;
        
        Status = status == FlightStatus.Undefined ?
                throw new DomainInvalidStateException(nameof(Status), ValidationMessages.DefaultValue) :
                status;

        if (Departure > Arrival)
        {
            throw new DomainInvalidStateException(nameof(Flight), ValidationMessages.DepartureGreaterOrEqualsThanArrival);
        }

        if (Origin == Destination)
        {
            throw new DomainInvalidStateException(nameof(Flight), ValidationMessages.OriginEqualsDestination);
        }
    }
    
    /// <summary>
    /// Возвращение в статус "Без задержек"
    /// </summary>
    /// <param name="delayDeparture">Опционально, для корректировки времени</param>
    /// <param name="delayArrival">Опционально, для корректировки времени</param>
    public void OnTime(TimeSpan delayDeparture = default, TimeSpan delayArrival = default)
    {
        OffsetDepartureAndArrival(delayDeparture, delayArrival);
        ChangeStatus(FlightStatus.OnTime);
    }

    public void Delayed(TimeSpan delayDeparture, TimeSpan delayArrival)
    {
        OffsetDepartureAndArrival(delayDeparture, delayArrival);
        ChangeStatus(FlightStatus.Delayed);
    }

    public void Cancel()
    {
        ChangeStatus(FlightStatus.Cancelled);
    }

    private void ChangeStatus(FlightStatus status)
    {
        var previousStatus = Status;
        Status = status;
        
        NotificationsInternal.Enqueue(new FlightStatusChangedNotification(Status, previousStatus, this));
    }

    private void OffsetDepartureAndArrival(TimeSpan delayDeparture, TimeSpan delayArrival)
    {
        Departure = Departure.Add(delayDeparture);
        Arrival = Arrival.Add(delayArrival);

        if (Departure > Arrival)
        {
            throw new DomainInvalidStateException(nameof(Flight), ValidationMessages.DepartureGreaterOrEqualsThanArrival);
        }
    }
}
