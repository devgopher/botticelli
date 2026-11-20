using Botticelli.Scheduler;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Scheduler.Tests;

[TestFixture]
public class ScheduleTests
{
    [Test]
    public void Schedule_Should_ExposeAssignableCron()
    {
        var schedule = new Schedule { Cron = "0 * * * *" };

        schedule.Cron.Should().Be("0 * * * *");
    }
}
