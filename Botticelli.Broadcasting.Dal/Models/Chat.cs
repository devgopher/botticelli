using System.ComponentModel.DataAnnotations;

namespace Botticelli.Broadcasting.Dal.Models;

/// <summary>
///     Represents a chat record in the system.
///     This record stores information about a specific chat session, including its status.
/// </summary>
public record Chat
{
    /// <summary>
    ///     Gets or sets the unique identifier for the chat.
    ///     This property serves as the primary key in the database.
    /// </summary>
    [Key]
    public required string ChatId { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the chat is currently active.
    /// </summary>
    public bool IsActive { get; set; }
}