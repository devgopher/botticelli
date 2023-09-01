namespace Botticelli.Shared.ValueObjects;

public interface IAttachment
{
    public string Uid { get; }
    public string Name { get; }
    public string OwnerId { get; }
}