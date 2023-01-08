namespace Botticelli.Shared.API.Admin.Requests;

public class StopBotRequest : BaseRequest<StopBotRequest>
{
    protected StopBotRequest(string uid) : base(uid)
    {
    }

    public static StopBotRequest GetInstance()
    {
        return new(Utils.Uid.GenerateShortUid());
    }
}