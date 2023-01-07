namespace Botticelli.Shared.ValueObjects;

/// <summary>
///     Received/sent message
/// </summary>
public class Message
{
    protected Message(string uid)
    {
        Uid = uid;
    }

    /// <summary>
    ///     Message uid
    /// </summary>
    public string Uid { get; }

    /// <summary>
    /// Chat id
    /// </summary>
    public string ChatId { get; }

    /// <summary>
    ///     Message subj
    /// </summary>
    public string? Subject { get; }

    /// <summary>
    ///     Message body
    /// </summary>
    public string? Body { get; }

    /// <summary>
    ///     Message attachments
    /// </summary>
    public ICollection<BinaryAttachment>? Attachments { get; set; }
}