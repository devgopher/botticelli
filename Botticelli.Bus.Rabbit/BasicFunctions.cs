using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.Rabbit;

public class BasicFunctions<TBot>
{
    protected static string GetRequestQueueName()
    {
        return $"{nameof(SendMessageRequest)}_{typeof(TBot).Name}_request";
    }

    protected static string GetResponseQueueName()
    {
        return $"{nameof(SendMessageResponse)}_{typeof(TBot).Name}_response";
    }
}