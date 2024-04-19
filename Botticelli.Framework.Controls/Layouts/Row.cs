namespace Botticelli.Framework.Controls.Layouts;

public class Row
{
    public List<Item> Items { get; init; } = new(10);

    public void AddItem(Item item) => Items.Add(item);
}