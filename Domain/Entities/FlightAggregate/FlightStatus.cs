using System;

namespace Domain.Entities.FlightAggregate;

public enum FlightStatus
{
    Undefined = 0,
    InTime = 1,
    Delayed = 2,
    Cancelled = 3
}

public static class FlightStatusExtensions
{
    public static string StatusToString(this FlightStatus status) =>
        status switch
        {
            FlightStatus.Undefined => nameof(FlightStatus.Undefined),
            FlightStatus.InTime => nameof(FlightStatus.InTime),
            FlightStatus.Delayed => nameof(FlightStatus.Delayed),
            FlightStatus.Cancelled => nameof(FlightStatus.Cancelled),
            _ => throw new ArgumentOutOfRangeException
            (
                nameof(status),
                status, 
                null
            )
        };
    
    public static FlightStatus ParseStatus(this string stringStatus)
    {
        return stringStatus switch
        {
            nameof(FlightStatus.Undefined) => FlightStatus.Undefined,
            nameof(FlightStatus.InTime) => FlightStatus.InTime,
            nameof(FlightStatus.Delayed) => FlightStatus.Delayed,
            nameof(FlightStatus.Cancelled) => FlightStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException
            (
                nameof(stringStatus),
                stringStatus,
                null
            )
        };
    }
}