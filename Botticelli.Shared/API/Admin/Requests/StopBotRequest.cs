namespace Botticelli.Shared.API.Admin.Requests;

public class StopBotRequest : BaseRequest<StopBotRequest>
{
    protected StopBotRequest(string uid) : base(uid)
    {
    }

    public override StopBotRequest GetInstance()
    {
        return new(Utils.Uid.GenerateShortUid());
    }
}