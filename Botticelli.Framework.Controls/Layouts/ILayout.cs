namespace Botticelli.Framework.Controls.Layouts;

public interface ILayout
{
    public void AddRow(Row row);
    
    public IEnumerable<Row> Rows { get; }
}