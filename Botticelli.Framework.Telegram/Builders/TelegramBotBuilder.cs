using System.Reflection;
using Botticelli.Bot.Data;
using Botticelli.Bot.Data.Repositories;
using Botticelli.Bot.Data.Settings;
using Botticelli.Bot.Utils;
using Botticelli.Bot.Utils.TextUtils;
using Botticelli.Client.Analytics;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Controls.Parsers;
using Botticelli.Framework.Builders;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Security;
using Botticelli.Framework.Services;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Framework.Telegram.HostedService;
using Botticelli.Framework.Telegram.Http;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Framework.Telegram.Utils;
using Botticelli.Interfaces;
using Botticelli.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Telegram.Builders;

/// <summary>
///     <inheritdoc />
/// </summary>
/// <typeparam name="TBot"></typeparam>
public abstract class TelegramBotBuilder<TBot>(bool isStandalone)
    : TelegramBotBuilder<TBot, TelegramBotBuilder<TBot>>(isStandalone)
    where TBot : TelegramBot
{
}

/// <summary>
///     Builder for a non-standalone Telegram bot
/// </summary>
/// <typeparam name="TBot"></typeparam>
/// <typeparam name="TBotBuilder"></typeparam>
public class TelegramBotBuilder<TBot, TBotBuilder> : BotBuilder<TBot, TBotBuilder>
    where TBot : TelegramBot
    where TBotBuilder : BotBuilder<TBot, TBotBuilder>
{
    private readonly bool _isStandalone;
    private readonly List<Action<IServiceProvider>> _subHandlers = [];
    private string? _botToken;
    private TelegramClientDecoratorBuilder _builder = null!;


    public TelegramBotBuilder() : this(false)
    {
        
    } 
    
    public TelegramBotBuilder(bool isStandalone)
    {
        _isStandalone = isStandalone;
    }

    protected TelegramBotSettings? BotSettings { get; set; }
    protected BotData.Entities.Bot.BotData? BotData { get; set; }

    public static TBotBuilderNew? Instance<TBotBuilderNew>(IServiceCollection services,
        ServerSettingsBuilder<ServerSettings> serverSettingsBuilder,
        BotSettingsBuilder<TelegramBotSettings> settingsBuilder,
        DataAccessSettingsBuilder<DataAccessSettings> dataAccessSettingsBuilder,
        AnalyticsClientSettingsBuilder<AnalyticsClientSettings> analyticsClientSettingsBuilder,
        bool isStandalone)
    where TBotBuilderNew : BotBuilder<TBot, TBotBuilder>
    {
        var botBuilder = Activator.CreateInstance(typeof(TBotBuilderNew), [isStandalone]) as TBotBuilderNew;

        (botBuilder as TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>)
            .AddBotSettings(settingsBuilder)
            .AddServerSettings(serverSettingsBuilder)
            .AddAnalyticsSettings(analyticsClientSettingsBuilder)
            .AddBotDataAccessSettings(dataAccessSettingsBuilder)
            .AddServices(services);
        
        return botBuilder;
    }

    public TelegramBotBuilder<TBot, TBotBuilder> AddSubHandler<T>()
        where T : class, IBotUpdateSubHandler
    {
        Services.NotNull();
        Services.AddSingleton<T>();

        _subHandlers.Add(sp =>
        {
            var botHandler = sp.GetRequiredService<IBotUpdateHandler>();
            var subHandler = sp.GetRequiredService<T>();

            botHandler.AddSubHandler(subHandler);
        });

        return this;
    }

    public TelegramBotBuilder<TBot, TBotBuilder> AddToken(string botToken)
    {
        _botToken = botToken;

        return this;
    }

    public TelegramBotBuilder<TBot, TBotBuilder> AddClient(TelegramClientDecoratorBuilder builder)
    {
        _builder = builder;

        return this;
    }

    protected override TBot? InnerBuild(IServiceProvider serviceProvider)
    {
        ApplyMigrations(serviceProvider);

        foreach (var sh in _subHandlers) sh.Invoke(serviceProvider);

        if (Activator.CreateInstance(typeof(TBot),
                _builder.Build(),
                serviceProvider.GetRequiredService<IBotUpdateHandler>(),
                serviceProvider.GetRequiredService<ILogger<TBot>>(),
                serviceProvider.GetRequiredService<ITextTransformer>(),
                serviceProvider.GetRequiredService<IBotDataAccess>(),
                serviceProvider.GetService<MetricsProcessor>()) is not TBot bot)
            throw new InvalidDataException($"{nameof(bot)} shouldn't be null!");

        AddEvents(bot);

        return bot;
    }

    public TelegramBotBuilder<TBot, TBotBuilder> Prepare()
    {
        if (!_isStandalone)
        {
            Services.AddSingleton(ServerSettingsBuilder!.Build());

            Services.AddHttpClient<BotStatusService>()
                .AddServerCertificates(BotSettings);
            Services.AddHostedService<BotStatusService>();

            Services.AddHttpClient<BotKeepAliveService>()
                .AddServerCertificates(BotSettings);
            Services.AddHostedService<BotKeepAliveService>();

            Services.AddHttpClient<GetBroadCastMessagesService<TelegramBot>>()
                .AddServerCertificates(BotSettings);
            Services.AddHostedService<GetBroadCastMessagesService<IBot<TelegramBot>>>()
                .AddHostedService<TelegramBotHostedService>();
        }

        var botId = BotDataUtils.GetBotId();

        if (botId == null) throw new InvalidDataException($"{nameof(botId)} shouldn't be null!");

        #region Metrics

        if (!_isStandalone)
        {
            var metricsPublisher = new MetricsPublisher(AnalyticsClientSettingsBuilder!.Build());
            var metricsProcessor = new MetricsProcessor(metricsPublisher);
            Services.AddSingleton(metricsPublisher);
            Services.AddSingleton(metricsProcessor);
        }

        #endregion

        #region Data

        Services.AddDbContext<BotInfoContext>(o =>
            o.UseSqlite($"Data source={BotDataAccessSettingsBuilder!.Build().ConnectionString}"));
        Services.AddScoped<IBotDataAccess, BotDataAccess>();

        #endregion

        #region TextTransformer

        Services.AddTransient<ITextTransformer, TelegramTextTransformer>();

        #endregion

        if (BotSettings?.UseThrottling is true) 
            _builder.AddThrottler(new OutcomeThrottlingDelegatingHandler());

        if (!string.IsNullOrWhiteSpace(_botToken)) _builder.AddToken(_botToken);

        var client = _builder.Build();

        client.NotNull();

        client!.Timeout = TimeSpan.FromMilliseconds(BotSettings?.Timeout ?? 10000);

        Services.AddSingleton<ILayoutSupplier<ReplyMarkup>, ReplyTelegramLayoutSupplier>()
            .AddBotticelliFramework()
            .AddSingleton<IBotUpdateHandler, BotUpdateHandler>()
            .AddSingleton(client);
        
        return this;
    }

    private void AddEvents(TBot bot)
    {
        bot.MessageSent += MessageSent;
        bot.MessageReceived += MessageReceived;
        bot.MessageRemoved += MessageRemoved;
        bot.ContactShared += SharedContact;
        bot.NewChatMembers += NewChatMembers;
    }

    protected TelegramBotBuilder<TBot, TBotBuilder> AddBotSettings<TBotSettings>(
        BotSettingsBuilder<TBotSettings> settingsBuilder)
        where TBotSettings : BotSettings, new()
    {
        BotSettings = settingsBuilder.Build() as TelegramBotSettings ?? throw new InvalidOperationException();

        return this;
    }

    private static void ApplyMigrations(IServiceProvider sp)
    {
        sp.GetRequiredService<BotInfoContext>().Database.Migrate();
    }
}