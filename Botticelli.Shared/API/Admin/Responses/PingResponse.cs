namespace Botticelli.Shared.API.Admin.Responses;

public class PingResponse : BaseResponse<PingResponse>
{
    protected PingResponse(string uid) : base(uid)
    {
    }

    public static PingResponse GetInstance()
    {
        return new(Utils.Uid.GenerateShortUid());
    }

    public static PingResponse GetInstance(string uid)
    {
        return new(uid);
    }
}