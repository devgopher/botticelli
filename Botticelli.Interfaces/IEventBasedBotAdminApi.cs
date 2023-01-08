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

    ///// <summary>
    ///// Adds event processor
    ///// </summary>
    ///// <param name="messageProcessor"></param>
    //public void AddAdminEventProcessor(IAdminMessageProcessor messageProcessor);

    /// <summary>
    /// Starts serving
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token);

    /// <summary>
    /// Stops serving
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token);
}