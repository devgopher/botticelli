using Botticelli.Shared.API.Client.Requests;

namespace Botticelli.Bus.Rabbit;

public class BasicFunctions<TBot>
{
    protected static string GetRequestQueueName() => $"{nameof(SendMessageRequest)}_{typeof(TBot).Name}_request";
    protected static string GetResponseQueueName() => $"{nameof(SendMessageRequest)}_{typeof(TBot).Name}_response";
    protected static string GetRkName() => $"{nameof(SendMessageRequest)}_{typeof(TBot).Name}_response_rk";
}