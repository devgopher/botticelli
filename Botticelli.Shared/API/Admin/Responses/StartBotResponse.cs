using Botticelli.Shared.Utils;

namespace Botticelli.Shared.API.Admin.Responses;

public class StartBotResponse : BaseResponse<StartBotResponse>
{
    public StartBotResponse(string? uid, string techMessage, AdminCommandStatus status)
            : base(uid, techMessage)
    {
        Status = status;
    }

    public AdminCommandStatus Status { get; }

    public static StartBotResponse GetInstance(AdminCommandStatus status, string techMessage)
    {
        return new StartBotResponse(BotIdUtils.GenerateShortBotId(), techMessage, status);
    }

    public static StartBotResponse GetInstance(string? uid, string techMessage, AdminCommandStatus status)
    {
        return new StartBotResponse(uid, techMessage, status);
    }
}