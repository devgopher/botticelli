using BotDataSecureStorage;
using Botticelli.Audio;
using Botticelli.BotBase;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Facebook.Messages.Options;
using Botticelli.Framework.Facebook.Messages.WebHooksReceivers;
using Botticelli.Framework.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Deveel.Webhooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Facebook.Messages.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a Vk bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddFacebookBot(this IServiceCollection services,
        IConfiguration config,
        BotOptionsBuilder<FacebookBotSettings> optionsBuilder)
    {
        var settings = optionsBuilder.Build();
        var secureStorage = new SecureStorage(settings.SecureStorageSettings);
        var botId = BotDataUtils.GetBotId();
        var botContext = secureStorage.GetBotContext(botId);

        if (string.IsNullOrWhiteSpace(botContext?.BotId))
        {
            botContext = new BotContext
            {
                BotId = botId,
                BotKey = string.Empty,
                Items = new Dictionary<string, string>()
            };

            secureStorage.SetBotContext(botContext);
        }

        services.AddHttpClient<BotStatusService<FacebookBot>>();
        services.AddHttpClient<BotActualizationService<FacebookBot>>();

        var serverConfig = new ServerSettings();
        config.GetSection(nameof(ServerSettings)).Bind(serverConfig);

        services.Configure<FacebookBotSettings>(config.GetSection(nameof(FacebookBotSettings)));

        services.AddSingleton(serverConfig)
            .AddSingleton<IConvertor, UniversalLowQualityConvertor>()
            .AddSingleton<IAnalyzer, InputAnalyzer>()
            .AddSingleton(secureStorage)
            .AddBotticelliFramework(config);

        services.AddFacebookReceiver(options =>
        {
            options.AppSecret = settings.AppSecret;
            options.VerifyToken = settings.VerifyToken;
            options.VerifySignature = true;
        }).AddHandler<FacebookHandler>(ServiceLifetime.Singleton);

        var sp = services.BuildServiceProvider();

        var bot = new FacebookBot(secureStorage,
            sp.GetRequiredService<MetricsProcessor>(),
            sp.GetRequiredService<ILogger<FacebookBot>>());

        return services.AddSingleton<IBot<FacebookBot>>(bot)
            .AddHostedService<BotStatusService<IBot<FacebookBot>>>()
            .AddHostedService<BotKeepAliveService<IBot<FacebookBot>>>();
    }
}