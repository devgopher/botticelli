namespace Botticelli.Framework.Controls.BasicControls;

public class Text : IControl
{
    public string? Content { get; set; }
    public Dictionary<string, Dictionary<string, object>> Specials { get; set; } = new();
}