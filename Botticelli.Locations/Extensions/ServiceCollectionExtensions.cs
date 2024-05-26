using System.Reflection;
using Botticelli.Locations.Integration;
using Flurl;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Nominatim.API.Address;
using Nominatim.API.Geocoders;
using Nominatim.API.Interfaces;
using Nominatim.API.Web;

namespace Botticelli.Locations.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds an OSM location provider
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection AddOsmLocations(this IServiceCollection services, string url = "https://nominatim.openstreetmap.org")
    {
        services.AddHttpClient<OsmLocationProvider>();
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        services.AddScoped<ILocationProvider, OsmLocationProvider>()
                .AddScoped<INominatimWebInterface, NominatimWebInterface>()
                .AddScoped<IAddressSearcher, AddressSearcher>()
                .AddScoped<IReverseGeocoder, ReverseGeocoder>(sp => 
                                                                      new ReverseGeocoder(sp.GetRequiredService<INominatimWebInterface>(), 
                                                                                          Url.Combine(url, "reverse")));

        return services;
    }
}
