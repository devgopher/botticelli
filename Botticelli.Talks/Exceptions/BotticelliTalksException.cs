namespace Botticelli.Talks.Exceptions;

public class BotticelliTalksException : Exception
{
    public BotticelliTalksException(string message, Exception? inner = null) : base(message, inner)
    {
    }
}