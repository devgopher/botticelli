using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Botticelli.Framework.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services, BotOptionsBuilder<TelegramBotSettings> optionsBuilder)
        {
            var settings = optionsBuilder.Build();
            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(sp =>
                new TelegramBotClient(settings.TelegramToken));
            services.AddSingleton(sp => new TelegramBot(sp.GetRequiredService<ITelegramBotClient>()));

            return services;
        }
    }
}
