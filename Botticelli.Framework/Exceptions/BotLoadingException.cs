namespace Botticelli.Framework.Exceptions;

public class BotLoadingException : Exception
{
    public BotLoadingException(string message, Exception? inner = null) : base(message, inner)
    {
    }
}