using System.ComponentModel;

namespace Common.Core.Enum;

public enum DRBACType
{
    NONE = 0,
    DEFAULT = 1,
    COMPANY = 2,
    BUSINESSUNIT = 3,
    BRANCH = 4,
    LOCATION = 5
}

public enum EventMethodType
{
    CREATE = 1,
    UPDATE = 2,
    DELETE = 3,
}

public enum HTTPMethodType
{
    POST = 1,
    GET = 2,
    PUT = 3,
    DELETE = 4
}

public enum JSONEngineType
{
    /// <summary>
    /// Default, using System.Text.Json
    /// </summary>
    DEFAULT = 0,
    /// <summary>
    /// Using Utf8Json Nuget package
    /// </summary>
    UTF8JSON = 11
}

public enum NotificationPlatform
{
    WEB = 1,
    MOBILE = 2
}

public enum NotificationType
{
    SYSTEM = 1,
    ACTIVITY = 2
}

public enum NotificationStatus
{
    INPROGRESS = 1,
    SUCCESS = 2,
    FAILED = 0
}

public enum OrderType
{
    ASC = 0,
    DESC = 1,
}