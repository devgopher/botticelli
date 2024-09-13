using Botticelli.Bot.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Client.Analytics.Extensions;
using Botticelli.Client.Analytics.Settings;
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

    private TelegramBotBuilder()
    {
    }
    
    public static TelegramBotBuilder Instance(IServiceCollection services,
        BotOptionsBuilder<TelegramBotSettings> optionsBuilder,
        AnalyticsSettingsBuilder<AnalyticsSettings> analyticsSettingsBuilder)
    {
        var builder = new TelegramBotBuilder()
            .AddServices(services)
            .AddServerSettings(ser)
            .AddAnalyticsSettings(analyticsSettingsBuilder)
            .AddBotSettings(optionsBuilder);

        return builder;
    }

    private TelegramBotSettings Settings { get; set; }

    public TelegramBotBuilder AddClient(TelegramClientDecoratorBuilder builder)
    {
        _builder = builder;

        return this;
    }

    protected override TelegramBot InnerBuild()
    {
        Services!.AddHttpClient<BotStatusService<TelegramBot>>()
                 .AddCertificates(Settings);
        Services!.AddHostedService<BotStatusService<IBot<TelegramBot>>>();
        Services!.AddHttpClient<BotKeepAliveService<TelegramBot>>()
                 .AddCertificates(Settings);
        Services!.AddHostedService<BotKeepAliveService<IBot<TelegramBot>>>();
        
        Services!.AddHostedService<TelegramBotHostedService>();
        var botId = BotDataUtils.GetBotId();

        if (botId == null) throw new InvalidDataException($"{nameof(botId)} shouldn't be null!");
       
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
        
        var token = BotContext.BotKey;
        _builder.AddToken(token);
        
        if (Settings.UseThrottling is true) 
            _builder.AddThrottler(new Throttler());
        _client = _builder.Build();
        _client.Timeout = TimeSpan.FromMilliseconds(Settings.Timeout);
        
        Services!.AddSingleton(_serverSettings)
            .AddScoped<ILayoutSupplier<ReplyMarkupBase>, ReplyTelegramLayoutSupplier>()
            .AddBotticelliFramework()
            .AddMetrics(zz)
            .AddSingleton<IBotUpdateHandler, BotUpdateHandler>();
        
        var sp = Services!.BuildServiceProvider();

        return new TelegramBot(_client,
                               sp.GetRequiredService<IBotUpdateHandler>(),
                               sp.GetRequiredService<ILogger<TelegramBot>>(),
                               sp.GetRequiredService<MetricsProcessor>(),
                               SecureStorage!);
    }

    public override TelegramBotBuilder AddBotSettings<TBotSettings>(
        BotOptionsBuilder<TBotSettings> optionsBuilder)
    {
        Settings = optionsBuilder.Build() as TelegramBotSettings ?? throw new InvalidOperationException();

        return this;
    }
}