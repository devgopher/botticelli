using System.Configuration;
using Botticelli.Bot.Data.Settings;
using Botticelli.Framework.Options;
using Botticelli.Framework.Security;
using Botticelli.Framework.Services;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Telegram.Builders;

/// <summary>
///     Builder for a standalone telegram bot
/// </summary>
/// <typeparam name="TBot"></typeparam>
public class TelegramStandaloneBotBuilder<TBot> : TelegramBotBuilder<TBot, TelegramStandaloneBotBuilder<TBot>>
    where TBot : TelegramBot
{
    public static TelegramStandaloneBotBuilder<TBot> Instance(IServiceCollection services,
        BotSettingsBuilder<TelegramBotSettings> settingsBuilder,
        DataAccessSettingsBuilder<DataAccessSettings> dataAccessSettingsBuilder) =>
        (TelegramStandaloneBotBuilder<TBot>)new TelegramStandaloneBotBuilder<TBot>()
            .AddBotSettings(settingsBuilder)
            .AddServices(services)
            .AddBotDataAccessSettings(dataAccessSettingsBuilder);

    public TelegramStandaloneBotBuilder<TBot> AddBotData(
        BotDataSettingsBuilder<BotDataSettings> dataBuilder)
    {
        var settings = dataBuilder.Build();

        BotData = new BotData.Entities.Bot.BotData
        {
            BotId = settings.BotId ?? throw new ConfigurationErrorsException("No BotId in bot settings!"),
            Status = BotStatus.Unlocked,
            Type = BotType.Telegram,
            BotKey = settings.BotKey ?? throw new ConfigurationErrorsException("No BotKey in bot settings!")
        };

        return this;
    }

    protected override TBot? InnerBuild()
    {
        Services.AddHttpClient<BotStandaloneService>()
            .AddServerCertificates(BotSettings);

        if (BotData == null)
            throw new ConfigurationErrorsException("BotData is null!");

        Services.AddHostedService<BotStandaloneService>()
            .AddSingleton(BotData);

        return base.InnerBuild();
    }
}