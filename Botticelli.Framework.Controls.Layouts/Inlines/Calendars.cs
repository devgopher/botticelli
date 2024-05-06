using System.Collections.Concurrent;
using System.Globalization;

namespace Botticelli.Framework.Controls.Layouts.Inlines;

public static class Calendars 
{
    public static InlineCalendar Get(DateTime dt, string culture)
    {
        var key = (dt.Month, dt.Year, culture);
        
        if (Cache.TryGetValue(key, out var calendar))
            return calendar;

        var cultureInfo = new CultureInfo(culture);
        
        calendar = new InlineCalendar(dt, cultureInfo);

        Cache[key] = calendar;
        
        return calendar;
    }

    private static ConcurrentDictionary<(int month, int year, string culture), InlineCalendar> Cache => new();
}