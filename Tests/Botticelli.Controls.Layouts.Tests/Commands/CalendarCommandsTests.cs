using Botticelli.Controls.Layouts.Commands.InlineCalendar;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Controls.Layouts.Tests.Commands;

[TestFixture]
public class CalendarCommandsTests
{
    [Test]
    public void BaseCalendarCommand_Implementations_Should_ExposeCurrentDtAndId()
    {
        BaseCalendarCommand[] commands =
        [
            new YearForwardCommand(),
            new YearBackwardCommand(),
            new MonthForwardCommand(),
            new MonthBackwardCommand(),
            new SetDateCommand()
        ];

        var dt = new DateTime(2026, 11, 20);
        foreach (var command in commands)
        {
            command.CurrentDt = dt;
            command.CurrentDt.Should().Be(dt);
            command.Id.Should().Be(Guid.Empty);
        }
    }

    [Test]
    public void DateChosenCommand_Should_ExposeCurrentDtAndId()
    {
        DateChosenCommand command = new TestDateChosenCommand
        {
            CurrentDt = new DateTime(2026, 1, 2)
        };

        command.CurrentDt.Should().Be(new DateTime(2026, 1, 2));
        command.Id.Should().Be(Guid.Empty);
    }

    private sealed class TestDateChosenCommand : DateChosenCommand;
}
