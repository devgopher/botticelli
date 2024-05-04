namespace Botticelli.Framework.Controls.Layouts.Inlines;
using System.Globalization;

public class InlineCalendar : ILayout
{
    public InlineCalendar(DateTime dt)
    {
        var calendar = CultureInfo.InvariantCulture.Calendar;

        var days = calendar.GetDaysInMonth(dt.Year, dt.Month);
        
        Rows.Add();
    }
    
    public void AddRow(Row row)
    {
        throw new NotImplementedException();
    }

    public static InlineCalendar Get(DateTime dt)
    {
        
    }
    
    public IList<Row> Rows => new List<Row>(6);
}