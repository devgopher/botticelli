using Botticelli.Bot.Data;
using Botticelli.Bot.Data.Repositories;
using Botticelli.Bot.Data.Settings;
using Botticelli.Bot.Utils;
using Botticelli.Bot.Utils.TextUtils;
using Botticelli.Client.Analytics;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Builders;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Security;
using Botticelli.Framework.Services;
using Botticelli.Framework.Vk.Messages.Handlers;
using Botticelli.Framework.Vk.Messages.HostedService;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Framework.Vk.Messages.Utils;
using Botticelli.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk.Messages.Builders;

public class VkBotBuilder : BotBuilder<VkBot, VkBotBuilder>
{
    private LongPollMessagesProvider? _longPollMessagesProvider;
    private LongPollMessagesProviderBuilder? _longPollMessagesProviderBuilder;
    private MessagePublisher? _messagePublisher;
    private MessagePublisherBuilder? _messagePublisherBuilder;
    private VkStorageUploader? _vkStorageUploader;
    private VkStorageUploaderBuilder? _vkStorageUploaderBuilder;

    private VkBotSettings? BotSettings { get; set; }

    protected override VkBot InnerBuild()
    {
        Services!.AddHttpClient<BotStatusService>()
                 .AddServerCertificates(BotSettings);
        Services!.AddHostedService<BotStatusService>();
        Services!.AddHttpClient<BotKeepAliveService>()
                 .AddServerCertificates(BotSettings);
        Services!.AddHttpClient<GetBroadCastMessagesService<VkBot>>()
                 .AddServerCertificates(BotSettings);
        Services!.AddHostedService<BotKeepAliveService>();
        Services!.AddHostedService<GetBroadCastMessagesService<IBot<VkBot>>>();

        Services!.AddHostedService<VkBotHostedService>();
        var botId = BotDataUtils.GetBotId();

        if (botId == null) throw new InvalidDataException($"{nameof(botId)} shouldn't be null!");

        #region Metrics

        var metricsPublisher = new MetricsPublisher(AnalyticsClientSettingsBuilder.Build());
        var metricsProcessor = new MetricsProcessor(metricsPublisher);
        Services!.AddSingleton(metricsPublisher);
        Services!.AddSingleton(metricsProcessor);

        #endregion

        #region Data

        Services!.AddDbContext<BotInfoContext>(o =>
                                                       o.UseSqlite($"Data source={BotDataAccessSettingsBuilder.Build().ConnectionString}"));
        Services!.AddScoped<IBotDataAccess, BotDataAccess>();

        #endregion

        #region TextTransformer

        Services!.AddTransient<ITextTransformer, VkTextTransformer>();

        #endregion

        _longPollMessagesProvider = _longPollMessagesProviderBuilder.Build();
        _messagePublisher = _messagePublisherBuilder.Build();
        _vkStorageUploader = _vkStorageUploaderBuilder.Build();

        Services!.AddBotticelliFramework()
                 .AddSingleton<IBotUpdateHandler, BotUpdateHandler>();

        var sp = Services!.BuildServiceProvider();

        return new VkBot(_longPollMessagesProvider,
                         _messagePublisher,
                         _vkStorageUploader,
                         sp.GetRequiredService<IBotDataAccess>(),
                         sp.GetRequiredService<IBotUpdateHandler>(),
                         sp.GetRequiredService<MetricsProcessor>(),
                         sp.GetRequiredService<ILogger<VkBot>>());
    }

    protected virtual VkBotBuilder AddBotSettings<TBotSettings>(BotSettingsBuilder<TBotSettings> settingsBuilder)
            where TBotSettings : BotSettings, new()
    {
        BotSettings = settingsBuilder.Build() as VkBotSettings ?? throw new InvalidOperationException();

        return this;
    }

    public VkBotBuilder AddClient(LongPollMessagesProviderBuilder builder)
    {
        _longPollMessagesProviderBuilder = builder;

        return this;
    }

    public static VkBotBuilder Instance(IServiceCollection services,
                                        ServerSettingsBuilder<ServerSettings> serverSettingsBuilder,
                                        BotSettingsBuilder<VkBotSettings> settingsBuilder,
                                        DataAccessSettingsBuilder<DataAccessSettings> dataAccessSettingsBuilder,
                                        AnalyticsClientSettingsBuilder<AnalyticsClientSettings> analyticsClientSettingsBuilder)
    {
        return (VkBotBuilder) new VkBotBuilder()
                              .AddBotSettings(settingsBuilder)
                              .AddServerSettings(serverSettingsBuilder)
                              .AddServices(services)
                              .AddAnalyticsSettings(analyticsClientSettingsBuilder)
                              .AddBotDataAccessSettings(dataAccessSettingsBuilder);
    }
}