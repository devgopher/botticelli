namespace Botticelli.Shared.API.Admin.Responses;

public class StopBotResponse : BaseResponse<StopBotResponse>
{
    protected StopBotResponse(string uid, string techMessage, AdminCommandStatus status)
            : base(uid, techMessage) =>
            Status = status;

    public AdminCommandStatus Status { get; }

    public static StopBotResponse GetInstance(AdminCommandStatus status, string techMessage) => new(Utils.Uid.GenerateShortUid(), techMessage, status);

    public static StopBotResponse GetInstance(string uid, string techMessage, AdminCommandStatus status) => new(uid, techMessage, status);
}