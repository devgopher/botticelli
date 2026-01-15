using Botticelli.Locations.Models;

namespace Botticelli.Locations.Integration;

public interface ILocationProvider
{
    public Task<Address?> GetAddress(Location location);

    public Task<string> GetMapLink(Location location);

    public Task<string> GetMapLink(Address address);

    public Task<IEnumerable<Address>> Search(string query, int maxPoints, double? latitude = null,
        double? longitude = null, int? radiusInMeters = null, string[]? languages = null);

    public Task<IEnumerable<Address>> SearchByIds(string[] ids, string[]? languages = null);

    public Task<TimeZoneInfo?> GetTimeZone(Location location);
}