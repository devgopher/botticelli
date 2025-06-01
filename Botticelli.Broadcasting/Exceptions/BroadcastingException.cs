namespace Botticelli.Broadcasting.Exceptions;

public class BroadcastingException(string message, Exception? ex = null) : Exception(message, ex);