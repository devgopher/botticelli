using Botticelli.Framework.Events;
using Telegram.Bot.Polling;

namespace Botticelli.Framework.Telegram.Handlers;

public interface IBotUpdateHandler : IUpdateHandler
{
    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);

    public event MsgReceivedEventHandler MessageReceived;
}