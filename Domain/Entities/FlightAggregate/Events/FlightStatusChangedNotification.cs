using MediatR;

namespace Domain.Entities.FlightAggregate.Events;

public class FlightStatusChangedNotification : INotification
{
    public FlightStatus NewStatus { get; }
    public FlightStatus PreviousStatus { get; }
    public Flight Flight { get; }

    public FlightStatusChangedNotification(FlightStatus newStatus, FlightStatus previousStatus, Flight flight)
    {
        NewStatus = newStatus;
        PreviousStatus = previousStatus;
        Flight = flight;
    }
}
