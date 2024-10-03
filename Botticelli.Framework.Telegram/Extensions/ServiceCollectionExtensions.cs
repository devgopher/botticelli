using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Builders;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    private static BotOptionsBuilder<TelegramBotSettings> _optionsBuilder = new();
    private static AnalyticsSettingsBuilder<AnalyticsSettings> _analyticsOptionsBuilder = new();
    
    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilderFunc"></param>
    /// <returns></returns>
    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
                                                    Action<BotOptionsBuilder<TelegramBotSettings>> optionsBuilderFunc)
    {
        optionsBuilderFunc(_optionsBuilder);
        var clientBuilder = TelegramClientDecoratorBuilder.Instance(services, _optionsBuilder);
        var botBuilder = TelegramBotBuilder.Instance(services, _optionsBuilder, _analyticsOptionsBuilder)
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