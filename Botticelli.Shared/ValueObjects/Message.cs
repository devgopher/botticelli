namespace Botticelli.Shared.ValueObjects;

/// <summary>
///     Received/sent message
/// </summary>
[Serializable]
public class Message
{
    public Message()
    {
        Uid = Guid.NewGuid().ToString();
    }

    public Message(string uid)
    {
        Uid = uid;
    }

    /// <summary>
    ///     Message uid
    /// </summary>
    public string? Uid { get; set; }

    /// <summary>
    ///     Chat ids
    /// </summary>
    public List<string> ChatIds { get; set; }

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
    public ICollection<IAttachment>? Attachments { get; set; }

    /// <summary>
    ///     From user
    /// </summary>
    public User? From { get; set; }

    /// <summary>
    ///     Forwarded from user
    /// </summary>
    public User? ForwardedFrom { get; set; }

    /// <summary>
    ///     Contacts
    /// </summary>
    public Contact? Contact { get; set; }

    /// <summary>
    ///     Poll
    /// </summary>
    public Poll? Poll { get; set; }

    /// <summary>
    ///     Reply to a message with uid
    /// </summary>
    public string? ReplyToMessageUid { get; set; }

    /// <summary>
    ///     GeoLocation
    /// </summary>
    public GeoLocation Location { get; set; }
}