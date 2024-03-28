﻿using System.Net.Http.Json;
using Botticelli.BotBase.Settings;
using Botticelli.BotBase.Utils;
using Botticelli.Interfaces;
using Flurl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Botticelli.BotBase;

/// <summary>
///     This service is intended for sending keepalive/hello messages
///     to Botticelli Admin server and receiving status messages from it
/// </summary>
public abstract class BotActualizationService<TBot> : IHostedService
        where TBot : IBot
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ServerSettings _serverSettings;
    protected readonly string? BotId;
    protected readonly ILogger Logger;
    protected TBot Bot;

    protected BotActualizationService(IHttpClientFactory httpClientFactory,
                                      ServerSettings serverSettings,
                                      TBot bot,
                                      ILogger<BotActualizationService<TBot>> logger)
    {
        _httpClientFactory = httpClientFactory;
        _serverSettings = serverSettings;
        Bot = bot;
        Logger = logger;
        BotId = BotDataUtils.GetBotId();
    }

    public abstract Task StartAsync(CancellationToken cancellationToken);

    public abstract Task StopAsync(CancellationToken cancellationToken);


    /// <summary>
    ///     Inner send
    /// </summary>
    /// <typeparam name="TReq">Request</typeparam>
    /// <typeparam name="TResp">Response</typeparam>
    /// <param name="request">Request</param>
    /// <param name="funcName">Response</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    protected async Task<TResp> InnerSend<TReq, TResp>(TReq request,
                                                       string funcName,
                                                       CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();

            var content = JsonContent.Create(request);

            Logger.LogDebug($"InnerSend request: {JsonConvert.SerializeObject(request)}");

            var response = await httpClient.PostAsync(Url.Combine(_serverSettings.ServerUri, funcName),
                                                      content,
                                                      cancellationToken);

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            Logger.LogDebug($"InnerSend response {responseJson}");

            return JsonConvert.DeserializeObject<TResp>(responseJson);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
        }

        return default;
    }
}