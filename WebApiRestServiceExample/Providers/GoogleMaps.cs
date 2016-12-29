using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace WebApiRestServiceExample.Providers
{
    public class GoogleMapsProvider : IGoogleMapsProvider
    {
        public async Task<string> GetTimeZoneName(string city)
        {
            return await GetTimeZoneName(city, string.Empty);    
        }

        public async Task<string> GetTimeZoneName(string city, string country)
        {
            string getAddressUrl;
            if (string.IsNullOrEmpty(country))
                getAddressUrl = $"http://maps.googleapis.com/maps/api/geocode/json?address={city}&sensor=false";
            else
                getAddressUrl =
                    $"http://maps.googleapis.com/maps/api/geocode/json?address={city},%20{country}&sensor=false";
            using (var httpClient = new HttpClient())
            {
                var json =
                    await
                        httpClient.GetStringAsync(getAddressUrl);
                var cityResult = JsonConvert.DeserializeObject<GoogleMapsSearchResult>(json);
                var firstResult = cityResult.Results.FirstOrDefault();
                if (firstResult?.Geometry?.Location == null)
                    return null;

                var jsonTimeZone =
                    await
                        httpClient.GetStringAsync(
                            $"https://maps.googleapis.com/maps/api/timezone/json?location={firstResult.Geometry.Location.Latitude},{firstResult.Geometry.Location.Longitude}&timestamp=1362209227&sensor=false");
                return JsonConvert.DeserializeObject<GoogleTimeZoneResult>(jsonTimeZone)?.TimeZoneName;
            }
        }
    }

    public class GoogleMapsSearchResult
    {
        [JsonProperty(PropertyName = "results")]
        public Result[] Results { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }

    public class Result
    {
        [JsonProperty(PropertyName = "address_components")]
        public AddressComponents[] AddressComponents { get; set; }
        [JsonProperty(PropertyName = "formatted_address")]
        public string FormattedAddress { get; set; }
        [JsonProperty(PropertyName = "geometry")]
        public Geometry Geometry { get; set; }
        [JsonProperty(PropertyName = "place_id")]
        public string PlaceId { get; set; }
        [JsonProperty(PropertyName = "types")]
        public string[] Types { get; set; }
    }

    public class Geometry
    {
        [JsonProperty(PropertyName = "bounds")]
        public Bounds Bounds { get; set; }
        [JsonProperty(PropertyName = "location")]
        public Coordinates Location { get; set; }
        [JsonProperty(PropertyName = "location_type")]
        public string LocationType { get; set; }
        [JsonProperty(PropertyName = "viewport")]
        public Bounds Viewport { get; set; }
    }

    public class Bounds
    {
        [JsonProperty(PropertyName = "northeast")]
        public Coordinates Northeast { get; set; }
        [JsonProperty(PropertyName = "southeast")]
        public Coordinates Southwest { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public string Longitude { get; set; }
    }

    public class AddressComponents
    {
        [JsonProperty(PropertyName = "long_name")]
        public string LongName { get; set; }
        [JsonProperty(PropertyName = "short_name")]
        public string ShortName { get; set; }
        [JsonProperty(PropertyName = "types")]
        public string[] Types { get; set; }
    }


    public class GoogleTimeZoneResult
    {
        [JsonProperty(PropertyName = "dstOffset")]
        public int DstOffset { get; set; }
        [JsonProperty(PropertyName = "rawOffset")]
        public int RawOffset { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "timeZoneId")]
        public string TimeZoneId { get; set; }
        [JsonProperty(PropertyName = "timeZoneName")]
        public string TimeZoneName { get; set; }
    }
}