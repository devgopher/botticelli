namespace Botticelli.Shared.ValueObjects;

/// <summary>
///     Class for chat entity
/// </summary>
public class Chat
{
    /// <summary>
    ///     Chat ID
    /// </summary>
    public string? ChatId { get; set; }

    /// <summary>
    ///     Chat name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Chat description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Is chat active or not
    /// </summary>
    public bool? IsActive { get; set; }
}