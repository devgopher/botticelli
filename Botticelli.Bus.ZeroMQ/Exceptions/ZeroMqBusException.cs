namespace Botticelli.Bus.ZeroMQ.Exceptions;

public class ZeroMqBusException : Exception
{
    public ZeroMqBusException(string message, Exception ex = default) : base(message, ex)
    {
    }
}