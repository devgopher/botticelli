namespace Botticelli.Shared.API.Client.Responses;

public class PingResponse : BaseResponse<PingResponse>
{
    protected PingResponse(string uid) : base(uid, string.Empty)
    {
    }

    public static PingResponse GetInstance() => new PingResponse(Utils.Uid.GenerateShortUid());

    public static PingResponse GetInstance(string uid) => new PingResponse(uid);
}