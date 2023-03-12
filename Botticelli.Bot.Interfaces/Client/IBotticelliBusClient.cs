using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bot.Interfaces.Client;

/// <summary>
///     Bus client for sending/receiving req/resp through a bus
/// </summary>
public interface IBotticelliBusClient
{
    public Task<SendMessageResponse> GetResponse(SendMessageRequest request,
                                                 CancellationToken token);

    public Task SendResponse(SendMessageResponse response, CancellationToken token);
}