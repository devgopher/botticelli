using Botticelli.Shared.API.Client.Requests;

namespace Botticelli.Bus.None.Bus;

public static class NoneBus
{
    public static Queue<SendMessageRequest> SendMessageRequests => new();
    public static Queue<SendMessageRequest> SendMessageResponses => new();
}