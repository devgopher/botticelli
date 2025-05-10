using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Builders;

public abstract class BotBuilder<TBot>
{
    protected abstract void Assert();

    public TBot? Build()
    {
        Assert();

        return InnerBuild();
    }

    protected abstract TBot? InnerBuild();
}

public abstract class BotBuilder<TBot, TBotBuilder> : BotBuilder<TBot>
        where TBotBuilder : BotBuilder<TBot, TBotBuilder>
{
    protected AnalyticsClientSettingsBuilder<AnalyticsClientSettings>? AnalyticsClientSettingsBuilder;
    protected DataAccessSettingsBuilder<DataAccessSettings>? BotDataAccessSettingsBuilder;
    protected ServerSettingsBuilder<ServerSettings>? ServerSettingsBuilder;
    protected IServiceCollection Services = null!;

    protected override void Assert()
    {
    }

    public BotBuilder<TBot, TBotBuilder> AddServices(IServiceCollection services)
    {
        Services = services;

        return this;
    }

    public BotBuilder<TBot, TBotBuilder> AddAnalyticsSettings(AnalyticsClientSettingsBuilder<AnalyticsClientSettings> clientSettingsBuilder)
    {
        AnalyticsClientSettingsBuilder = clientSettingsBuilder;

        return this;
    }

    protected BotBuilder<TBot, TBotBuilder> AddServerSettings(ServerSettingsBuilder<ServerSettings> settingsBuilder)
    {
        ServerSettingsBuilder = settingsBuilder;

        return this;
    }

    public BotBuilder<TBot, TBotBuilder> AddBotDataAccessSettings(DataAccessSettingsBuilder<DataAccessSettings> botDataAccessBuilder)
    {
        BotDataAccessSettingsBuilder = botDataAccessBuilder;

        return this;
    }
}