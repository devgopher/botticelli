using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nominatim.API.Interfaces;
using Nominatim.API.Models;

namespace Botticelli.Locations.Tests;

public class AddressSearcherMock : IAddressSearcher
{
    public Task<AddressLookupResponse[]> Lookup(AddressSearchRequest req)
    {
        var mockResult = new AddressLookupResponse[]
        {
            new()
            {
                PlaceID = 11222,
                License = "OpenStreetMap © OpenStreetMap contributors",
                OSMType = "node",
                OSMID = 123456789,
                Latitude = 57.6000,
                Longitude = 11.4000,
                DisplayName = "Test Street 1, 12345 Test City, Test Country",
                ExtraTags = new Dictionary<string, string>
                {
                    { "building", "yes" },
                    { "addr:housenumber", "1" },
                    { "addr:street", "Test Street" }
                },
                Address = new AddressResult
                {
                    HouseNumber = "1",
                    Road = "Test Street",
                    Suburb = "Test Suburb",
                    City = "Test City",
                    State = "Test State",
                    PostCode = "12345",
                    Country = "Test Country",
                    CountryCode = "tc"
                },
                Class = "building",
                Importance = 0.75,
                IconURL = "https://example.com/icons/building.png",
                Category = "residential",
                ClassType = "house",
                Addresstype = "building",
                PlaceRank = "30",
                Name = "Test House"
            },

            // Additional mock entry to illustrate an array of results
            new()
            {
                PlaceID = 11223,
                License = "OpenStreetMap © OpenStreetMap contributors",
                OSMType = "way",
                OSMID = 987654321,
                Latitude = 57.6050,
                Longitude = 11.4050,
                DisplayName = "Sample Park, 12345 Test City, Test Country",
                ExtraTags = new Dictionary<string, string>
                {
                    { "leisure", "park" },
                    { "name", "Sample Park" }
                },
                Address = new AddressResult
                {
                    Road = "Park Avenue",
                    Suburb = "Green District",
                    City = "Test City",
                    State = "Test State",
                    PostCode = "12345",
                    Country = "Test Country",
                    CountryCode = "tc"
                },
                Class = "leisure",
                Importance = 0.60,
                IconURL = "https://example.com/icons/park.png",
                Category = "park",
                ClassType = "public",
                Addresstype = "park",
                PlaceRank = "12",
                Name = "Sample Park"
            }
        };

        return Task.FromResult(mockResult);
    }
}