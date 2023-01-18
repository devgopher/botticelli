using System.Net.Http.Json;
using Botticelli.BotBase.Exceptions;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Framework;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Polly;

namespace Botticelli.BotBase;

/// <summary>
///     This service is intended for sending keepalive/hello messages
///     to Botticelli Admin server and receiving status messages from it
/// </summary>
internal class BotStatusService<TBot> : IHostedService
    where TBot : IBot
{
    private const short MaxRetryCount = 5;
    private const short PausePeriod = 5000;
    private readonly string? _botId;
    private readonly ManualResetEventSlim _getRequiredStatusEvent = new(false);
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ManualResetEventSlim _keepAliveEvent = new(false);
    private readonly ServerSettings _serverSettings;
    private readonly BotType _type;
    private Task _getRequiredStatusEventTask;
    private Task _keepAliveTask;
    private TBot _bot;

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
        var request = new RegisterBotRequest
        {
            BotId = _botId,
            Type = _bot.Type
        };

        var response =
            await InnerSend<RegisterBotRequest, RegisterBotResponse>(request,
                "/client/RegisterBot", cancellationToken);

        if (!response.IsSuccess)
            throw new BotException("Error starting a bot!");

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
        if (_keepAliveTask != default)
            return;

        _keepAliveEvent.Set();
        _keepAliveTask = Task.Factory.StartNew(async () =>
        {
            var request = new KeepAliveNotificationRequest
            {
                BotId = _botId
            };

            short unsuccessCounter = 0;

            while (!cancellationToken.IsCancellationRequested || _keepAliveEvent.IsSet)
            {
                var response =
                    await InnerSend<KeepAliveNotificationRequest, KeepAliveNotificationResponse>(request,
                        "/client/KeepAlive", cancellationToken);

                if (!response.IsSuccess)
                {
                    ++unsuccessCounter;
                    if (unsuccessCounter == MaxRetryCount)
                        // TODO: log
                        break;
                }

                Thread.Sleep(2 * PausePeriod);
            }
        }, cancellationToken);

        if (_keepAliveTask.IsFaulted)
            throw new BotException($"{nameof(KeepAlive)} exception: {_keepAliveTask.Exception?.Message}",
                _keepAliveTask.Exception);
    }

    /// <summary>
    /// Get required status for a bot from server
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <exception cref="BotException"></exception>
    private void GetRequiredStatus(CancellationToken cancellationToken)
    {
        if (_getRequiredStatusEventTask != default)
            return;

        _getRequiredStatusEvent.Set();
        _getRequiredStatusEventTask = Task.Factory.StartNew(async () =>
        {
            var request = new GetRequiredStatusFromServerRequest
            {
                BotId = _botId
            };

            short unsuccessCounter = 0;
            while (!cancellationToken.IsCancellationRequested || _getRequiredStatusEvent.IsSet)
            {
                try
                {
                    var response =
                        await InnerSend<GetRequiredStatusFromServerRequest, GetRequiredStatusFromServerResponse>(
                            request, "/client/GetRequiredBotStatus",
                            cancellationToken);

                    if (!response.IsSuccess)
                    {
                        ++unsuccessCounter;
                        if (unsuccessCounter == MaxRetryCount)
                            // TODO: log
                            break;
                    }
                    else
                    {
                        unsuccessCounter = 0;
                        switch (response.Status)
                        {
                            case BotStatus.Active:
                                await _bot.StartBotAsync(StartBotRequest.GetInstance(), cancellationToken);
                                break;
                            case BotStatus.NonActive:
                                await _bot.StopBotAsync(StopBotRequest.GetInstance(), cancellationToken);
                                break;
                                //default: log
                                //throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ++unsuccessCounter;

                    // throw new BotException("Error receiving bot status!", ex);
                }

                Thread.Sleep(PausePeriod);
            }
        }, cancellationToken);

        if (_getRequiredStatusEventTask.IsFaulted)
            throw new BotException($"{nameof(GetRequiredStatus)} exception: {_keepAliveTask.Exception?.Message}",
                _keepAliveTask.Exception);
    }

    private async Task<TResp> InnerSend<TReq, TResp>(TReq request,
        string funcName,
        CancellationToken cancellationToken)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(_serverSettings.ServerUri);


        var content = JsonContent.Create(request);

        var policyResult = await Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(5,
                _ => TimeSpan.FromMilliseconds(3000),
                async (result, timespan, retryNo, context) =>
                {
                    // log
                    Console.WriteLine("NONE");
                })
            .ExecuteAndCaptureAsync(async () => await httpClient.PostAsync(funcName, content, cancellationToken));

        if (policyResult.Result == null || !policyResult.Result.IsSuccessStatusCode)
            throw new BotException("Error sending request to a Botticelli server!");

        var responseJson = await policyResult.Result.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TResp>(responseJson);
    }
}