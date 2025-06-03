using Botticelli.Shared.ValueObjects;

namespace Botticelli.Broadcasting.Shared.Responses;

public class GetBroadcastMessagesResponse
{
    public required string Id { get; set; }
    public required List<Message> Messages { get; set; }
}