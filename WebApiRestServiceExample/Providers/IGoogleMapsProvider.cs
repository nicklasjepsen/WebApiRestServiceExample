using System.Threading.Tasks;

namespace WebApiRestServiceExample.Providers
{
    public interface IGoogleMapsProvider
    {
        Task<string> GetTimeZoneName(string city);
        Task<string> GetTimeZoneName(string city, string country);
    }
}