using System.Text.Json;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.ZeroMQ;
using Botticelli.Bus.ZeroMQ.Settings;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using NetMQ;
using NetMQ.Sockets;

namespace Botticelli.Bus.ZeroMQTests.Mocks;

public class AgentMock<TBot, THandler> : BasicFunctions<TBot>, IBotticelliBusAgent<THandler>
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly ResponseSocket _responseSocket;

    public AgentMock(ZeroMqBusSettings settings) => _responseSocket = new ResponseSocket(settings.ListenUri);


    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

    public async Task SendResponseAsync(SendMessageResponse response,
                                        CancellationToken token,
                                        int timeoutMs = 10000)
        => _responseSocket.SendFrame(JsonSerializer.SerializeToUtf8Bytes(response));
}