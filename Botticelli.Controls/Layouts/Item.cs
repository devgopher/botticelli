using Botticelli.Controls.BasicControls;

namespace Botticelli.Controls.Layouts;

public class Item
{
    public Item()
    {
    }

    public Item(IControl? control)
    {
        Control = control;
    }

    public IControl? Control { get; set; }

    public ItemParams? Params { get; set; }
}