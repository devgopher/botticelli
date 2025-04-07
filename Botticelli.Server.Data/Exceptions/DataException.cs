namespace Botticelli.Server.Data.Exceptions;

public class DataException(string message, Exception? inner = null) : Exception(message, inner);