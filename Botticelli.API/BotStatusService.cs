using System.Net.Http.Json;
using Botticelli.BotBase.Exceptions;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Microsoft.Extensions.Hosting;
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
    private readonly ServerSettings _serverSettings;
    private readonly BotType _type;
    private TBot _bot;
    private Task _getRequiredStatusEventTask;
    private Task _keepAliveTask;

    public BotStatusService(IHttpClientFactory httpClientFactory,
                            ServerSettings serverSettings,
                            TBot bot)
    {
        _httpClientFactory = httpClientFactory;
        _serverSettings = serverSettings;
        _bot = bot;
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

        _keepAliveTask = Policy.HandleResult<KeepAliveNotificationResponse>(r => true)
                               .WaitAndRetryForeverAsync(_ => TimeSpan.FromMilliseconds(PausePeriod))
                               .ExecuteAndCaptureAsync(() => InnerSend<KeepAliveNotificationRequest, KeepAliveNotificationResponse>(request,
                                                                                                                                    "/client/KeepAlive",
                                                                                                                                    cancellationToken));

        if (_keepAliveTask.IsFaulted)
            throw new BotException($"{nameof(KeepAlive)} exception: {_keepAliveTask.Exception?.Message}",
                                   _keepAliveTask.Exception);
    }

    /// <summary>
    ///     Get required status for a bot from server
    /// </summary>
    /// <param name="cancellationToken"></param>
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
                                            .ExecuteAndCaptureAsync(() =>
                                            {
                                                var result = InnerSend<GetRequiredStatusFromServerRequest, GetRequiredStatusFromServerResponse>(request,
                                                                                                                                                "/client/GetRequiredBotStatus",
                                                                                                                                                cancellationToken);

                                                switch (result.Result.Status)
                                                {
                                                    case BotStatus.Active:
                                                        _bot.StartBotAsync(StartBotRequest.GetInstance(), cancellationToken);

                                                        break;
                                                    case BotStatus.NonActive:
                                                        _bot.StopBotAsync(StopBotRequest.GetInstance(), cancellationToken);

                                                        break;
                                                    case BotStatus.Unknown: break;
                                                    case null:              break;
                                                    default:                throw new ArgumentOutOfRangeException();
                                                }

                                                return result;
                                            });
    }

    private async Task<TResp> InnerSend<TReq, TResp>(TReq request,
                                                     string funcName,
                                                     CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_serverSettings.ServerUri);

            var content = JsonContent.Create(request);

            var response = await httpClient.PostAsync(funcName, content, cancellationToken);

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonConvert.DeserializeObject<TResp>(responseJson);
        }
        catch (Exception ex)
        {
            // todo: logging
        }

        return default;
    }
}