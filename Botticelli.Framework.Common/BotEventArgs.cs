using System;
using Botticelli.Interfaces;

namespace Botticelli.Framework.Events
{
    public delegate void MessengerSpecificEventHandler<T>(object sender, MessengerSpecificBotEventArgs<T> e) 
        where T : IBot;

    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);

    public delegate void MsgRemovedEventHandler(object sender, MessageRemovedBotEventArgs e);

    public delegate void MsgSentEventHandler(object sender, MessageSentBotEventArgs e);

    public delegate void StartedEventHandler(object sender, StartedBotEventArgs e);

    public delegate void StoppedEventHandler(object sender, StoppedBotEventArgs e);

    public class BotEventArgs : EventArgs
    {
        public DateTime DateTime { get; } = DateTime.Now;
    }
}