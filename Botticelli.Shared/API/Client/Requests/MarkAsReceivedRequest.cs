namespace Botticelli.Shared.API.Client.Requests;

public class MarkAsReceivedRequest : IBotRequest
{
    public string? MessageId { get; set; }
    public string? BotId { get; set; }
}