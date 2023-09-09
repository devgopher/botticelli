using BotDataSecureStorage;
using Botticelli.BotBase;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Vk.Handlers;
using Botticelli.Framework.Vk.Messages;
using Botticelli.Framework.Vk.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Botticelli.Framework.Vk.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a Vk bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddVkBot(this IServiceCollection services,
                                              IConfiguration config,
                                              BotOptionsBuilder<VkBotSettings> optionsBuilder)
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

        var token = botContext.BotKey ?? string.Empty;

        services.AddHttpClient<BotStatusService<VkBot>>();

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
                .AddSingleton(secureStorage)
                .AddBotticelliFramework();

        var sp = services.BuildServiceProvider();

        var bot = new VkBot(sp.GetRequiredService<LongPollMessagesProvider>(),
                            sp.GetRequiredService<MessagePublisher>(),
                            secureStorage,
                            sp.GetRequiredService<VkStorageUploader>(),
                            sp.GetRequiredService<IBotUpdateHandler>(),
                            sp.GetRequiredService<ILogger<VkBot>>());

        return services.AddSingleton<IBot<VkBot>>(bot)
                       .AddHostedService<BotStatusService<IBot<VkBot>>>();
    }
}