namespace Botticelli.Shared.API.Client.Responses;

public class PingResponse : BaseResponse<PingResponse>
{
    protected PingResponse(string uid) : base(uid, string.Empty)
    {
    }

    public static PingResponse GetInstance()
    {
        return new PingResponse(Utils.Uid.GenerateShortUid());
    }

    public static PingResponse GetInstance(string uid)
    {
        return new PingResponse(uid);
    }
}