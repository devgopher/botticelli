using Botticelli.Framework.Events;
using Telegram.Bot.Polling;

namespace Botticelli.Framework.Telegram.Handlers;

public interface IBotUpdateHandler : IUpdateHandler
{
    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);
    public delegate void NewChatMembersHandler(object sender, NewChatMembersBotEventArgs e);
    public delegate void ContactSharedHandler(object sender, SharedContactBotEventArgs e);
    
    public void AddSubHandler<T>(T subHandler) where T : IBotUpdateSubHandler;

    public event MsgReceivedEventHandler MessageReceived;
    public event NewChatMembersHandler NewChatMembers;
    public event ContactSharedHandler ContactShared;
}
