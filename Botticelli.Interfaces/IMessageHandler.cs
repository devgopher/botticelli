namespace Botticelli.Interfaces;

public interface IMessageHandler
{
    public void AddClientEventProcessor(IClientMessageProcessor messageProcessor);
}