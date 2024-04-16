using Botticelli.Shared.Constants;

namespace Botticelli.Shared.ValueObjects;

public class BinaryBaseAttachment : BaseAttachment
{
    public BinaryBaseAttachment(string uid,
        string name,
        MediaType mediaType,
        string url,
        byte[] data)
    {
        Uid = uid;
        Name = name;
        Data = data;
        Url = url;
        MediaType = mediaType;
    }

    public string Url { get; }
    public MediaType MediaType { get; }
    public byte[] Data { get; }
    public override string Uid { get; }
    public override string Name { get; }
    public override string OwnerId { get; }
}