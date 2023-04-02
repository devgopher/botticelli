using Botticelli.Framework.Events;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Framework.Viber.WebHook;

internal class Notificator<TBot> : INotificator<TBot> where TBot : IBot
{
    public event INotificator<TBot>.MsgReceivedEventHandler? MessageReceived;
    public event INotificator<TBot>.MessengerSpecificEventHandler? MessengerSpecificEvent;
    public void NotifyReceived(object sender, Message message) 
        => MessageReceived?.Invoke(sender, new MessageReceivedBotEventArgs());

    public void NotifyMessengerSpecific(object sender, string eventName, IEnumerable<string> args) 
        => MessengerSpecificEvent?.Invoke(sender, new MessengerSpecificBotEventArgs<TBot>
        {
            EventName = eventName,
            Arguments = args
        });
}