using Botticelli.Scheduler.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Scheduler.Tests.Settings;

[TestFixture]
public class SchedulerSettingsTests
{
    [Test]
    public void SchedulerSettings_Should_ExposeConnectionString()
    {
        var settings = new SchedulerSettings
        {
            JobStorageConnectionString = "Host=localhost;Database=jobs"
        };

        settings.JobStorageConnectionString.Should().Be("Host=localhost;Database=jobs");
    }
}
