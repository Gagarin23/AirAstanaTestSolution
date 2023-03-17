using System;
using Domain.Entities.FlightAggregate.Events;
using Domain.Interfaces;

namespace Domain.Entities.FlightAggregate;

public class Flight : AggregateBase, ICacheable
{
    public string Origin { get; }
    public string Destination { get; }
    public DateTimeOffset Departure { get; private set; }
    public DateTimeOffset Arrival { get; private set; }
    public FlightStatus Status { get; private set;  }

    public Flight(string origin, string destination, DateTimeOffset departure, DateTimeOffset arrival, FlightStatus status)
    {
        Origin = origin;
        Destination = destination;
        Departure = departure;
        Arrival = arrival;
        Status = status;
    }

    #region System

    public string CacheKey => Id.ToString();
    

    #endregion

    public void Delay(TimeSpan delayBy)
    {
        NotificationsInternal.Enqueue(new FlightStatusChangedNotification(FlightStatus.Delayed, Status, this));
        Departure = Departure.Add(delayBy);
        Arrival = Arrival.Add(delayBy);
        Status = FlightStatus.Delayed;
    }

    public void Cancel()
    {
        NotificationsInternal.Enqueue(new FlightStatusChangedNotification(FlightStatus.Cancelled, Status, this));
        Status = FlightStatus.Cancelled;
    }

    internal Flight()
    {

    }
}
