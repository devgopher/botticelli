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
        public Task<StartBotResponse> Start(StartBotRequest request);
        public Task<StopBotResponse> Stop(StopBotRequest request);


    }
}
