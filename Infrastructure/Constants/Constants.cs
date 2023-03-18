using System;

namespace Infrastructure.Constants;

public static class FlightStatusConstants
{
    public static readonly Guid InTimeId = new Guid("701ed6a9-e40b-479f-af89-82f2234bc62a");
    public static readonly Guid DelayedId = new Guid("711ed6a9-e40b-479f-af89-82f2234bc62a");
    public static readonly Guid CancelledId = new Guid("721ed6a9-e40b-479f-af89-82f2234bc62a");
}
