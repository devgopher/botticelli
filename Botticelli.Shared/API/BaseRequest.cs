namespace Botticelli.Shared.API;

public abstract class BaseRequest<T>(string? uid) : BaseRequest(uid)
    where T : BaseRequest;

public abstract class BaseRequest(string? uid)
{
    public string? Uid { get; } = uid;
}