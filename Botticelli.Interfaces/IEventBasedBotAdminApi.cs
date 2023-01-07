using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Interfaces;

/// <summary>
/// Administration API
/// </summary>
public interface IEventBasedBotAdminApi
{
    /// <summary>
    /// Ping
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<PingResponse> PingAsync(PingRequest request);

    /// <summary>
    /// Adds event processor
    /// </summary>
    /// <param name="requestProcessor"></param>
    public void AddAdminEventProcessor(IAdminRequestProcessor requestProcessor);

    /// <summary>
    /// Starts serving
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public Task<StartBotResponse> StartBotAsync(StartBotRequest request);

    /// <summary>
    /// Stops serving
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public Task<StopBotResponse> StopBotAsync(StopBotRequest request);
}