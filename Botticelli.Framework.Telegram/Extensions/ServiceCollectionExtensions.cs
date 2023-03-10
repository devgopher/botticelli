using BotDataSecureStorage;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Botticelli.Framework.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
                                                    BotOptionsBuilder<TelegramBotSettings> optionsBuilder)
    {
        var settings = optionsBuilder.Build();

        var secureStorage = new SecureStorage(settings.SecureStorageSettings);
        var botKey = secureStorage.GetBotKey(BotDataUtils.GetBotId());
        var token = botKey.Key;

        return services.AddScoped(sp => new TelegramBot(sp.GetRequiredService<ITelegramBotClient>(), services))
                       .AddSingleton<IBot<TelegramBot>, TelegramBot>(sp => new TelegramBot(new TelegramBotClient(token), services))
                       .AddSingleton<IBot, TelegramBot>(sp => new TelegramBot(new TelegramBotClient(token), services))
                       .AddTransient<IUpdateHandler, BotUpdateHandler>();
    }
}