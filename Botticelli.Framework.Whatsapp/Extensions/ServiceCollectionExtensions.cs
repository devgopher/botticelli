using BotDataSecureStorage;
using Botticelli.BotBase;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.WhatsApp.HostedService;
using Botticelli.Framework.WhatsApp.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.WhatsApp.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddWhatsAppBot(this IServiceCollection services,
                                                    IConfiguration config,
                                                    BotOptionsBuilder<WhatsAppBotSettings> optionsBuilder)
    {
        var settings = optionsBuilder.Build();
        var secureStorage = new SecureStorage(settings.SecureStorageSettings);
        var botKey = secureStorage.GetBotKey(BotDataUtils.GetBotId());
        var token = botKey.Key;

        services.AddHttpClient<BotStatusService<WhatsAppBot>>();

        var serverConfig = new ServerSettings();
        config.GetSection(nameof(ServerSettings)).Bind(serverConfig);

        services.AddSingleton(serverConfig)
                .AddBotticelliFramework();

        var sp = services.BuildServiceProvider();

        var bot = new WhatsAppBot(sp.GetRequiredService<ILogger<WhatsAppBot>>());

        return services.AddSingleton<IBot<WhatsAppBot>>(bot)
                       .AddHostedService<BotStatusService<IBot<WhatsAppBot>>>()
                       .AddHostedService<WhatsAppBotHostedService>();
    }
}