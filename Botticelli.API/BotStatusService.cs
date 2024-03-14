using System.Net.Http.Json;
using Botticelli.BotBase.Exceptions;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Flurl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace Botticelli.BotBase;

/// <summary>
///     This service is intended for sending keepalive/hello messages
///     to Botticelli Admin server and receiving status messages from it
/// </summary>
public class BotStatusService<TBot> : IHostedService
    where TBot : IBot
{
    private const short PausePeriod = 5000;
    private readonly string? _botId;
    private readonly ManualResetEventSlim _getRequiredStatusEvent = new(false);
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ManualResetEventSlim _keepAliveEvent = new(false);
    private readonly ILogger<BotStatusService<TBot>> _logger;
    private readonly ServerSettings _serverSettings;
    private TBot _bot;
    private Task _getRequiredStatusEventTask;
    private Task _keepAliveTask;

    public BotStatusService(IHttpClientFactory httpClientFactory,
        ServerSettings serverSettings,
        TBot bot,
        ILogger<BotStatusService<TBot>> logger)
    {
        _httpClientFactory = httpClientFactory;
        _serverSettings = serverSettings;
        _bot = bot;
        _logger = logger;
        _keepAliveEvent.Reset();
        _getRequiredStatusEvent.Reset();
        _botId = BotDataUtils.GetBotId();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        KeepAlive(cancellationToken);
        GetRequiredStatus(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _keepAliveEvent.Reset();
        _getRequiredStatusEvent.Reset();
    }

    /// <summary>
    ///     Sends KeepAlive messages
    /// </summary>
    /// <param name="cancellationToken" />
    /// <exception cref="BotException" />
    private void KeepAlive(CancellationToken cancellationToken)
    {
        if (_keepAliveTask != default) return;

        _keepAliveEvent.Set();
        var request = new KeepAliveNotificationRequest
        {
            BotId = _botId
        };

        _logger.LogDebug($"KeepAlive botId: {_botId}");
        
        _keepAliveTask = Policy.HandleResult<KeepAliveNotificationResponse>(_ => true)
            .WaitAndRetryForeverAsync(_ => TimeSpan.FromMilliseconds(PausePeriod))
            .ExecuteAndCaptureAsync(ct =>
                {

                    var response = InnerSend<KeepAliveNotificationRequest, KeepAliveNotificationResponse>(
                        request,
                        "/bot/client/KeepAlive",
                        ct);

                    _logger.LogDebug($"KeepAlive botId: {_botId} response: {response.Result.BotId}, {response.Result.IsSuccess}");
                    
                    return response;
                },
                cancellationToken);

        if (!_keepAliveTask.IsFaulted)
        {
            _logger.LogTrace($"{nameof(KeepAlive)} sent for bot: {_botId}");

            return;
        }

        _logger.LogError($"KeepAlive error: {_keepAliveTask.Exception?.Message}");

        throw new BotException($"{nameof(KeepAlive)} exception: {_keepAliveTask.Exception?.Message}",
            _keepAliveTask.Exception);
    }

    /// <summary>
    ///     Get required status for a bot from server
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="BotException"></exception>
    private void GetRequiredStatus(CancellationToken cancellationToken)
    {
        if (_getRequiredStatusEventTask != default) return;

        _getRequiredStatusEvent.Set();
        var request = new GetRequiredStatusFromServerRequest
        {
            BotId = _botId
        };


        _getRequiredStatusEventTask = Policy.HandleResult<GetRequiredStatusFromServerResponse>(r => true)
            .WaitAndRetryForeverAsync(_ => TimeSpan.FromMilliseconds(5 * PausePeriod))
            .ExecuteAndCaptureAsync(ct =>
                {
                    var task = InnerSend<GetRequiredStatusFromServerRequest, GetRequiredStatusFromServerResponse>(
                        request,
                        "/bot/client/GetRequiredBotStatus",
                        ct);

                    task.Wait(cancellationToken);
                    _bot.SetBotContext(task.Result?.BotContext, ct);

                    if (task.Exception != default)
                    {
                        _logger.LogError($"GetRequiredStatus task error: {task.Exception?.Message}");
                        _bot.StopBotAsync(StopBotRequest.GetInstance(), ct);

                        return task;
                    }

                    switch (task.Result?.Status)
                    {
                        case BotStatus.Unlocked:
                            _bot.StartBotAsync(StartBotRequest.GetInstance(), ct);

                            break;
                        case BotStatus.Locked:
                        case BotStatus.Unknown:
                        case null:
                            _bot.StopBotAsync(StopBotRequest.GetInstance(), ct);

                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }

                    return task;
                },
                cancellationToken);
    }

    /// <summary>
    ///     Inner send
    /// </summary>
    /// <typeparam name="TReq">Request</typeparam>
    /// <typeparam name="TResp">Response</typeparam>
    /// <param name="request">Request</param>
    /// <param name="funcName">Response</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    private async Task<TResp> InnerSend<TReq, TResp>(TReq request,
        string funcName,
        CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();

            var content = JsonContent.Create(request);

            _logger.LogDebug($"InnerSend request: {JsonConvert.SerializeObject(request)}");
            
            var response = await httpClient.PostAsync(Url.Combine(_serverSettings.ServerUri, funcName), content,
                cancellationToken);

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug($"InnerSend response {responseJson}");
            
            return JsonConvert.DeserializeObject<TResp>(responseJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return default;
    }
}