namespace Botticelli.Shared.API.Client.Responses;

public class SendMessageResponse : BaseResponse<SendMessageResponse>
{
    public SendMessageResponse(string uid, string? techMessage) : base(uid, techMessage)
    {
    }

    public string? MessageUid { get; set; }

    public MessageSentStatus MessageSentStatus { get; set; }

    public static SendMessageResponse GetInstance(string? techMessage)
    {
        return new SendMessageResponse(Utils.Uid.GenerateShortUid(), techMessage);
    }

    public static SendMessageResponse GetInstance(string uid, string? techMessage)
    {
        return new SendMessageResponse(uid, techMessage);
    }
}