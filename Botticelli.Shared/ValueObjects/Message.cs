namespace Botticelli.Shared.ValueObjects;

/// <summary>
///     Received/sent message
/// </summary>
public class Message
{
    public Message(string uid) => Uid = uid;

    /// <summary>
    ///     Message uid
    /// </summary>
    public string Uid { get; set; }

    /// <summary>
    ///     Chat id
    /// </summary>
    public string ChatId { get; set; }

    /// <summary>
    ///     Message subj
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    ///     Message body
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    ///     Message attachments
    /// </summary>
    public ICollection<BinaryAttachment>? Attachments { get; set; }
}