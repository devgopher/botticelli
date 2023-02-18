using Botticelli.Shared.Constants;

namespace Botticelli.Shared.ValueObjects;

public class BinaryAttachment : IAttachment
{
    public BinaryAttachment(string uid,
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

    public string Uid { get; }
    public string Name { get; }
}