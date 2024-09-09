using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Options;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Builders;

public abstract class BotBuilder<TBot>
{
    protected abstract void Assert();

    public TBot Build()
    {
        Assert();

        return InnerBuild();
    }

    protected abstract TBot InnerBuild();
}

public abstract class BotBuilder<TBotBuilder, TBot> : BotBuilder<TBot>
        where TBotBuilder : BotBuilder<TBot>
{
    protected BotContext? BotContext;
    protected SecureStorage.SecureStorage? SecureStorage;
    protected IServiceCollection? Services;
    private readonly ServerSettings _serverSettings;
    private AnalyticsSettingsBuilder<AnalyticsSettings> analyticsSettingsBuilder;

    protected override void Assert()
    {
        if (SecureStorage == default) throw new NullReferenceException($"{nameof(SecureStorage)} is null!");
    }

    protected TBotBuilder AddServices(IServiceCollection services)
    {
        Services = services;

        return (this as TBotBuilder)!;
    }

    public abstract TBotBuilder AddBotSettings<TBotSettings>(BotOptionsBuilder<TBotSettings> optionsBuilder)
            where TBotSettings : BotSettings, new();


    public TBotBuilder AddStorage(SecureStorage.SecureStorage secureStorage)
    {
        SecureStorage = secureStorage;

        return (this as TBotBuilder)!;
    }
        
    public TBotBuilder AddAnalyticsSettings(AnalyticsSettingsBuilder<AnalyticsSettings> settingsBuilder)
    {
        analyticsSettingsBuilder = settingsBuilder;

        return (this as TBotBuilder)!;
    }
    
    public TBotBuilder AddServerSettings(ServerSettingsBuilder<ServerSettings> settingsBuilder)
    {
        analyticsSettingsBuilder = settingsBuilder;

        return (this as TBotBuilder)!;
    }
}