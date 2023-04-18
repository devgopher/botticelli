using System.Text;
using System.Text.Json;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bus.ZeroMQ.Exceptions;
using Botticelli.Bus.ZeroMQ.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using Polly;
using Polly.Timeout;

namespace Botticelli.Bus.ZeroMQ.Client;

public class ZeroMqClient<TBot> : BasicFunctions<TBot>, IBotticelliBusClient, IDisposable
        where TBot : IBot
{
    private readonly ILogger<ZeroMqClient<TBot>> _logger;
    private readonly Dictionary<string, SendMessageResponse> _responses = new(100);
    private readonly ZeroMqBusSettings _settings;
    private readonly TimeSpan _timeout;
    private ResponseSocket _responseSocket;
    private RequestSocket _requestSocket;
    private readonly int _delta = 50;

    public ZeroMqClient(ZeroMqBusSettings settings,
                        ILogger<ZeroMqClient<TBot>> logger)
    {
        _settings = settings;
        _logger = logger;
        _timeout = settings.Timeout;

        Init();
    }

    public async Task<SendMessageResponse> GetResponse(SendMessageRequest request, CancellationToken token)
    {
        try
        {
            var timeoutPolicy = Policy.TimeoutAsync<SendMessageResponse>(_timeout, TimeoutStrategy.Pessimistic);


            //var sendResult = await Policy.HandleResult<bool>(s => s)
            //                             .WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(50))
            //                             .ExecuteAsync(async s => _requestSocket.TrySendFrame(_settings.Timeout, JsonSerializer.SerializeToUtf8Bytes(request)), token);

            //if (sendResult == false) return SendMessageResponse.GetInstance("Can't send a message through ZeroMQ!");
            
            _requestSocket.SendFrame(JsonSerializer.SerializeToUtf8Bytes(request));

            var resultPolicy = Policy.HandleResult<SendMessageResponse>(s => s == null)
                                     .WaitAndRetryAsync(int.MaxValue, _ => TimeSpan.FromMilliseconds(50));

            var combined = Policy.WrapAsync(timeoutPolicy, resultPolicy);

            var result = await combined.ExecuteAndCaptureAsync(async () => !_responses.ContainsKey(request.Message.Uid) ? default : _responses[request.Message.Uid]);


            if (result.FinalHandledResult != default)
                throw new ZeroMqBusException($"Error getting a response: {result.FinalException.Message}",
                                             result.FinalException?.InnerException);

            return result?.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }
    }

    public async Task SendResponse(SendMessageResponse response, CancellationToken token)
    {
        try
        {
            //var timeoutPolicy = Policy.TimeoutAsync<bool>(_timeout, TimeoutStrategy.Pessimistic);

            //var sendResultPolicy = Policy.HandleResult<bool>(s => s == false)
            //                             .WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(50));


            //var sendResult = await Policy.WrapAsync(timeoutPolicy, sendResultPolicy)
            //                     .ExecuteAsync(async s => _requestSocket.TrySendFrame(_settings.Timeout,
            //                                                                           JsonSerializer.SerializeToUtf8Bytes(response),
            //                                                                           true), token);


            //if (sendResult == false) 
            //    throw new ZeroMqBusException("Can't send a response through ZeroMQ!");

            _requestSocket.SendFrame(JsonSerializer.SerializeToUtf8Bytes(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }
    }

    private void Init()
    {
        _responseSocket = new ResponseSocket(_settings.ListenUri);
        _requestSocket = new RequestSocket(_settings.TargetUri);
        
        _responseSocket.ReceiveReady += (sender, args) =>
        {
            var frame = args.Socket.ReceiveFrameString(Encoding.UTF8);
            var message = JsonSerializer.Deserialize<SendMessageResponse>(frame);

            if (message != default) _responses[message.MessageUid] = message;
        };
    }

    public void Dispose() => _responseSocket?.Dispose();
}