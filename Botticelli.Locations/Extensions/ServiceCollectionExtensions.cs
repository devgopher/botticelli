using System.Reflection;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
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
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;


namespace Botticelli.Locations.Extensions;

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
                .AddScoped<ICommandValidator<FindLocationsCommand>, PassValidator<FindLocationsCommand>>()
                .AddScoped<FindLocationsCommandProcessor<InlineKeyboardMarkup>>()
                .AddScoped<ILocationProvider, OsmLocationProvider>()
                .AddScoped<INominatimWebInterface, NominatimWebInterface>()
                .AddScoped<IAddressSearcher, AddressSearcher>()
                .AddScoped<ILayoutSupplier<InlineKeyboardMarkup>, InlineTelegramLayoutSupplier>()
                .AddScoped<IForwardGeocoder, ForwardGeocoder>(sp =>
                                                                      new ForwardGeocoder(sp.GetRequiredService<INominatimWebInterface>(),
                                                                                          Url.Combine(url, "search")))
                .AddScoped<IReverseGeocoder, ReverseGeocoder>(sp =>
                                                                      new ReverseGeocoder(sp.GetRequiredService<INominatimWebInterface>(),
                                                                                          Url.Combine(url, "reverse")));
    }

    public static IServiceProvider RegisterOsmLocationsCommands<TBot>(this IServiceProvider sp, string url = "https://nominatim.openstreetmap.org") 
            where TBot : class, IBot<TBot> =>
            sp.RegisterBotCommand<FindLocationsCommand, FindLocationsCommandProcessor<InlineKeyboardMarkup>, TBot>();
}
