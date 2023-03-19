using System;

namespace Application.Common.Constants;

public static class FlightStatusConstants
{
    public static readonly Guid InTimeId = new Guid("701ed6a9-e40b-479f-af89-82f2234bc62a");
    public static readonly Guid DelayedId = new Guid("711ed6a9-e40b-479f-af89-82f2234bc62a");
    public static readonly Guid CancelledId = new Guid("721ed6a9-e40b-479f-af89-82f2234bc62a");
}

public static class AuthConstants
{
    public const string ModeratorRoleId = "62b7cfd9-d0b8-4300-baf9-0540a0f817c7";
    public const string ModeratorRoleName = "Moderator";
    public const string UserRoleId = "d25993c3-bbbc-440f-b7f0-6811e83a9f48";
    public const string UserRoleName = "User";
}
