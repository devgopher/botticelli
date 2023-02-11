using Botticelli.Bus.Agent;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Pay
{
    public class PayClient : IBotticelliBusAgent
    {
        public async Task<SendMessageResponse> SendResponse(SendMessageRequest request, 
                                                            CancellationToken token, 
                                                            int timeoutMs = 10000) => throw new NotImplementedException();
    }
}