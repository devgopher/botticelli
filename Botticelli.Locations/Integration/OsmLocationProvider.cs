using System.Globalization;
using Botticelli.Locations.Models;
using Botticelli.Locations.Options;
using GeoTimeZone;
using Mapster;
using Microsoft.Extensions.Options;
using Nominatim.API.Interfaces;
using Nominatim.API.Models;

namespace Botticelli.Locations.Integration;

public class OsmLocationProvider : ILocationProvider
{
    private readonly IForwardGeocoder _forwardGeocoder;
    private readonly IOptionsSnapshot<LocationsProcessorOptions> _options;
    private readonly IReverseGeocoder _reverseGeoCoder;
    private readonly IAddressSearcher _addressSearcher;

    public OsmLocationProvider(IReverseGeocoder reverseGeoCoder,
                               IForwardGeocoder forwardGeocoder,
                               IAddressSearcher addressSearcher,
                               IOptionsSnapshot<LocationsProcessorOptions> options)
    {
        _reverseGeoCoder = reverseGeoCoder;
        _forwardGeocoder = forwardGeocoder;
        _options = options;
        _addressSearcher = addressSearcher;
    }

    public async Task<Address?> GetAddress(Location location)
    {
        return await InnerGetAddress(location);
    }

    public async Task<string> GetMapLink(Location location)
    {
        return $"{_options.Value.ApiUrl}/" +
               $"#map={(int) _options.Value.InitialZoom}/" +
               $"{location.Lat.ToString("0.00000", CultureInfo.InvariantCulture)}/" +
               $"{location.Lng.ToString("0.00000", CultureInfo.InvariantCulture)}";
    }

    public async Task<string> GetMapLink(Address address)
    {
        return $"{_options.Value.ApiUrl}/" +
               $"#map={(int) _options.Value.InitialZoom}/" +
               $"{address.Latitude.ToString("0.00000", CultureInfo.InvariantCulture)}/" +
               $"{address.Longitude.ToString("0.00000", CultureInfo.InvariantCulture)}";
    }

    public async Task<IEnumerable<Address>> Search(string query, int maxPoints)
    {
        var results = (await _forwardGeocoder.Geocode(new ForwardGeocodeRequest
                {
                    queryString = query
                })).Select(gr =>
                   {
                       var address = gr.Address?.Adapt<Address>() ?? new Address();
                       address.ObjectId = gr.OSMID.ToString();
                       address.ObjectType = gr.OSMType;
                       address.Longitude = gr.Longitude;
                       address.Latitude = gr.Latitude;
                       address.DisplayName = gr.DisplayName;

                       return address;
                   })
                   .Take(maxPoints)
                   .ToList();

        return results;
    }

    public async Task<IEnumerable<Address>> SearchByIds(string[] ids)
    {
        var results = (await _addressSearcher.Lookup(new AddressSearchRequest
            {
                OSMIDs = ids
            })).Select(gr =>
            {
                var address = gr.Address?.Adapt<Address>() ?? new Address();
                address.ObjectId = gr.OSMID.ToString();
                address.ObjectType = gr.OSMType;
                address.Longitude = gr.Longitude;
                address.Latitude = gr.Latitude;
                address.DisplayName = gr.DisplayName;

                return address;
            })
            .ToList();

        return results;
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