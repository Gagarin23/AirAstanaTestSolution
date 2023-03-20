using System;

namespace Domain.Entities.FlightAggregate;

public enum FlightStatus
{
    Undefined = 0,
    OnTime = 1,
    Delayed = 2,
    Cancelled = 3
}