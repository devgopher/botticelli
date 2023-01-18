namespace Botticelli.Shared.API.Client.Requests;

public class PingRequest : BaseRequest<PingRequest>
{
    protected PingRequest(string uid) : base(uid)
    {
    }

    public static PingRequest GetInstance()
    {
        return new(Utils.Uid.GenerateShortUid());
    }
}