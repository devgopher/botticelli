using System.Threading.Tasks;
using Nominatim.API.Interfaces;
using Nominatim.API.Models;

namespace Botticelli.Locations.Tests;

public class ReverseGeocoderMock : IReverseGeocoder
{
    public Task<GeocodeResponse> ReverseGeocode(ReverseGeocodeRequest req)
    {
        if (req is { Latitude: not null, Longitude: not null })
            return Task.FromResult<GeocodeResponse>(new()
            {
                Latitude = req.Latitude.Value,
                Longitude = req.Longitude.Value,
                DisplayName = "TESTGEO",
                Address = new AddressResult
                {
                    Country = "TestCountry",
                    CountryCode = "TC0202",
                    County = "TestCounty",
                    HouseNumber = "999",
                    PostCode = "10291",
                    Road = "Abbey",
                    State = "NowhereState",
                    Town = "NowhereVille",
                    Pedestrian = "Sidewalk",
                    District = "NoDistrict",
                    Name = string.Empty
                }
            });
        
        throw new System.InvalidOperationException();
    }
}