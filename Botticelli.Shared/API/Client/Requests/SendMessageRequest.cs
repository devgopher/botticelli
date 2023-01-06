using Botticelli.Shared.ValueObjects;

namespace Botticelli.Shared.API.Client.Requests;

public class SendMessageRequest : BaseRequest<SendMessageRequest>
{
    public SendMessageRequest(string uid) : base(uid)
    {
    }

    public Message Message { get; set; }

    public override SendMessageRequest GetInstance()
    {
        return new(Utils.Uid.GenerateShortUid());
    }
}