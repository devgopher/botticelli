using BotDataSecureStorage;
using Botticelli.BotBase;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Framework.Telegram.HostedService;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Botticelli.Framework.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
                                                    IConfiguration config,
                                                    BotOptionsBuilder<TelegramBotSettings> optionsBuilder)
    {
        var settings = optionsBuilder.Build();
        var secureStorage = new SecureStorage(settings.SecureStorageSettings);
        var botId = BotDataUtils.GetBotId();
        var botKey = secureStorage.GetBotKey(botId);

        if (string.IsNullOrWhiteSpace(botKey?.Id))
        {
            secureStorage.SetBotKey(botId, string.Empty);
            botKey = secureStorage.GetBotKey(botId);
        }

        var token = botKey.Key ?? string.Empty;

        services.AddHttpClient<BotStatusService<TelegramBot>>();

        var serverConfig = new ServerSettings();
        config.GetSection(nameof(ServerSettings)).Bind(serverConfig);

        services.AddSingleton(serverConfig)
                .AddSingleton<IBotUpdateHandler, BotUpdateHandler>()
                .AddBotticelliFramework();

        var sp = services.BuildServiceProvider();

        var bot = new TelegramBot(new TelegramBotClient(token),
                                  sp.GetRequiredService<IBotUpdateHandler>(),
                                  sp.GetRequiredService<ILogger<TelegramBot>>(),
                                  secureStorage);

        return services.AddSingleton<IBot<TelegramBot>>(bot)
                       .AddHostedService<BotStatusService<IBot<TelegramBot>>>()
                       .AddHostedService<TelegramBotHostedService>();
    }
}