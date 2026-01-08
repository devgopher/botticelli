using Botticelli.Locations.Models;

namespace Botticelli.Locations.Integration;

public interface ILocationProvider
{
    public Task<Address?> GetAddress(Location location);

    public Task<string> GetMapLink(Location location);

    public Task<string> GetMapLink(Address address);

    public Task<IEnumerable<Address>> Search(string query, int maxPoints);

    public Task<IEnumerable<Address>> SearchByIds(string[] ids);

    public Task<TimeZoneInfo?> GetTimeZone(Location location);
}