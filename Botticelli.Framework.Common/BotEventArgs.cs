using System;

namespace Botticelli.Framework.Events
{
    public class BotEventArgs : EventArgs
    {
        public DateTime DateTime { get; } = DateTime.Now;
    }
}