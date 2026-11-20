using System.Globalization;
using Botticelli.Controls.Layouts.Inlines;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Controls.Layouts.Tests.Inlines;

[TestFixture]
public class CalendarFactoryTests
{
    [Test]
    public void Get_Should_ReturnCalendarForCulture()
    {
        var dt = new DateTime(2026, 3, 10);
        var calendar = CalendarFactory.Get(dt, CultureInfo.InvariantCulture.Name);

        calendar.Should().NotBeNull();
        calendar.Rows.Should().NotBeEmpty();
    }

    [Test]
    public void GetMonthsForward_Overloads_Should_ShiftMonth()
    {
        var baseDt = new DateTime(2026, 1, 15);
        var forward = CalendarFactory.GetMonthsForward(baseDt, "en-US", 2);
        var backward = CalendarFactory.GetMonthsBackward(baseDt, "en-US", 1);
        var todayForward = CalendarFactory.GetMonthsForward("en-US");
        var todayBackward = CalendarFactory.GetMonthsBackward("en-US");

        forward.Should().NotBeNull();
        backward.Should().NotBeNull();
        todayForward.Should().NotBeNull();
        todayBackward.Should().NotBeNull();
    }
}
