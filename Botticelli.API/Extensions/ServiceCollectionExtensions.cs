using Botticelli.BotBase.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.BotBase.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseBotticelli(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient<BotStatusService>();
        var serverConfig = new ServerSettings();
        config.GetSection(nameof(ServerSettings)).Bind(serverConfig);

        return services
            .AddSingleton(services)
            .AddSingleton(serverConfig)
            .AddHostedService<BotStatusService>();
    }
}