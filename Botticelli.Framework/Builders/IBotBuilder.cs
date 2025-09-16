using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Builders;

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
    TBot? Build(IServiceProvider serviceProvider);
}