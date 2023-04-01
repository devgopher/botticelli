namespace Botticelli.Interfaces;

/// <summary>
///     Client API request processor
/// </summary>
public interface IClientMessageProcessor : IMessageProcessor
{
    public void AddBot(IBot bot);
    public void SetServiceProvider(IServiceProvider sp);
}