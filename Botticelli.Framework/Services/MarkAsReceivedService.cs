using Botticelli.Framework.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Services;

/// <summary>
///     Marks a broadcast message as received
/// </summary>
/// <param name="httpClientFactory"></param>
/// <param name="serverSettings"></param>
/// <param name="bot"></param>
/// <param name="logger"></param>
/// <typeparam name="TBot"></typeparam>
public class MarkAsReceivedService<TBot>(
        IHttpClientFactory httpClientFactory,
        ServerSettings serverSettings,
        IBot bot,
        ILogger<BotActualizationService> logger)
        : PollActualizationService<MarkAsReceivedRequest, MarksAsReceivedResponse>(httpClientFactory,
                                                                                   "broadcast",
                                                                                   serverSettings,
                                                                                   bot,
                                                                                   logger)
{
    private readonly IBot _bot = bot;

    protected override async Task InnerProcess(MarksAsReceivedResponse response, CancellationToken ct)
    {
    }
}