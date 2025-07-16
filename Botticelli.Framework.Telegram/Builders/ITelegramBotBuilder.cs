using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Builders;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram.Decorators;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Framework.Telegram.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Telegram.Builders;

public interface ITelegramBotBuilder<TBot, TBotBuilder> where TBot : TelegramBot where TBotBuilder : BotBuilder<TBot, TBotBuilder>
{
    ITelegramBotBuilder<TBot, TBotBuilder> AddSubHandler<T>()
        where T : class, IBotUpdateSubHandler;

    ITelegramBotBuilder<TBot, TBotBuilder> AddToken(string botToken);
    ITelegramBotBuilder<TBot, TBotBuilder> AddClient(TelegramClientDecoratorBuilder builder);
    TBot? Build();
    BotBuilder<TBot, TBotBuilder> AddServices(IServiceCollection services);
    BotBuilder<TBot, TBotBuilder> AddAnalyticsSettings(AnalyticsClientSettingsBuilder<AnalyticsClientSettings> clientSettingsBuilder);
    BotBuilder<TBot, TBotBuilder> AddBotDataAccessSettings(DataAccessSettingsBuilder<DataAccessSettings> botDataAccessBuilder);
    BotBuilder<TBot, TBotBuilder> AddOnMessageSent(BaseBot.MsgSentEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddOnMessageReceived(BaseBot.MsgReceivedEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddOnMessageRemoved(BaseBot.MsgRemovedEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddNewChatMembers(BaseBot.NewChatMembersEventHandler handler);
    BotBuilder<TBot, TBotBuilder> AddSharedContact(BaseBot.ContactSharedEventHandler handler);

    static abstract ITelegramBotBuilder<TBot, TBotBuilder> Instance(IServiceCollection services,
        ServerSettingsBuilder<ServerSettings> serverSettingsBuilder,
        BotSettingsBuilder<TelegramBotSettings> settingsBuilder,
        DataAccessSettingsBuilder<DataAccessSettings> dataAccessSettingsBuilder,
        AnalyticsClientSettingsBuilder<AnalyticsClientSettings> analyticsClientSettingsBuilder,
        bool isStandalone);
}