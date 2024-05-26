using Botticelli.Locations.Models;
using GeoTimeZone;
using Mapster;
using Microsoft.Extensions.Logging;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using Nominatim.API.Web;

namespace Botticelli.Locations.Integration;

public class OsmLocationProvider : ILocationProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OsmLocationProvider(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<Address?> GetAddress(Location location)
        => await InnerGetAddress(location);

    public Task<TimeZoneInfo?> GetTimeZone(Location location)
    {
        var tz = TimeZoneLookup.GetTimeZone(location.Lat, location.Long).Result;
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(tz);
        return Task.FromResult(tzi)!;
    }

    private async Task<Address?> InnerGetAddress(Location location)
    {
        var nominatimWebInterface = new NominatimWebInterface(_httpClientFactory);

        var geoCoder = new ReverseGeocoder(nominatimWebInterface);

        var response = await geoCoder.ReverseGeocode(new ReverseGeocodeRequest()
        {
            Latitude = location.Lat,
            Longitude = location.Long
        });

        var result = response.Address?.Adapt<Address>();
        
        return result;
    }
}