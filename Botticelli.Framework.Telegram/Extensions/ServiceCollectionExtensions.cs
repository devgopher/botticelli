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
    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="optionsBuilder"></param>
    /// <param name="telegramClientBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddTelegramBot(this IServiceCollection services,
                                                    IConfiguration config,
                                                    BotOptionsBuilder<TelegramBotSettings> optionsBuilder,
                                                    TelegramClientDecoratorBuilder telegramClientBuilder)
    {
        var bot = TelegramBotBuilder.Instance(services, optionsBuilder, config)
                                    .AddStorage()
                                    .AddClient(telegramClientBuilder)
                                    .Build();

        return services.AddSingleton<IBot<TelegramBot>>(bot)
                       .AddSingleton<IBot>(bot);
    }

    public static IServiceCollection AddTelegramLayoutsSupport(this IServiceCollection services)
    {
        return services.AddScoped<ILayoutParser, JsonLayoutParser>()
                       .AddScoped<ILayoutSupplier<ReplyKeyboardMarkup>, ReplyTelegramLayoutSupplier>()
                       .AddScoped<ILayoutSupplier<InlineKeyboardMarkup>, InlineTelegramLayoutSupplier>()
                       .AddScoped<ILayoutLoader<ReplyKeyboardMarkup>, LayoutLoader<ILayoutParser, ILayoutSupplier<ReplyKeyboardMarkup>, ReplyKeyboardMarkup>>()
                       .AddScoped<ILayoutLoader<InlineKeyboardMarkup>, LayoutLoader<ILayoutParser, ILayoutSupplier<InlineKeyboardMarkup>, InlineKeyboardMarkup>>();
    }
}