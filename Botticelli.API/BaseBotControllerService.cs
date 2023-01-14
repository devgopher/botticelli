using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.BotBase;

public class BaseBotControllerService : IBotControllerService
{
    public Task<StartBotResponse> StartAsync(StartBotRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<StopBotResponse> StopAsync(StopBotRequest request)
    {
        throw new NotImplementedException();
    }
}