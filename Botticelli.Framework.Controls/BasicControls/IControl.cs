namespace Botticelli.Framework.Controls.BasicControls;

/// <summary>
/// A basic control interface
/// </summary>
public interface IControl
{
    /// <summary>
    /// Content for a control
    /// </summary>
    public string? Content { get; set; }
    
    /// <summary>
    /// Messenger-specific parameters
    /// <MessengerName, <Key, Value>>
    /// </summary>
    public Dictionary<string, Dictionary<string, object>> Specials { get; set; }
}