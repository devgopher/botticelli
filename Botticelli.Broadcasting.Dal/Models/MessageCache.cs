using System.ComponentModel.DataAnnotations;

namespace Botticelli.Broadcasting.Dal.Models;

/// <summary>
///     Represents a cache for serialized messages.
///     This class is used to store messages in a serialized format for efficient retrieval.
/// </summary>
public class MessageCache
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageCache" /> class.
    /// </summary>
    /// <param name="id">The unique identifier for the message cache entry.</param>
    /// <param name="serializedMessageObject">The serialized message object.</param>
    public MessageCache(string id, string serializedMessageObject)
    {
        Id = id;
        SerializedMessageObject = serializedMessageObject;
    }

    /// <summary>
    ///     Gets or sets the unique identifier for the message cache entry.
    ///     This property serves as the primary key in the database.
    /// </summary>
    [Key]
    public required string Id { get; set; }

    /// <summary>
    ///     Gets or sets the serialized representation of the message object.
    ///     This property stores the message in a format such as JSON or XML.
    /// </summary>
    [MaxLength(100000)]
    public required string SerializedMessageObject { get; set; }
}