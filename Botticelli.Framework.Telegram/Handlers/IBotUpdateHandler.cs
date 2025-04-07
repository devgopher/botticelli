using Botticelli.Framework.Events;
using Telegram.Bot.Polling;

namespace Botticelli.Framework.Telegram.Handlers;

public interface IBotUpdateHandler : IUpdateHandler
{
    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);

    public void AddSubHandler<T>(T subHandler) where T : IBotUpdateSubHandler;

    public event MsgReceivedEventHandler MessageReceived;
}