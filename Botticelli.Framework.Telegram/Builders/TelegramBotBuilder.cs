using Botticelli.BotBase;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Builders;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Framework.Telegram.HostedService;
using Botticelli.Framework.Telegram.Layout;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Interfaces;
using Botticelli.SecureStorage.Settings;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Telegram.Builders;

public class TelegramBotBuilder : BotBuilder<TelegramBotBuilder, TelegramBot>
{
    private TelegramClientDecoratorBuilder _builder;
    private TelegramClientDecorator _client;
    private readonly ServerSettings _serverSettings;

    private TelegramBotBuilder()
    {
        _serverSettings = new ServerSettings();
        Configuration!.GetSection(nameof(ServerSettings)).Bind(_serverSettings);
    }
    
    public static TelegramBotBuilder Instance(IServiceCollection services,
        BotOptionsBuilder<TelegramBotSettings> optionsBuilder,
        IConfiguration configuration)
    {
        var builder = new TelegramBotBuilder()
            .AddServices(services)
            .AddBotSettings(optionsBuilder);
        builder.Configuration = configuration;
        
        return builder;
    }

    protected TelegramBotSettings Settings { get; set; }

    public TelegramBotBuilder AddClient(TelegramClientDecoratorBuilder builder)
    {
        _builder = builder;

        if (Settings.UseThrottling is true) _builder.AddThrottler(new Throttler());
        var token = BotContext!.BotKey ?? string.Empty;
        _builder.AddToken(token);
        _client = _builder.Build();
        _client.Timeout = TimeSpan.FromMilliseconds(Settings.Timeout);
        
        return this;
    }

    protected override void Assert()
    {
        base.Assert();
        
        if (_client == default)
            throw new NullReferenceException($"{nameof(_client)} is null!");
    }
    
    protected override TelegramBot InnerBuild()
    {
        Services!.AddHttpClient<BotStatusService<TelegramBot>>()
                 .AddCertificates(Settings);
        Services!.AddHostedService<BotStatusService<IBot<TelegramBot>>>();
        Services!.AddHttpClient<BotKeepAliveService<TelegramBot>>()
                 .AddCertificates(Settings);
        Services.AddHostedService<BotKeepAliveService<IBot<TelegramBot>>>();
        
        Services.AddHostedService<TelegramBotHostedService>();
        var botId = BotDataUtils.GetBotId();
       
        if (string.IsNullOrWhiteSpace(BotContext?.BotId))
        {
            BotContext = new BotContext
            {
                BotId = botId,
                BotKey = string.Empty,
                Items = new Dictionary<string, string>()
            };

            SecureStorage!.SetBotContext(BotContext);
        }
        
        Services.AddSingleton(_serverSettings)
            .AddScoped<ILayoutSupplier<ReplyMarkupBase>, ReplyTelegramLayoutSupplier>()
            .AddBotticelliFramework(Configuration)
            .AddMetrics();
        
        var sp = Services.BuildServiceProvider();

        var bot = new TelegramBot(_client,
                                  sp.GetRequiredService<IBotUpdateHandler>(),
                                  sp.GetRequiredService<ILogger<TelegramBot>>(),
                                  MetricsProcessor,
                                  SecureStorage);

        return bot;
    }

    public override TelegramBotBuilder AddBotSettings<TBotSettings>(
        BotOptionsBuilder<TBotSettings> optionsBuilder)
    {
        Settings = optionsBuilder.Build() as TelegramBotSettings;

        return this;
    }

    protected override SecureStorageSettings GetStorageSettings() => Settings.SecureStorageSettings;
}