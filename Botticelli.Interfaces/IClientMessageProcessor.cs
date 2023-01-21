namespace Botticelli.Interfaces;

/// <summary>
///     Client API request processor
/// </summary>
public interface IClientMessageProcessor : IMessageProcessor
{
    public void SetBot(IBot bot);
}