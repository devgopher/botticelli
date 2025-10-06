namespace Botticelli.Broadcasting.Dal.Models;

/// <summary>
///     Represents the status of a message within a chat.
///     This class stores information about whether a message has been sent,
///     along with relevant timestamps and identifiers.
/// </summary>
public class MessageStatus
{
    /// <summary>
    ///     Gets or sets the unique identifier for the message.
    ///     This property is used to associate the status with a specific message.
    /// </summary>
    public required string MessageId { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier for the chat.
    ///     This property links the message status to a specific chat session.
    /// </summary>
    public required string ChatId { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the message has been sent.
    ///     This property indicates the delivery status of the message (true if sent, false otherwise).
    /// </summary>
    public required bool IsSent { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the message status was created.
    ///     This property records when the status entry was created in the system.
    /// </summary>
    public required DateTime CreatedDate { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the message was sent.
    ///     This property is nullable, as it may not be set if the message has not been sent yet.
    /// </summary>
    public DateTime? SentDate { get; set; }
}