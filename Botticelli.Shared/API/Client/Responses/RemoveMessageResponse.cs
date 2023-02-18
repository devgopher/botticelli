namespace Botticelli.Shared.API.Client.Responses;

public class RemoveMessageResponse : BaseResponse<RemoveMessageResponse>
{
    public RemoveMessageResponse(string uid, string? techMessage) : base(uid, techMessage)
    {
    }

    public string? MessageUid { get; set; }

    public MessageRemovedStatus MessageRemovedStatus { get; set; }

    public static SendMessageResponse GetInstance(string? techMessage) => new(Utils.Uid.GenerateShortUid(), techMessage);

    public static SendMessageResponse GetInstance(string uid, string? techMessage) => new(uid, techMessage);
}