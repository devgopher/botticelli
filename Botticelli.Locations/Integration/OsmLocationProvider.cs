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
    private readonly IAddressSearcher _addressSearcher;
    private readonly IForwardGeocoder _forwardGeocoder;
    private readonly IOptionsSnapshot<LocationsProcessorOptions> _options;
    private readonly IReverseGeocoder _reverseGeoCoder;

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
               $"#map={(int)_options.Value.InitialZoom}/" +
               $"{location.Lat.ToString("0.00000", CultureInfo.InvariantCulture)}/" +
               $"{location.Lng.ToString("0.00000", CultureInfo.InvariantCulture)}";
    }

    public async Task<string> GetMapLink(Address address)
    {
        return $"{_options.Value.ApiUrl}/" +
               $"#map={(int)_options.Value.InitialZoom}/" +
               $"{address.Latitude.ToString("0.00000", CultureInfo.InvariantCulture)}/" +
               $"{address.Longitude.ToString("0.00000", CultureInfo.InvariantCulture)}";
    }

    public async Task<IEnumerable<Address>> Search(string query, int maxPoints, double? latitude = null,
        double? longitude = null, int? radiusInMeters = null, string[]? languages = null)
    {
        if (radiusInMeters != null && (!latitude.HasValue || !longitude.HasValue))
            throw new ArgumentException("Please provide a valid latitude and longitude!");

        var deltaLat = radiusInMeters == null ? 0 : DeltaLat(radiusInMeters.Value);
        var deltaLong = radiusInMeters == null ? 0 : DeltaLong(radiusInMeters.Value);

        var results = (await _forwardGeocoder.Geocode(new ForwardGeocodeRequest
            {
                queryString = query,
                PreferredLanguages = languages != null ? string.Join(',', languages) : null,
                LimitResults = maxPoints,
                DedupeResults = true,
                ViewBox = radiusInMeters == null
                    ? null
                    : new BoundingBox
                    {
                        minLatitude = latitude!.Value - deltaLat,
                        minLongitude = longitude!.Value - deltaLong,
                        maxLatitude = latitude.Value + deltaLat,
                        maxLongitude = longitude.Value + deltaLong
                    }
            }))
            .OrderByDescending(gr => gr.PlaceRank)
            .Select(gr =>
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

    public async Task<IEnumerable<Address>> SearchByIds(string[] ids, string[]? languages = null)
    {
        var results = (await _addressSearcher.Lookup(new AddressSearchRequest
            {
                OSMIDs = ids,
                PreferredLanguages = languages != null ? string.Join(',', languages) : null
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

    private async Task<Address?> InnerGetAddress(Location location, string language = "")
    {
        var response = await _reverseGeoCoder.ReverseGeocode(new ReverseGeocodeRequest
        {
            Latitude = location.Lat,
            Longitude = location.Lng,
            PreferredLanguages = language
        });

        var result = response.Address?.Adapt<Address>();

        return result;
    }

    private static double DeltaLat(double radius)
    {
        return radius / 111312.0d;
    }

    private static double DeltaLong(double radius)
    {
        return radius / 72386.1936d;
    }
}