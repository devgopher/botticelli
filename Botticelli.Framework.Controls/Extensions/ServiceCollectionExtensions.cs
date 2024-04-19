using System.Windows.Input;
using Botticelli.Framework.Controls.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Controls.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramLayoutsSupport(this IServiceCollection services) =>
        services.AddScoped<ILayoutParser, JsonLayoutParser>()
            .AddScoped<ILayoutSupplier<ReplyKeyboardMarkup>, TelegramLayoutSupplier>()
            .AddScoped<ILayoutLoader<ReplyKeyboardMarkup>,
                LayoutLoader<ILayoutParser, ILayoutSupplier<ReplyKeyboardMarkup>, ReplyKeyboardMarkup>>();
}