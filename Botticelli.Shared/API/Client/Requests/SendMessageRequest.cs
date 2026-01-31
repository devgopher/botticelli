using Botticelli.Shared.ValueObjects;

namespace Botticelli.Shared.API.Client.Requests;

public class SendMessageRequest(string uid) : BaseRequest<SendMessageRequest>(uid)
{
    public SendMessageRequest() : this(Guid.NewGuid().ToString())
    {
    }

    public bool? ExpectPartialResponse { get; set; }
    public int? SequenceNumber { get; set; }
    public bool? IsFinal { get; set; }
    public Message Message { get; set; } = new(uid);
}