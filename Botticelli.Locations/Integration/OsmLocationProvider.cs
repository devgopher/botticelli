using Botticelli.Locations.Models;
using GeoTimeZone;
using Mapster;
using Nominatim.API.Interfaces;
using Nominatim.API.Models;

namespace Botticelli.Locations.Integration;

public class OsmLocationProvider : ILocationProvider
{
    private readonly IReverseGeocoder _reverseGeoCoder;
    private readonly IForwardGeocoder _forwardGeocoder;

    public OsmLocationProvider(IReverseGeocoder reverseGeoCoder, IForwardGeocoder forwardGeocoder)
    {
        _reverseGeoCoder = reverseGeoCoder;
        _forwardGeocoder = forwardGeocoder;
    }

    public async Task<Address?> GetAddress(Location location)
        => await InnerGetAddress(location);

    public async Task<IEnumerable<Address>> Search(string query, int maxPoints)
    {
        var results = await _forwardGeocoder.Geocode(new ForwardGeocodeRequest
        {
            queryString = query
        });

        return results.Take(maxPoints)
                      .Select(r => r.Address)
                      .Adapt<IEnumerable<Address>>();
    }

    public Task<TimeZoneInfo?> GetTimeZone(Location location)
    {
        var tz = TimeZoneLookup.GetTimeZone(location.Lat, location.Lng).Result;
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(tz);

        return Task.FromResult(tzi)!;
    }

    private async Task<Address?> InnerGetAddress(Location location)
    {
        var response = await _reverseGeoCoder.ReverseGeocode(new ReverseGeocodeRequest
        {
            Latitude = location.Lat,
            Longitude = location.Lng
        });

        var result = response.Address?.Adapt<Address>();

        return result;
    }
}