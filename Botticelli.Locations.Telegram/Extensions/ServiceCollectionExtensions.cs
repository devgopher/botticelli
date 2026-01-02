using System.Reflection;
using Botticelli.Controls.Parsers;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Locations.Commands;
using Botticelli.Locations.Commands.CommandProcessors;
using Botticelli.Locations.Integration;
using Botticelli.Locations.Options;
using Flurl;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nominatim.API.Address;
using Nominatim.API.Geocoders;
using Nominatim.API.Interfaces;
using Nominatim.API.Web;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Locations.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds an OSM location provider
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection AddOsmLocations(this IServiceCollection services,
                                                     IConfiguration config,
                                                     string url = "https://nominatim.openstreetmap.org")
    {
        services.AddHttpClient<OsmLocationProvider>();
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        return services.Configure<LocationsProcessorOptions>(config)
                       .AddSingleton<ICommandValidator<FindLocationsCommand>, PassValidator<FindLocationsCommand>>()
                       .AddSingleton<ICommandValidator<MapCommand>, PassValidator<MapCommand>>()
                       .AddScoped<FindLocationsCommandProcessor<InlineKeyboardMarkup>>()
                       .AddScoped<MapCommandProcessor<ReplyKeyboardMarkup>>()
                       .AddScoped<ILocationProvider, OsmLocationProvider>()
                       .AddSingleton<INominatimWebInterface, NominatimWebInterface>()
                       .AddSingleton<IAddressSearcher, AddressSearcher>()
                       .AddSingleton<ILayoutSupplier<InlineKeyboardMarkup>, InlineTelegramLayoutSupplier>()
                       .AddSingleton<ILayoutSupplier<ReplyKeyboardMarkup>, ReplyTelegramLayoutSupplier>()
                       .AddSingleton<IForwardGeocoder, ForwardGeocoder>(sp => new ForwardGeocoder(sp.GetRequiredService<INominatimWebInterface>(),
                                                                                               Url.Combine(url, "search")))
                       .AddSingleton<IReverseGeocoder, ReverseGeocoder>(sp => new ReverseGeocoder(sp.GetRequiredService<INominatimWebInterface>(),
                                                                                               Url.Combine(url, "reverse")));
    }
}