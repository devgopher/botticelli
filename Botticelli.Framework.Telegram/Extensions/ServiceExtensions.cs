﻿using BotDataSecureStorage;
using BotDataSecureStorage.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Botticelli.Framework.Telegram.Extensions;

public static class ServiceExtensions
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
        var token = "";

        var secureStorage = new SecureStorage(settings.SecureStorageSettings);
        token = secureStorage.GetBotKey(BotDataUtils.GetBotId()).Key;

        return services.AddSingleton<ITelegramBotClient, TelegramBotClient>(sp =>
                                                                                    new TelegramBotClient(token))
                       .AddSingleton(sp => new TelegramBot(sp.GetRequiredService<ITelegramBotClient>(), services))
                       .AddSingleton<IBot<TelegramBot>, TelegramBot>(sp => new TelegramBot(sp.GetRequiredService<ITelegramBotClient>(), services))
                       .AddSingleton<IUpdateHandler, BotUpdateHandler>();
    }
}