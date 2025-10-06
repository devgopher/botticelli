namespace Botticelli.Controls.Layouts.Inlines;

public class Table(int cols) : ILayout
{
    public virtual void AddRow(Row row)
    {
        Rows.Add(row);
    }

    public IList<Row>? Rows { get; } = new List<Row>();

    public void AddItem(Item item)
    {
        var lastRow = Rows.SkipWhile(row => row.Items.Count == cols).FirstOrDefault();

        if (lastRow == null)
        {
            lastRow = new Row();
            AddRow(lastRow);
        }

        lastRow.Items.Add(item);
    }
}