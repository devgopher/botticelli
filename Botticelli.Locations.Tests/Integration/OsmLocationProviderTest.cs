using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Botticelli.Locations.Integration;
using Botticelli.Locations.Models;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Shared;

namespace Botticelli.Locations.Tests.Integration;

[TestFixture]
[TestOf(typeof(OsmLocationProvider))]
public class OsmLocationProviderTest
{
    private ILocationProvider _locationProvider;

    public OsmLocationProviderTest()
    {
        var reverseGeocoder = new ReverseGeocoderMock();
        var forwardGeocoder = new ForwardGeocoderMock();
        _locationProvider = new OsmLocationProvider(reverseGeocoder, forwardGeocoder);
    }
    
    [Test]
    [TestCase("Test", 1000)]
    public async Task SearchTest(string query, int maxPoints)
    {
        var result = await _locationProvider.Search(query, maxPoints);
        
        Assert.NotNull(result);
    }
    
    [Test]
    [TestCase(22, 44)]
    public async Task GetAddressTest(double lat, double lng)
    {
        var result = await _locationProvider.GetAddress(new Location(lat, lng));
        
        Assert.NotNull(result);
    }

    [Test]
    [TestCase(51, 0)]
    public async Task GetTimeZoneTest(double lat, double lng)
    {
        var result = await _locationProvider.GetTimeZone(new Location(lat, lng));
        
        Assert.NotNull(result);
        Assert.AreEqual(result.BaseUtcOffset, TimeSpan.Zero);
    }
}