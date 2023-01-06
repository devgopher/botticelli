using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Interfaces;

public interface IEventBasedBotAdminApi
{
    public Task<PingResponse> PingAsync(PingRequest request);

    public void AddAdminEventProcessor(IAdminResponseProcessor responseProcessor);

    public Task<StartBotResponse> StartBotAsync(StartBotRequest request);
    public Task<StopBotResponse> StopBotAsync(StopBotRequest request);
}