using Botticelli.Scheduler;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Scheduler.Tests;

[TestFixture]
public class ReliabilityTests
{
    [Test]
    public void Reliability_Should_ExposeAssignableProperties()
    {
        var delay = TimeSpan.FromSeconds(3);
        var reliability = new Reliability
        {
            IsEnabled = true,
            MaxTries = 5,
            IsExponential = true,
            Delay = delay
        };

        reliability.IsEnabled.Should().BeTrue();
        reliability.MaxTries.Should().Be(5);
        reliability.IsExponential.Should().BeTrue();
        reliability.Delay.Should().Be(delay);
    }
}
