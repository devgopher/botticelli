using Botticelli.BotBase.Settings;
using Botticelli.Framework.Extensions;
using Botticelli.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.BotBase.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseBotticelli<TBot>(this IServiceCollection services,
                                                         IConfiguration config)
            where TBot : IBot
    {
        services.AddBotticelliFramework();
        services.AddHttpClient<BotStatusService<TBot>>();

        var serverConfig = new ServerSettings();
        config.GetSection(nameof(ServerSettings)).Bind(serverConfig);

        return services
               .AddSingleton(services)
               .AddSingleton(serverConfig)
               .AddHostedService<BotStatusService<TBot>>();
    }
}