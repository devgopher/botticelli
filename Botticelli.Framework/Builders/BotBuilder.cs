using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Builders;

public abstract class BotBuilder<TBot>
{
    protected abstract void Assert();

    public virtual TBot? Build()
    {
        Assert();

        return InnerBuild();
    }

    protected abstract TBot? InnerBuild();
}

public interface IBotBuilder<TBot, TBotBuilder> where TBotBuilder : BotBuilder<TBot, TBotBuilder>
{
    BotBuilder<TBot, TBotBuilder> AddServices(IServiceCollection services);
    BotBuilder<TBot, TBotBuilder> AddAnalyticsSettings(AnalyticsClientSettingsBuilder<AnalyticsClientSettings> clientSettingsBuilder);
    BotBuilder<TBot, TBotBuilder> AddBotDataAccessSettings(DataAccessSettingsBuilder<DataAccessSettings> botDataAccessBuilder);
    BotBuilder<TBot, TBotBuilder> AddOnMessageSent(BaseBot.MsgSentEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddOnMessageReceived(BaseBot.MsgReceivedEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddOnMessageRemoved(BaseBot.MsgRemovedEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddNewChatMembers(BaseBot.NewChatMembersEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddSharedContact(BaseBot.ContactSharedEventHandler handler);
    TBot? Build();
}

public abstract class BotBuilder<TBot, TBotBuilder> : BotBuilder<TBot>, IBotBuilder<TBot, TBotBuilder> 
    where TBotBuilder : BotBuilder<TBot, TBotBuilder>
{
    protected AnalyticsClientSettingsBuilder<AnalyticsClientSettings>? AnalyticsClientSettingsBuilder;
    protected DataAccessSettingsBuilder<DataAccessSettings>? BotDataAccessSettingsBuilder;
    protected ServerSettingsBuilder<ServerSettings>? ServerSettingsBuilder;
    public IServiceCollection Services = null!;

    protected BaseBot.MsgSentEventHandler? MessageSent;
    protected BaseBot.MsgReceivedEventHandler? MessageReceived;
    protected BaseBot.MsgRemovedEventHandler? MessageRemoved;
    protected BaseBot.ContactSharedEventHandler? SharedContact;
    protected BaseBot.NewChatMembersEventHandler? NewChatMembers;
    
    protected override void Assert()
    {
    }

    public virtual BotBuilder<TBot, TBotBuilder> AddServices(IServiceCollection services)
    {
        Services = services;

        return this;
    }

    public virtual BotBuilder<TBot, TBotBuilder> AddAnalyticsSettings(AnalyticsClientSettingsBuilder<AnalyticsClientSettings> clientSettingsBuilder)
    {
        AnalyticsClientSettingsBuilder = clientSettingsBuilder;

        return this;
    }

    protected virtual BotBuilder<TBot, TBotBuilder> AddServerSettings(ServerSettingsBuilder<ServerSettings> settingsBuilder)
    {
        ServerSettingsBuilder = settingsBuilder;

        return this;
    }

    public virtual  BotBuilder<TBot, TBotBuilder> AddBotDataAccessSettings(DataAccessSettingsBuilder<DataAccessSettings> botDataAccessBuilder)
    {
        BotDataAccessSettingsBuilder = botDataAccessBuilder;

        return this;
    }
    
    public virtual  BotBuilder<TBot, TBotBuilder> AddOnMessageSent(BaseBot.MsgSentEventHandler handler)
    {
        MessageSent += handler;

        return this;
    }
 
    public  virtual BotBuilder<TBot, TBotBuilder> AddOnMessageReceived(BaseBot.MsgReceivedEventHandler handler)
    {
        MessageReceived += handler;

        return this;
    }
    
    public virtual  BotBuilder<TBot, TBotBuilder> AddOnMessageRemoved(BaseBot.MsgRemovedEventHandler handler)
    {
        MessageRemoved += handler;

        return this;
    }
    
    public virtual  BotBuilder<TBot, TBotBuilder> AddNewChatMembers(BaseBot.NewChatMembersEventHandler handler)
    {
        NewChatMembers += handler;

        return this;
    }
    
    public  virtual BotBuilder<TBot, TBotBuilder> AddSharedContact(BaseBot.ContactSharedEventHandler handler)
    {
        SharedContact += handler;

        return this;
    }
}