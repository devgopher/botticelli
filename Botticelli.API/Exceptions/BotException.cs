namespace Botticelli.BotBase.Exceptions;

public class BotException : Exception
{
    public BotException(string message, Exception inner = default) : base(message, inner)
    {
    }
}