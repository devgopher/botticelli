using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Nominatim.API.Interfaces;
using RichardSzalay.MockHttp;

namespace Botticelli.Locations.Tests;

public class NominatimInterfaceMock : INominatimWebInterface
{
    public Task<T> GetRequest<T>(string url, Dictionary<string, string> parameters) => throw new System.NotImplementedException();
}