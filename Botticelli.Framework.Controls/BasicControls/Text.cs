namespace Botticelli.Framework.Controls.BasicControls;

public class Text : IControl
{
    public string? Content { get; set; }

    public string? CallbackData
    {
        get => Params?["CallbackData"];
        set
        {
            Params ??= new Dictionary<string, string>();

            if (value != null) Params["CallbackData"] = value;
        }
    }

    public Dictionary<string, string>? Params { get; set; }
    public Dictionary<string, Dictionary<string, object>>? MessengerSpecificParams { get; set; } = new();
}