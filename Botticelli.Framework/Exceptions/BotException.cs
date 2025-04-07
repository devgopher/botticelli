namespace Botticelli.Framework.Exceptions;

public class BotException : Exception
{
    public BotException(string message, Exception? inner = null) : base(message, inner)
    {
    }
}