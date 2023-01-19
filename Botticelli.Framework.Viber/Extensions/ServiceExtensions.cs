using Botticelli.Framework.Options;
using Botticelli.Framework.Viber.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Viber.Api;
using Viber.Api.Settings;

namespace Botticelli.Framework.Viber.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddViberBot(this IServiceCollection services,
                                                 BotOptionsBuilder<ViberBotSettings> optionsBuilder)
    {
        var settings = optionsBuilder.Build();
        services.AddSingleton(new ViberApiSettings
        {
            HookUrl = "https://*:8081/",
            RemoteUrl = "https://chatapi.viber.com/pa/",
            ViberToken = settings.ViberToken
        });

        services.AddHttpClient<ViberService>();
        services.AddSingleton<IViberService, ViberService>();
        services.AddSingleton<IBot<ViberBot>, ViberBot>();

        return services;
    }
}