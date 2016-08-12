using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRestServiceExample.Providers
{
    public class DateTimeProvider
    {
        public DateTime GetTimeForTimeZone(string timeZoneName)
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now,
                TimeZoneInfo.FindSystemTimeZoneById(timeZoneName));
        }
    }
}