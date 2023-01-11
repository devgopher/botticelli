using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.BotBase;

public abstract class BotApiControllerBase : ControllerBase
{
    protected readonly IBotApiService _botApiService;

    public BotApiControllerBase(IBotApiService botApiService)
    {
        _botApiService = botApiService;
    }

    /// <summary>
    ///     Starts bot functioning
    /// </summary>
    /// <returns></returns>
    [Route("admin/[action]")]
    public abstract Task<StartBotResponse> Start(StartBotRequest request);

    /// <summary>
    ///     Stops bot functioning
    /// </summary>
    /// <returns></returns>
    [Route("admin/[action]")]
    public abstract Task<StopBotResponse> Stop(StopBotRequest request);
}