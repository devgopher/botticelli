using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;

namespace Botticelli.Framework;

/// <summary>
///     A base class for bot
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseBot<T> : IBot<T>
        where T : BaseBot<T>
{
    public BaseBot() => isStarted = false;

    protected bool isStarted { get; set; }

    public async Task<PingResponse> PingAsync(PingRequest request) => PingResponse.GetInstance(request.Uid);

    public virtual async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
    {
        StartBotResponse response;

        try
        {
            response = StartBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.OK);
        }
        catch (Exception ex)
        {
            response = StartBotResponse.GetInstance(request.Uid, $"Error: {ex.Message}", AdminCommandStatus.FAIL);
        }

        return response;
    }

    public virtual async Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token)
    {
        StopBotResponse response;

        try
        {
            response = StopBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.OK);
        }
        catch (Exception ex)
        {
            response = StopBotResponse.GetInstance(request.Uid, $"Error: {ex.Message}", AdminCommandStatus.FAIL);
        }

        isStarted = false;

        return response;
    }

    /// <summary>
    ///     Sends a message
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public abstract Task<SendMessageResponse> SendAsync(SendMessageRequest request, CancellationToken token);

    public abstract BotType Type { get; }
}