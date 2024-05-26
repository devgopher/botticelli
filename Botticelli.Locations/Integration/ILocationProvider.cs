using System.Globalization;
using Botticelli.Locations.Models;

namespace Botticelli.Locations.Integration;

public interface ILocationProvider
{
    public Task<Address?> GetAddress(Location location);

    public Task<IEnumerable<Address>> Search(string query, int maxPoints);
    
    public Task<TimeZoneInfo?> GetTimeZone(Location location);
}