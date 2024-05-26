using Botticelli.Locations.Integration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Locations.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds an OSM location provider
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection AddOsmLocations(this IServiceCollection services)
    {
        services.AddHttpClient<OsmLocationProvider>();
        services.AddScoped<ILocationProvider, OsmLocationProvider>();

        return services;
    }
}
