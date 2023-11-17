namespace Botticelli.Audio.Exceptions;

public class AudioConvertorException : Exception
{
    public AudioConvertorException(string message, Exception ex) : base(message, ex)
    {
    }
}