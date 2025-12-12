namespace Botticelli.Shared.API;

public abstract class BaseResponse(string? uid, string? techMessage)
{
    public string? Uid { get; } = uid;

    public string? TechMessage { get; } = techMessage;
}

public abstract class BaseResponse<T>(string? uid, string? techMessage) : BaseResponse(uid, techMessage)
    where T : BaseResponse;