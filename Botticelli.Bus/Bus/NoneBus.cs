using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.None.Bus;

public static class NoneBus
{
    static NoneBus()
    {
        SendMessageResponses = new Queue<SendMessageResponse>();
        SendMessageRequests = new Queue<SendMessageRequest>();
    }

    public static Queue<SendMessageRequest> SendMessageRequests { get; }
    public static Queue<SendMessageResponse> SendMessageResponses { get; }
}