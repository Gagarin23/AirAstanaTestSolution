using System;
using Application.Common.Constants;
using Domain.Entities.FlightAggregate;

namespace Application.Common.Extensions;

public static class FlightStatusExtensions
{
    public static Guid ToGuid(this FlightStatus status) =>
        status switch
        {
            FlightStatus.OnTime => FlightStatusConstants.InTimeId,
            FlightStatus.Delayed => FlightStatusConstants.DelayedId,
            FlightStatus.Cancelled => FlightStatusConstants.CancelledId,
            _ => Guid.Empty
        };
    
    public static FlightStatus ToFlightStatus(this Guid id)
    {
        if (id == FlightStatusConstants.InTimeId)
        {
            return FlightStatus.OnTime;
        }
        
        if (id == FlightStatusConstants.DelayedId)
        {
            return FlightStatus.Delayed;
        }
        
        if (id == FlightStatusConstants.CancelledId)
        {
            return FlightStatus.Cancelled;
        }

        return FlightStatus.Undefined;
    }
}