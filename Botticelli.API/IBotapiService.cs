using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.BotBase
{
    /// <summary>
    /// This service interface is intended for starting/stopping your bot from
    /// Api Controller 
    /// </summary>
    public interface IBotApiService
    {
        public Task<StartBotResponse> StartAsync(StartBotRequest request);
        public Task<StopBotResponse> StopAsync(StopBotRequest request);
    }


    public class BaseBotApiService : IBotApiService
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
}
