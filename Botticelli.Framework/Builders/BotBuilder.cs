using Botticelli.Client.Analytics;
using Botticelli.Framework.Options;
using Botticelli.SecureStorage.Settings;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Builders;

public abstract class BotBuilder<TBot>
{
    protected abstract void Assert();

    protected abstract SecureStorageSettings GetStorageSettings();

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
    public IConfiguration? Configuration;
    protected MetricsProcessor? MetricsProcessor;
    protected SecureStorage.SecureStorage? SecureStorage;
    protected IServiceCollection? Services;
    
    protected override void Assert()
    {
        if (SecureStorage == default)
            throw new NullReferenceException($"{nameof(SecureStorage)} is null!");
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

        return  (this as TBotBuilder)!;
    }

    public TBotBuilder AddStorage()
    {
        SecureStorage = new SecureStorage.SecureStorage(GetStorageSettings());

        return  (this as TBotBuilder)!;
    }
}