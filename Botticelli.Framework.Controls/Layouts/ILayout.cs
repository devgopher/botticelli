namespace Botticelli.Framework.Controls.Layouts;

public interface ILayout
{
    public void AddRow(Row row);
    
    public IList<Row> Rows { get; }
}