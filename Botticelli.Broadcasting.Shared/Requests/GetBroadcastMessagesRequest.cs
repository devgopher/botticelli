namespace Botticelli.Broadcasting.Shared.Requests;

public class GetBroadcastMessagesRequest
{
    public required string BotId { get; set; }
    public required TimeSpan HowOld { get; set; }
}