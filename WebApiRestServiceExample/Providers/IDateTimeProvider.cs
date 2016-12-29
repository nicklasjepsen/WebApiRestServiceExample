using System;

namespace WebApiRestServiceExample.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetTimeForTimeZone(string timeZoneName);
    }
}