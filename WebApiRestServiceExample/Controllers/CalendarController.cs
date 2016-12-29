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
        private readonly IGoogleMapsProvider googleMapsProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        public CalendarController(IGoogleMapsProvider googleMapsProvider, IDateTimeProvider dateTimeProvider)
        {
            this.googleMapsProvider = googleMapsProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        [HttpGet]
        [Route("Calendar/{city}/LocalTime")]
        public async Task<IHttpActionResult> GetTime(string city)
        {
            return await GetLocalTimeInternal(city);
        }

        [HttpGet]
        [Route("Calendar/{city}/{country}/LocalTime")]
        public async Task<IHttpActionResult> GetTime(string city, string country)
        {
            return await GetLocalTimeInternal(city, country);
        }

        private async Task<IHttpActionResult> GetLocalTimeInternal(string city, string country = null)
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
