namespace Botticelli.Bus.Rabbit.Exceptions;

public class RabbitBusException : Exception
{
    public RabbitBusException(string message, Exception? ex = null) : base(message, ex)
    {
    }
}