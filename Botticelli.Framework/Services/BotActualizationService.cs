using System.Net.Http.Json;
using Botticelli.Bot.Utils;
using Botticelli.Framework.Options;
using Botticelli.Interfaces;
using Flurl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Services;

/// <summary>
///     This service is intended for sending keepalive/hello messages
///     to Botticelli Admin server and receiving status messages from it
/// </summary>
public abstract class BotActualizationService : IHostedService
{
    protected readonly ManualResetEventSlim ActualizationEvent = new(false);
    protected readonly IBot Bot;
    protected readonly string? BotId = BotDataUtils.GetBotId();
    protected readonly IHttpClientFactory HttpClientFactory;
    protected readonly ILogger Logger;
    protected readonly ServerSettings? ServerSettings;

    /// <summary>
    ///     This service is intended for sending keepalive/hello messages
    ///     to Botticelli Admin server and receiving status messages from it
    /// </summary>
    protected BotActualizationService(IHttpClientFactory httpClientFactory,
                                      IBot bot,
                                      ILogger logger)
    {
        HttpClientFactory = httpClientFactory;
        Bot = bot;
        Logger = logger;

        ActualizationEvent.Reset();
    }


    /// <summary>
    ///     This service is intended for sending keepalive/hello messages
    ///     to Botticelli Admin server and receiving status messages from it
    /// </summary>
    protected BotActualizationService(IHttpClientFactory httpClientFactory,
                                      IBot bot,
                                      ILogger logger,
                                      ServerSettings? serverSettings)
    {
        ServerSettings = serverSettings;
        HttpClientFactory = httpClientFactory;
        Bot = bot;
        Logger = logger;

        ActualizationEvent.Reset();
    }

    public abstract Task StartAsync(CancellationToken cancellationToken);

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        ActualizationEvent.Reset();

        return Task.CompletedTask;
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
    protected virtual async Task<TResp?> InnerSendPost<TReq, TResp>(TReq request,
                                                                    string funcName,
                                                                    CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = HttpClientFactory.CreateClient();

            var content = JsonContent.Create(request);

            Logger.LogDebug("InnerSend request: {Request}", request);

            var response = await httpClient.PostAsync(Url.Combine(ServerSettings?.ServerUri, funcName),
                                                      content,
                                                      cancellationToken);

            return await response.Content.ReadFromJsonAsync<TResp>(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
        }

        return default;
    }
}