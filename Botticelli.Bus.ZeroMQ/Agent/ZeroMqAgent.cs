using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Bus.Handlers;
using Botticelli.Bus.ZeroMQ.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;

namespace Botticelli.Bus.ZeroMQ.Agent;

/// <summary>
///     ZeroMqMQ agent
/// </summary>
/// <typeparam name="TBot" />
/// <typeparam name="THandler"></typeparam>
public class ZeroMqAgent<TBot, THandler> : BasicFunctions<TBot>, IBotticelliBusAgent<THandler>, IDisposable
        where TBot : IBot
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly IList<THandler> _handlers = new List<THandler>(5);
    private readonly ILogger<ZeroMqAgent<TBot, THandler>> _logger;
    private readonly ConcurrentQueue<SendMessageRequest> _requests = new();
    private readonly ZeroMqBusSettings _settings;
    private readonly IServiceProvider _sp;
    private bool _isActive;
    private RequestSocket _requestSocket;
    private ResponseSocket _responseSocket;

    public ZeroMqAgent(IServiceProvider sp,
                       ZeroMqBusSettings settings,
                       ILogger<ZeroMqAgent<TBot, THandler>> logger)
    {
        _sp = sp;
        _settings = settings;
        _logger = logger;

        Init();
    }

    /// <summary>
    ///     Returns response to a bus
    /// </summary>
    /// <param name="response"></param>
    /// <param name="token"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public async Task SendResponseAsync(SendMessageResponse response,
                                        CancellationToken token,
                                        int timeoutMs = 60000)
    {
        try
        {
            _logger.LogDebug($"{nameof(SendResponseAsync)}({response.Uid}) start...");

            await InnerSend(response);

            _logger.LogDebug($"{nameof(SendResponseAsync)}({response.Uid}) finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending a response: {ex.Message}");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _isActive = true;

        Task.Run(() => ProcessSubscriptions(cancellationToken), cancellationToken);

        await Subscribe(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _isActive = false;
        Thread.Sleep(3000);
    }

    public void Dispose()
    {
        _requestSocket.Dispose();
        _responseSocket.Dispose();
    }

    /// <summary>
    ///     Subscribes with a new handler
    /// </summary>
    public async Task Subscribe(CancellationToken token)
    {
        _logger.LogDebug($"{nameof(Subscribe)}({typeof(THandler).Name}) start...");

        var handler = _sp.GetRequiredService<THandler>();
        _handlers.Add(handler);
    }

    private async Task ProcessSubscriptions(CancellationToken token)
    {
        while (token is {CanBeCanceled: true, IsCancellationRequested: true})
        {
            if (!_requests.TryDequeue(out var message)) continue;

            if (!_handlers.Any())
            {
                Thread.Sleep(5);

                continue;
            }

            foreach (var handler in _handlers) await handler?.Handle(message, token);
        }
    }

    private void Init()
    {
        _requestSocket = new RequestSocket(_settings.ListenUri);
        _responseSocket = new ResponseSocket(_settings.TargetUri);

        _responseSocket.ReceiveReady += async (_, args) =>
        {
            var frame = await args.Socket.ReceiveFrameStringAsync(Encoding.UTF8);
            var request = JsonSerializer.Deserialize<SendMessageRequest>(frame.Item1);

            _requests.Enqueue(request);
        };
    }

    private async Task InnerSend(SendMessageResponse response)
    {
        try
        {
            _requestSocket.SendFrame(JsonSerializer.SerializeToUtf8Bytes(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }
    }
}