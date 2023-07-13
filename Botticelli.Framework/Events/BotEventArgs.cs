namespace Botticelli.Framework.Events;

public class BotEventArgs : EventArgs
{
    public BotEventArgs() => DateTime = DateTime.Now;
    public DateTime DateTime { get; }
}