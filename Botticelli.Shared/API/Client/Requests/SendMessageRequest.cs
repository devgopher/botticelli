using Botticelli.Shared.ValueObjects;

namespace Botticelli.Shared.API.Client.Requests;

public class SendMessageRequest : BaseRequest<SendMessageRequest>
{
    public SendMessageRequest(string uid) : base(uid)
    {
    }

    public Message Message { get; set; }

    public static SendMessageRequest GetInstance() => new(Utils.Uid.GenerateShortUid());
}