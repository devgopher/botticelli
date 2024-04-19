namespace Botticelli.Framework.Controls.Layouts;

public class Row
{
    private readonly List<Item> _cells = new(10);
    
    public void AddCell(Item item) => _cells.Add(item);
}