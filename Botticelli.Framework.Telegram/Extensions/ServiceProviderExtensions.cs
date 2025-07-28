using Botticelli.Framework.Telegram.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Telegram.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseTelegramBot(this IServiceProvider serviceProvider)
    {
        var builder = serviceProvider.GetRequiredService<TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>>();

        builder.Build(serviceProvider);

        return serviceProvider;
    }
}