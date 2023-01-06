namespace Botticelli.Shared.ValueObjects;

public class BinaryAttachment
{
    protected BinaryAttachment(string uid, string name, byte[] data)
    {
        Uid = uid;
        Name = name;
        Data = data;
    }

    public string Uid { get; }
    public string Name { get; }
    public byte[] Data { get; }

    public BinaryAttachment GetInstance(string uid, string name, byte[] data)
    {
        return new(uid, name, data);
    }
}