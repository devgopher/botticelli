using Botticelli.BotBase;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Framework.Telegram.HostedService;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

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
        var secureStorage = new SecureStorage.SecureStorage(settings.SecureStorageSettings);
        var botId = BotDataUtils.GetBotId();
        var botContext = secureStorage.GetBotContext(botId);

        if (string.IsNullOrWhiteSpace(botContext?.BotId))
        {
            botContext = new BotContext
            {
                BotId = botId,
                BotKey = string.Empty,
                Items = new Dictionary<string, string>()
            };

            secureStorage.SetBotContext(botContext);
        }

        var token = botContext.BotKey ?? string.Empty;

        services.AddHttpClient<BotStatusService<TelegramBot>>()
                .AddCertificates(settings);
        services.AddHttpClient<BotKeepAliveService<TelegramBot>>()
                .AddCertificates(settings);
        
        var serverConfig = new ServerSettings();
        config.GetSection(nameof(ServerSettings)).Bind(serverConfig);
        services.AddSingleton(serverConfig)
                .AddScoped<ILayoutSupplier<ReplyMarkupBase>, ReplyTelegramLayoutSupplier>()
                .AddScoped<IBotUpdateHandler, BotUpdateHandler>()
                .AddBotticelliFramework(config);

        var sp = services.BuildServiceProvider();

        var telegramClient = new TelegramClientDecorator(token)
        {
            Timeout = TimeSpan.FromMilliseconds(settings.Timeout)
        };

        var bot = new TelegramBot(telegramClient,
            sp.GetRequiredService<IBotUpdateHandler>(),
            sp.GetRequiredService<ILogger<TelegramBot>>(),
            sp.GetRequiredService<MetricsProcessor>(),
            secureStorage);

        return services.AddSingleton<IBot<TelegramBot>>(bot)
            .AddSingleton<IBot>(bot)
            .AddHostedService<BotStatusService<IBot<TelegramBot>>>()
            .AddHostedService<BotKeepAliveService<IBot<TelegramBot>>>()
            .AddHostedService<TelegramBotHostedService>();
    }
    
    public static IServiceCollection AddTelegramLayoutsSupport(this IServiceCollection services) =>
        services.AddScoped<ILayoutParser, JsonLayoutParser>()
            .AddScoped<ILayoutSupplier<ReplyKeyboardMarkup>, ReplyTelegramLayoutSupplier>()
            .AddScoped<ILayoutSupplier<InlineKeyboardMarkup>, InlineTelegramLayoutSupplier>()
            .AddScoped<ILayoutLoader<ReplyKeyboardMarkup>, LayoutLoader<ILayoutParser, ILayoutSupplier<ReplyKeyboardMarkup>, ReplyKeyboardMarkup>>()
            .AddScoped<ILayoutLoader<InlineKeyboardMarkup>, LayoutLoader<ILayoutParser, ILayoutSupplier<InlineKeyboardMarkup>, InlineKeyboardMarkup>>();
}
