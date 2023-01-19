using Botticelli.Shared.Constants;

namespace Botticelli.Shared.ValueObjects;

public class BinaryAttachment
{
    public BinaryAttachment(string uid,
                            string name,
                            MediaType mediaType,
                            byte[] data)
    {
        Uid = uid;
        Name = name;
        Data = data;
        MediaType = mediaType;
    }

    public string Uid { get; }
    public string Name { get; }
    public MediaType MediaType { get; }
    public byte[] Data { get; }

    public BinaryAttachment GetInstance(string uid,
                                        string name,
                                        MediaType mediaType,
                                        byte[] data)
    {
        return new BinaryAttachment(uid,
                                    name,
                                    mediaType,
                                    data);
    }
}