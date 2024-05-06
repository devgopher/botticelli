using System.Globalization;
using Botticelli.Framework.Controls.BasicControls;

namespace Botticelli.Framework.Controls.Layouts.Inlines;

public class InlineCalendar : ILayout
{
    private static readonly DayOfWeek[] Days =
    [
        DayOfWeek.Sunday,
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday
    ];

    public InlineCalendar(DateTime dt, CultureInfo cultureInfo)
    {
        Init(dt, cultureInfo);
    }

    public void AddRow(Row row) => Rows.Add(row);

    public IList<Row> Rows => new List<Row>(6);

    private void Init(DateTime dt, CultureInfo cultureInfo)
    {
        // Displays a month name in a header
        var monthName = cultureInfo.DateTimeFormat.MonthNames[dt.Month];
        var monthNameRow = new Row();
        monthNameRow.Items.Add(new Item { Control = new Text() { Content = monthName}});
        Rows.Add(monthNameRow);
        
        // Displays a weekday names in a header
        var sortedDays = new DayOfWeek[Days.Length];
        var fdw = (int)cultureInfo.DateTimeFormat.FirstDayOfWeek;
        for (var i = 0; i < Days.Length; ++i) sortedDays[i] = Days[(fdw + i) % Days.Length];

        var weekDaysRow = new Row();
        weekDaysRow.Items.AddRange(sortedDays.Select(sd => new Item { Control = new Text { Content = sd.ToString("G") } }));
        Rows.Add(weekDaysRow);
        
        // Displays dates
        var calendar = CultureInfo.InvariantCulture.Calendar;
        var days = calendar.GetDaysInMonth(dt.Year, dt.Month);
        var rows = new Row?[5];

        for (var day = 0; day < days; ++day)
        {
            var cdt = new DateTime(day, dt.Month, dt.Year);
            var offset = (int)(day + cdt.DayOfWeek);
            
            rows[offset % rows.Length] ??= new Row();
            rows[offset % rows.Length]!.Items[offset] = new Item()
            {
                Control = new Button()
                {
                    Content = (day + 1).ToString()
                }
            };

        }
        
        foreach (var row in rows) 
            Rows.Add(row!);
    }
}