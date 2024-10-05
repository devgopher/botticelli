using Botticelli.Audio;
using Botticelli.Bot.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Vk.Messages.API.Markups;
using Botticelli.Framework.Vk.Messages.Handlers;
using Botticelli.Framework.Vk.Messages.Layout;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Botticelli.Framework.Vk.Messages.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a Vk bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="settingsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddVkBot(this IServiceCollection services,
        IConfiguration config,
        BotSettingsBuilder<VkBotSettings> settingsBuilder)
    {
        var settings = settingsBuilder.Build();
        var secureStorage = new SecureStorage.SecureStorage(settings.SecureStorageConnectionString);
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

        services.AddHttpClient<BotStatusService<VkBot>>();
        services.AddHttpClient<BotActualizationService<VkBot>>();
        
        var serverConfig = new ServerSettings();
        config.GetSection(nameof(ServerSettings)).Bind(serverConfig);

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryForeverAsync(n => TimeSpan.FromMicroseconds(settings.PollIntervalMs));

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(120);

        services.Configure<VkBotSettings>(config.GetSection(nameof(VkBotSettings)));

        services.AddHttpClient<LongPollMessagesProvider>()
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        services.AddHttpClient<VkStorageUploader>()
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        services.AddHttpClient<MessagePublisher>()
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        services.AddSingleton(serverConfig)
            .AddSingleton<IBotUpdateHandler, BotUpdateHandler>()
            .AddSingleton<VkStorageUploader>()
            .AddSingleton<LongPollMessagesProvider>()
            .AddSingleton<MessagePublisher>()
            .AddSingleton<IConvertor, UniversalLowQualityConvertor>()
            .AddSingleton<IAnalyzer, InputAnalyzer>()
            .AddSingleton(secureStorage)
            .AddBotticelliFramework(config);

        var sp = services.BuildServiceProvider();

        var bot = new VkBot(sp.GetRequiredService<LongPollMessagesProvider>(),
            sp.GetRequiredService<MessagePublisher>(),
            secureStorage,
            sp.GetRequiredService<VkStorageUploader>(),
            sp.GetRequiredService<IBotUpdateHandler>(),
            sp.GetRequiredService<MetricsProcessor>(),
            sp.GetRequiredService<ILogger<VkBot>>());

        return services.AddSingleton<IBot<VkBot>>(bot)
            .AddHostedService<BotStatusService<IBot<VkBot>>>()
            .AddHostedService<BotKeepAliveService<IBot<VkBot>>>();
    }
    public static IServiceCollection AddVkLayoutsSupport(this IServiceCollection services) =>
            services.AddScoped<ILayoutParser, JsonLayoutParser>()
                    .AddScoped<ILayoutSupplier<VkKeyboardMarkup>, VkLayoutSupplier>()
                    .AddScoped<ILayoutLoader<VkKeyboardMarkup>,
                            LayoutLoader<ILayoutParser, ILayoutSupplier<VkKeyboardMarkup>, VkKeyboardMarkup>>();
}