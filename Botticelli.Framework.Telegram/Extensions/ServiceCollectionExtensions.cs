using System.Configuration;
using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Builders;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Builders;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly BotSettingsBuilder<TelegramBotSettings> SettingsBuilder = new();
    private static readonly ServerSettingsBuilder<ServerSettings> ServerSettingsBuilder = new();

    private static readonly AnalyticsClientSettingsBuilder<AnalyticsClientSettings> AnalyticsClientOptionsBuilder =
            new();

    private static readonly DataAccessSettingsBuilder<DataAccessSettings> DataAccessSettingsBuilder = new();

    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
                                                    IConfiguration configuration,
                                                    Action<BotBuilder<TelegramBot>>? telegramBotBuilderFunc = null)
    {
        return AddTelegramBot<TelegramBot>(services, configuration, telegramBotBuilderFunc);
    }

    public static IServiceCollection AddTelegramBot<TBot>(this IServiceCollection services,
                                                          IConfiguration configuration,
                                                          Action<BotBuilder<TBot>>? telegramBotBuilderFunc = null)
            where TBot : TelegramBot
    {
        var telegramBotSettings = configuration
                                  .GetSection(TelegramBotSettings.Section)
                                  .Get<TelegramBotSettings>() ??
                                  throw new ConfigurationErrorsException($"Can't load configuration for {nameof(TelegramBotSettings)}!");

        var analyticsClientSettings = configuration
                                      .GetSection(AnalyticsClientSettings.Section)
                                      .Get<AnalyticsClientSettings>() ??
                                      throw new ConfigurationErrorsException($"Can't load configuration for {nameof(AnalyticsClientSettings)}!");

        var serverSettings = configuration
                             .GetSection(ServerSettings.Section)
                             .Get<ServerSettings>() ??
                             throw new ConfigurationErrorsException($"Can't load configuration for {nameof(ServerSettings)}!");

        var dataAccessSettings = configuration
                                 .GetSection(DataAccessSettings.Section)
                                 .Get<DataAccessSettings>() ??
                                 throw new ConfigurationErrorsException($"Can't load configuration for {nameof(DataAccessSettings)}!");

        return services.AddTelegramBot(telegramBotSettings,
                                       analyticsClientSettings,
                                       serverSettings,
                                       dataAccessSettings,
                                       telegramBotBuilderFunc);
    }

    public static IServiceCollection AddTelegramBot<TBot>(this IServiceCollection services,
                                                          TelegramBotSettings botSettings,
                                                          AnalyticsClientSettings analyticsClientSettings,
                                                          ServerSettings serverSettings,
                                                          DataAccessSettings dataAccessSettings,
                                                          Action<BotBuilder<TBot>>? telegramBotBuilderFunc = null)
            where TBot : TelegramBot
    {
        return services.AddTelegramBot(o => o.Set(botSettings),
                                       o => o.Set(analyticsClientSettings),
                                       o => o.Set(serverSettings),
                                       o => o.Set(dataAccessSettings),
                                       telegramBotBuilderFunc);
    }

    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilderFunc"></param>
    /// <param name="analyticsOptionsBuilderFunc"></param>
    /// <param name="serverSettingsBuilderFunc"></param>
    /// <param name="dataAccessSettingsBuilderFunc"></param>
    /// <param name="telegramBotBuilderFunc"></param>
    /// <returns></returns>
    public static IServiceCollection AddTelegramBot<TBot>(this IServiceCollection services,
        Action<BotSettingsBuilder<TelegramBotSettings>> optionsBuilderFunc,
        Action<AnalyticsClientSettingsBuilder<AnalyticsClientSettings>> analyticsOptionsBuilderFunc,
        Action<ServerSettingsBuilder<ServerSettings>> serverSettingsBuilderFunc,
        Action<DataAccessSettingsBuilder<DataAccessSettings>> dataAccessSettingsBuilderFunc,
        Action<TelegramBotBuilder<TBot, TelegramBotBuilder<TBot>>>? telegramBotBuilderFunc = null)
        where TBot : TelegramBot
    {
        optionsBuilderFunc(SettingsBuilder);
        serverSettingsBuilderFunc(ServerSettingsBuilder);
        analyticsOptionsBuilderFunc(AnalyticsClientOptionsBuilder);
        dataAccessSettingsBuilderFunc(DataAccessSettingsBuilder);

        var clientBuilder = TelegramClientDecoratorBuilder.Instance(services, SettingsBuilder);

        var botBuilder = TelegramBotBuilder<TBot>.Instance(services,
                ServerSettingsBuilder,
                SettingsBuilder,
                DataAccessSettingsBuilder,
                AnalyticsClientOptionsBuilder)
            .AddClient(clientBuilder);

        telegramBotBuilderFunc?.Invoke(botBuilder);
        TelegramBot? bot = botBuilder.Build();

        return services.AddSingleton<IBot>(bot!)
            .AddTelegramLayoutsSupport();
    }
    
    public static IServiceCollection AddStandaloneTelegramBot<TBot>(this IServiceCollection services,
        Action<BotSettingsBuilder<TelegramBotSettings>> optionsBuilderFunc,
        Action<DataAccessSettingsBuilder<DataAccessSettings>> dataAccessSettingsBuilderFunc,
        Action<TelegramStandaloneBotBuilder<TBot>>? telegramBotBuilderFunc = null)
        where TBot : TelegramBot
    {
        optionsBuilderFunc(SettingsBuilder);
        dataAccessSettingsBuilderFunc(DataAccessSettingsBuilder);

        var clientBuilder = TelegramClientDecoratorBuilder.Instance(services, SettingsBuilder);

        var botBuilder = TelegramStandaloneBotBuilder<TBot>.Instance(services,
                SettingsBuilder,
                DataAccessSettingsBuilder)
            .AddClient(clientBuilder);

        telegramBotBuilderFunc?.Invoke(botBuilder);
        TelegramBot? bot = botBuilder.Build();
        
        return services.AddSingleton<IBot>(bot!)
            .AddTelegramLayoutsSupport();
    }
    
    public static IServiceCollection AddTelegramLayoutsSupport(this IServiceCollection services) =>
        services.AddSingleton<ILayoutParser, JsonLayoutParser>()
            .AddSingleton<ILayoutSupplier<ReplyKeyboardMarkup>, ReplyTelegramLayoutSupplier>()
            .AddSingleton<ILayoutSupplier<InlineKeyboardMarkup>, InlineTelegramLayoutSupplier>()
            .AddSingleton<ILayoutLoader<ReplyKeyboardMarkup>, LayoutLoader<ILayoutParser,
                ILayoutSupplier<ReplyKeyboardMarkup>, ReplyKeyboardMarkup>>()
            .AddSingleton<ILayoutLoader<InlineKeyboardMarkup>, LayoutLoader<ILayoutParser,
                ILayoutSupplier<InlineKeyboardMarkup>, InlineKeyboardMarkup>>();
}