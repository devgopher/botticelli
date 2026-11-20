using System.Globalization;
using Botticelli.Controls.BasicControls;
using Botticelli.Controls.Layouts.Inlines;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Controls.Layouts.Tests.Inlines;

[TestFixture]
public class InlineCalendarTests
{
    [Test]
    public void Constructor_Should_BuildYearMonthAndWeekdayHeaders()
    {
        var dt = new DateTime(2026, 11, 15);
        var calendar = new InlineCalendar(dt, CultureInfo.InvariantCulture)
        {
            TodayMark = "*"
        };

        calendar.Rows.Should().NotBeNull();
        calendar.Rows!.Count.Should().BeGreaterThan(3);
        calendar.TodayMark.Should().Be("*");

        var yearRow = calendar.Rows[0];
        yearRow.Items.Should().HaveCount(3);
        ((Button)yearRow.Items[0].Control!).Content.Should().Be("<<");
        ((Button)yearRow.Items[1].Control!).Content.Should().Be("2026");
        ((Button)yearRow.Items[0].Control!).Params.Should().ContainKey("CallbackData")
            .WhoseValue.Should().Contain("/YearBackward");

        var monthRow = calendar.Rows[1];
        ((Button)monthRow.Items[0].Control!).Params.Should().ContainKey("CallbackData")
            .WhoseValue.Should().Contain("/MonthBackward");
        ((Button)monthRow.Items[2].Control!).Params.Should().ContainKey("CallbackData")
            .WhoseValue.Should().Contain("/MonthForward");
    }

    [Test]
    public void AddRow_Should_AppendRow()
    {
        var calendar = new InlineCalendar(DateTime.Today, CultureInfo.InvariantCulture);
        var before = calendar.Rows!.Count;
        var row = new Layouts.Row();

        calendar.AddRow(row);

        calendar.Rows.Should().HaveCount(before + 1);
    }

    [Test]
    public void TodayMark_Default_Should_UseCheckmarkForToday()
    {
        var today = DateTime.Today;
        var calendar = new InlineCalendar(today, CultureInfo.InvariantCulture);

        var dayButtons = calendar.Rows!
            .Skip(3)
            .Where(r => r is not null)
            .SelectMany(r => r!.Items)
            .Select(i => i.Control)
            .OfType<Button>()
            .Where(b => b.Content is not null && b.Content != "-")
            .ToList();

        dayButtons.Should().Contain(b => b.Content!.Contains(today.Day.ToString()) &&
                                         (b.Content.Contains('✓') || !string.IsNullOrEmpty(b.Image)));
    }
}
