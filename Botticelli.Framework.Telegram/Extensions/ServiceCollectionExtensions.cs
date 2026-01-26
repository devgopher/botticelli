using System.Configuration;
using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Controls.Parsers;
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
    private static readonly BotDataSettingsBuilder<BotDataSettings> BotDataSettingsBuilder = new();

    private static readonly AnalyticsClientSettingsBuilder<AnalyticsClientSettings> AnalyticsClientOptionsBuilder =
        new();

    private static readonly DataAccessSettingsBuilder<DataAccessSettings> DataAccessSettingsBuilder = new();

    public static TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>> AddTelegramBot(this IServiceCollection services,
                                                                                                  IConfiguration configuration,
                                                                                                  Action<TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>>? telegramBotBuilderFunc = null)
    {
        return AddTelegramBot<TelegramBot, TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>>(services, configuration, telegramBotBuilderFunc);
    }

    public static TBotBuilder AddTelegramBot<TBot, TBotBuilder>(this IServiceCollection services,
                                                                       IConfiguration configuration,
                                                                       Action<TBotBuilder>? telegramBotBuilderFunc = null)
            where TBotBuilder : TelegramBotBuilder<TBot, TelegramBotBuilder<TBot>>
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
        
        return services.AddTelegramBot<TBot, TBotBuilder>(
            o => o.Set(telegramBotSettings),
            o => o.Set(analyticsClientSettings),
            o => o.Set(serverSettings),
            o => o.Set(dataAccessSettings),
            telegramBotBuilderFunc);
    }

    public static TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>> AddTelegramBot(this IServiceCollection services,
                                                                                                  TelegramBotSettings botSettings,
                                                                                                  AnalyticsClientSettings analyticsClientSettings,
                                                                                                  ServerSettings serverSettings,
                                                                                                  DataAccessSettings dataAccessSettings,
                                                                                                  Action<TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>>? telegramBotBuilderFunc = null)
    {
        return services.AddTelegramBot<TelegramBot, TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>>(o => o.Set(botSettings),
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
    public static TBotBuilder AddTelegramBot<TBot, TBotBuilder>(this IServiceCollection services,
                                                                       Action<BotSettingsBuilder<TelegramBotSettings>> optionsBuilderFunc,
                                                                       Action<AnalyticsClientSettingsBuilder<AnalyticsClientSettings>> analyticsOptionsBuilderFunc,
                                                                       Action<ServerSettingsBuilder<ServerSettings>> serverSettingsBuilderFunc,
                                                                       Action<DataAccessSettingsBuilder<DataAccessSettings>> dataAccessSettingsBuilderFunc,
                                                                       Action<TBotBuilder>? telegramBotBuilderFunc = null)
            where TBotBuilder : TelegramBotBuilder<TBot, TelegramBotBuilder<TBot>>
            where TBot : TelegramBot
    {
        services.AddHttpClient();
        var botBuilder = InnerBuild<TBot, TBotBuilder>(services,
                                                       optionsBuilderFunc,
                                                       analyticsOptionsBuilderFunc,
                                                       serverSettingsBuilderFunc,
                                                       dataAccessSettingsBuilderFunc,
                                                       telegramBotBuilderFunc);

        services.AddTelegramLayoutsSupport()
            .AddSingleton(botBuilder);

        return botBuilder;
    }

    static TBotBuilder InnerBuild<TBot, TBotBuilder>(IServiceCollection services,
                                                     Action<BotSettingsBuilder<TelegramBotSettings>> optionsBuilderFunc,
        Action<AnalyticsClientSettingsBuilder<AnalyticsClientSettings>> analyticsOptionsBuilderFunc,
        Action<ServerSettingsBuilder<ServerSettings>> serverSettingsBuilderFunc, 
        Action<DataAccessSettingsBuilder<DataAccessSettings>> dataAccessSettingsBuilderFunc,
        Action<TBotBuilder>? telegramBotBuilderFunc)
        where TBotBuilder  : TelegramBotBuilder<TBot, TelegramBotBuilder<TBot>>
        where TBot : TelegramBot
    {
        services.AddHttpClient();
        optionsBuilderFunc(SettingsBuilder);
        serverSettingsBuilderFunc(ServerSettingsBuilder);
        analyticsOptionsBuilderFunc(AnalyticsClientOptionsBuilder);
        dataAccessSettingsBuilderFunc(DataAccessSettingsBuilder);

        var clientBuilder = TelegramClientDecoratorBuilder.Instance(services, SettingsBuilder);

        var botBuilder = TelegramBotBuilder<TBot>.Instance<TBotBuilder>(services,
            ServerSettingsBuilder,
            SettingsBuilder,
            DataAccessSettingsBuilder,
            AnalyticsClientOptionsBuilder,
            false);

        if (botBuilder == null)
            throw new ApplicationException("bot builder is null!");
        
        botBuilder.AddClient(clientBuilder)
                  .AddServices(services);
        
        telegramBotBuilderFunc?.Invoke(botBuilder);
        
        services.AddSingleton<IBot>(sp => sp.GetRequiredService<TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>>()
                                            .Build(sp)!);
        
        return botBuilder;
    }

    public static TelegramStandaloneBotBuilder<TelegramBot> AddStandaloneTelegramBot(this IServiceCollection services,
                                                                              IConfiguration configuration,
                                                                              Action<TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>>? telegramBotBuilderFunc = null) =>
        AddStandaloneTelegramBot<TelegramBot>(services, configuration, telegramBotBuilderFunc);

    public static TelegramStandaloneBotBuilder<TBot> AddStandaloneTelegramBot<TBot>(this IServiceCollection services,
                                                                                    IConfiguration configuration,
                                                                                    Action<TelegramBotBuilder<TBot, TelegramBotBuilder<TBot>>>? telegramBotBuilderFunc = null)
        where TBot : TelegramBot
    {
        services.AddHttpClient();
        var telegramBotSettings = configuration
                                      .GetSection(TelegramBotSettings.Section)
                                      .Get<TelegramBotSettings>() ??
                                  throw new ConfigurationErrorsException(
                                      $"Can't load configuration for {nameof(TelegramBotSettings)}!");

        var dataAccessSettings = configuration
                                     .GetSection(DataAccessSettings.Section)
                                     .Get<DataAccessSettings>() ??
                                 throw new ConfigurationErrorsException(
                                     $"Can't load configuration for {nameof(DataAccessSettings)}!");

        var botDataSettings = configuration
                                  .GetSection(BotDataSettings.Section)
                                  .Get<BotDataSettings>() ??
                              throw new ConfigurationErrorsException(
                                  $"Can't load configuration for {nameof(BotDataSettings)}!");

        return services.AddStandaloneTelegramBot<TBot>(
            botSettingsBuilder => botSettingsBuilder.Set(telegramBotSettings),
            dataAccessSettingsBuilder => dataAccessSettingsBuilder.Set(dataAccessSettings),
            botDataSettingsBuilder => botDataSettingsBuilder.Set(botDataSettings));
    }

    public static TelegramStandaloneBotBuilder<TBot> AddStandaloneTelegramBot<TBot>(this IServiceCollection services,
                                                                                    Action<BotSettingsBuilder<TelegramBotSettings>> optionsBuilderFunc,
                                                                                    Action<DataAccessSettingsBuilder<DataAccessSettings>> dataAccessSettingsBuilderFunc,
                                                                                    Action<BotDataSettingsBuilder<BotDataSettings>> botDataSettingsBuilderFunc,
                                                                                    Action<TelegramStandaloneBotBuilder<TBot>>? telegramBotBuilderFunc = null)
        where TBot : TelegramBot
    {
        services.AddHttpClient();
        optionsBuilderFunc(SettingsBuilder);
        dataAccessSettingsBuilderFunc(DataAccessSettingsBuilder);
        botDataSettingsBuilderFunc(BotDataSettingsBuilder);

        var clientBuilder = TelegramClientDecoratorBuilder.Instance(services, SettingsBuilder);

        var botBuilder = TelegramStandaloneBotBuilder<TBot>.Instance(services,
                SettingsBuilder,
                DataAccessSettingsBuilder)
            .AddBotData(BotDataSettingsBuilder)
            .AddClient(clientBuilder);

        telegramBotBuilderFunc?.Invoke((TelegramStandaloneBotBuilder<TBot>)botBuilder);
        
        services.AddTelegramLayoutsSupport();

        return botBuilder as TelegramStandaloneBotBuilder<TBot>;
    }

    public static IServiceCollection AddTelegramLayoutsSupport(this IServiceCollection services) =>
        services.AddSingleton<ILayoutParser, JsonLayoutParser>()
            .AddSingleton<ILayoutSupplier<ReplyKeyboardMarkup>, ReplyTelegramLayoutSupplier>()
            .AddSingleton<ILayoutSupplier<InlineKeyboardMarkup>, InlineTelegramLayoutSupplier>()
            .AddSingleton<IReplyTelegramLayoutSupplier, ReplyTelegramLayoutSupplier>()
            .AddSingleton<IInlineTelegramLayoutSupplier, InlineTelegramLayoutSupplier>()
            .AddSingleton<ILayoutLoader<ReplyKeyboardMarkup>, LayoutLoader<ILayoutParser,
                ILayoutSupplier<ReplyKeyboardMarkup>, ReplyKeyboardMarkup>>()
            .AddSingleton<ILayoutLoader<InlineKeyboardMarkup>, LayoutLoader<ILayoutParser,
                ILayoutSupplier<InlineKeyboardMarkup>, InlineKeyboardMarkup>>();
}