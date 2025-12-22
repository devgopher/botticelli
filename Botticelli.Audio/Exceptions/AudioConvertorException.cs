namespace Botticelli.Audio.Exceptions;

public class AudioConvertorException(string message, Exception ex) : Exception(message, ex);