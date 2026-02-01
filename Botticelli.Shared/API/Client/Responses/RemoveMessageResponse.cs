using Botticelli.Shared.Utils;

namespace Botticelli.Shared.API.Client.Responses;

public class RemoveMessageResponse(string? uid, string? techMessage)
    : BaseResponse<RemoveMessageResponse>(uid, techMessage)
{
    public string? MessageUid { get; set; }

    public MessageRemovedStatus MessageRemovedStatus { get; set; }

    public static RemoveMessageResponse GetInstance(string? techMessage)
    {
        return new RemoveMessageResponse(BotIdUtils.GenerateShortBotId(), techMessage);
    }

    public static RemoveMessageResponse GetInstance(string? uid, string? techMessage)
    {
        return new RemoveMessageResponse(uid, techMessage);
    }
}