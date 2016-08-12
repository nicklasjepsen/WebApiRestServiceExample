using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using WebApiRestServiceExample.Models;
using WebApiRestServiceExample.Providers;

namespace WebApiRestServiceExample.Controllers
{
    public class CalendarController : ApiController
    {
        private readonly GoogleMapsProvider googleMapsProvider;
        private readonly DateTimeProvider dateTimeProvider;

        public CalendarController()
        {
            googleMapsProvider = new GoogleMapsProvider();
            dateTimeProvider = new DateTimeProvider();
        }

        [HttpGet]
        [Route("Calendar/{city}/LocalTime")]
        public async Task<IHttpActionResult> GetTime(string city)
        {
            return await GetLocalTimeInternal(city, string.Empty);
        }

        [HttpGet]
        [Route("Calendar/{city}/{country}/LocalTime")]
        public async Task<IHttpActionResult> GetTime(string city, string country)
        {
            return await GetLocalTimeInternal(city, country);
        }

        private async Task<IHttpActionResult> GetLocalTimeInternal(string city, string country)
        {
            var timeZoneName = await googleMapsProvider.GetTimeZoneName(city, country);
            if (string.IsNullOrEmpty(timeZoneName))
                return NotFound();

            var localTime = dateTimeProvider.GetTimeForTimeZone(timeZoneName);

            return Ok(new LocalTimeModel
            {
                TimeZoneName = timeZoneName,
                UtcOffSet = Math.Round((localTime.ToUniversalTime() - DateTime.UtcNow).TotalHours, 2),
                LocalTime = localTime
            });
        }
    }
}
