﻿using Botticelli.BotBase.Exceptions;
using Botticelli.BotBase.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;
using Polly;

namespace Botticelli.BotBase;

public class BotStatusService<TBot> : BotActualizationService<TBot> where TBot : IBot
{
    private const short GetStatusPeriod = 15000;
    private readonly ManualResetEventSlim _getRequiredStatusEvent = new(false);
    private Task _getRequiredStatusEventTask;

    public BotStatusService(IHttpClientFactory httpClientFactory,
                            ServerSettings serverSettings,
                            TBot bot,
                            ILogger<BotActualizationService<TBot>> logger) : base(httpClientFactory,
                                                                                  serverSettings,
                                                                                  bot,
                                                                                  logger)
    {
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        GetRequiredStatus(cancellationToken);

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _getRequiredStatusEvent.Reset();

        return Task.CompletedTask;
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
            BotId = BotId
        };

        _getRequiredStatusEventTask = Policy.HandleResult<GetRequiredStatusFromServerResponse>(r => true)
                                            .WaitAndRetryForeverAsync(_ => TimeSpan.FromMilliseconds(GetStatusPeriod))
                                            .ExecuteAndCaptureAsync(ct =>
                                                                    {
                                                                        var task = InnerSend<GetRequiredStatusFromServerRequest, GetRequiredStatusFromServerResponse>(request,
                                                                                                                                                                      "/bot/client/GetRequiredBotStatus",
                                                                                                                                                                      ct);

                                                                        task.Wait(cancellationToken);
                                                                        Bot.SetBotContext(task.Result?.BotContext, ct);

                                                                        if (task.Exception != default)
                                                                        {
                                                                            Logger.LogError($"GetRequiredStatus task error: {task.Exception?.Message}");
                                                                            Bot.StopBotAsync(StopBotRequest.GetInstance(), ct);

                                                                            return task;
                                                                        }

                                                                        switch (task.Result?.Status)
                                                                        {
                                                                            case BotStatus.Unlocked:
                                                                                Bot.StartBotAsync(StartBotRequest.GetInstance(), ct);

                                                                                break;
                                                                            case BotStatus.Locked:
                                                                            case BotStatus.Unknown:
                                                                            case null:
                                                                                Bot.StopBotAsync(StopBotRequest.GetInstance(), ct);

                                                                                break;
                                                                            default: throw new ArgumentOutOfRangeException();
                                                                        }

                                                                        return task;
                                                                    },
                                                                    cancellationToken);
    }
}