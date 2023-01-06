namespace Botticelli.Shared.API.Admin.Requests;

public class StartBotRequest : BaseRequest<StartBotRequest>
{
    protected StartBotRequest(string uid) : base(uid)
    {
    }

    public override StartBotRequest GetInstance()
    {
        return new(Utils.Uid.GenerateShortUid());
    }
}