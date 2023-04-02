using Botticelli.Framework.Events;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Framework.Viber.WebHook
{
    public interface INotificator<TBot>
        where TBot : IBot
    {
        public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);
        public delegate void MessengerSpecificEventHandler(object sender, MessengerSpecificBotEventArgs<TBot> e);
        public event MsgReceivedEventHandler MessageReceived;
        public event MessengerSpecificEventHandler MessengerSpecificEvent;

        public void NotifyReceived(object sender, Message message);
        public void NotifyMessengerSpecific(object sender, string eventName, IEnumerable<string> args);
    }
}
