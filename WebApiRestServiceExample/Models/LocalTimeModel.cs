using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRestServiceExample.Models
{
    public class LocalTimeModel
    {
        public string TimeZoneName { get; set; }
        public double UtcOffSet { get; set; }
        public DateTime LocalTime { get; set; }
    }
}