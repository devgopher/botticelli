using Botticelli.Framework.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Services;

public class BotKeepAliveService(
        IHttpClientFactory httpClientFactory,
        ServerSettings serverSettings,
        IBot bot,
        ILogger<BotActualizationService> logger)
        : PollActualizationService<KeepAliveNotificationRequest, KeepAliveNotificationResponse>(httpClientFactory,
                                                                                                "keepalive",
                                                                                                serverSettings,
                                                                                                bot,
                                                                                                logger);