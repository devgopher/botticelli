using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bot.Interfaces.Client;

/// <summary>
///     Event-based client
/// </summary>
public interface IEventBusClient : IDisposable
{
    public delegate void BusEventHandler(object sender, SendMessageResponse e);

    public event BusEventHandler OnReceived;

    public void Send(SendMessageRequest request);
}