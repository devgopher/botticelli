namespace Botticelli.AI.Exceptions;

public class AiException : Exception
{
    public AiException(string message, Exception? inner = null) : base(message, inner)
    {
    }
}