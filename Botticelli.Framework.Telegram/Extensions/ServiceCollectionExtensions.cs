using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
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
    private static readonly AnalyticsSettingsBuilder<AnalyticsSettings> AnalyticsOptionsBuilder = new();
    private static readonly DataAccessSettingsBuilder<DataAccessSettings> DataAccessSettingsBuilder = new();

    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        
        return services.AddTelegramBot(o => o.Set(sp.GetRequiredService<TelegramBotSettings>()),
                                       o => o.Set(sp.GetRequiredService<AnalyticsSettings>()),
                                       o => o.Set(sp.GetRequiredService<ServerSettings>()),
                                       o => o.Set(sp.GetRequiredService<DataAccessSettings>()));
    }

    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
                                                    TelegramBotSettings botSettings,
                                                    AnalyticsSettings analyticsSettings,
                                                    ServerSettings serverSettings,
                                                    DataAccessSettings dataAccessSettings) =>
            services.AddTelegramBot(o => o.Set(botSettings),
                                    o => o.Set(analyticsSettings),
                                    o => o.Set(serverSettings),
                                    o => o.Set(dataAccessSettings));

    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilderFunc"></param>
    /// <param name="analyticsOptionsBuilderFunc"></param>
    /// <param name="serverSettingsBuilderFunc"></param>
    /// <returns></returns>
    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
                                                    Action<BotSettingsBuilder<TelegramBotSettings>> optionsBuilderFunc,
                                                    Action<AnalyticsSettingsBuilder<AnalyticsSettings>> analyticsOptionsBuilderFunc,
                                                    Action<ServerSettingsBuilder<ServerSettings>> serverSettingsBuilderFunc,
                                                    Action<DataAccessSettingsBuilder<DataAccessSettings>> dataAccessSettingsBuilderFunc>)
    {
        optionsBuilderFunc(SettingsBuilder);
        serverSettingsBuilderFunc(ServerSettingsBuilder);
        analyticsOptionsBuilderFunc(AnalyticsOptionsBuilder);
        dataAccessSettingsBuilderFunc(DataAccessSettingsBuilder);
        
        var clientBuilder = TelegramClientDecoratorBuilder.Instance(services, SettingsBuilder);
        
        var botBuilder = TelegramBotBuilder.Instance(services,
                                                     ServerSettingsBuilder,
                                                     SettingsBuilder, 
                                                     DataAccessSettingsBuilder,
                                                     AnalyticsOptionsBuilder)
                                           .AddClient(clientBuilder);
        var bot = botBuilder.Build();
        return services.AddSingleton<IBot<TelegramBot>>(bot)
                       .AddSingleton<IBot>(bot);
    }

    public static IServiceCollection AddTelegramLayoutsSupport(this IServiceCollection services) =>
            services.AddScoped<ILayoutParser, JsonLayoutParser>()
                    .AddScoped<ILayoutSupplier<ReplyKeyboardMarkup>, ReplyTelegramLayoutSupplier>()
                    .AddScoped<ILayoutSupplier<InlineKeyboardMarkup>, InlineTelegramLayoutSupplier>()
                    .AddScoped<ILayoutLoader<ReplyKeyboardMarkup>, LayoutLoader<ILayoutParser, ILayoutSupplier<ReplyKeyboardMarkup>, ReplyKeyboardMarkup>>()
                    .AddScoped<ILayoutLoader<InlineKeyboardMarkup>, LayoutLoader<ILayoutParser, ILayoutSupplier<InlineKeyboardMarkup>, InlineKeyboardMarkup>>();
}