using Botticelli.Shared.Utils;

namespace Botticelli.Shared.API.Admin.Responses;

public class StartBotResponse(string? uid, string techMessage, AdminCommandStatus status)
    : BaseResponse<StartBotResponse>(uid, techMessage)
{
    public AdminCommandStatus Status { get; } = status;

    public static StartBotResponse GetInstance(AdminCommandStatus status, string techMessage)
    {
        return new StartBotResponse(BotIdUtils.GenerateShortBotId(), techMessage, status);
    }

    public static StartBotResponse GetInstance(string? uid, string techMessage, AdminCommandStatus status)
    {
        return new StartBotResponse(uid, techMessage, status);
    }
}