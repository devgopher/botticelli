using Botticelli.Analytics.Shared.Metrics;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Analytics.Shared.Tests.Metrics;

[TestFixture]
public class MetricNamesTests
{
    [Test]
    public void Constants_Should_HaveExpectedValues()
    {
        MetricNames.MessageReceived.Should().Be("MessageReceived");
        MetricNames.MessageSent.Should().Be("MessageSent");
        MetricNames.MessageRemoved.Should().Be("MessageRemoved");
        MetricNames.NewSubscription.Should().Be("NewSubscription");
        MetricNames.UnSubscription.Should().Be("UnSubscription");
        MetricNames.BotStarted.Should().Be("BotStarted");
        MetricNames.BotStopped.Should().Be("BotStopped");
        MetricNames.CommandReceived.Should().Be("CommandReceived");
        MetricNames.BotError.Should().Be("BotError");
        MetricNames.UserDefined.Should().Be("UserDefined");
    }

    [Test]
    public void Names_Should_ContainAllKnownMetrics()
    {
        MetricNames.Names.Should().BeEquivalentTo(
        [
            MetricNames.MessageReceived,
            MetricNames.MessageSent,
            MetricNames.MessageRemoved,
            MetricNames.BotStarted,
            MetricNames.BotStopped,
            MetricNames.NewSubscription,
            MetricNames.UnSubscription,
            MetricNames.CommandReceived,
            MetricNames.BotError,
            MetricNames.UserDefined
        ]);
    }
}
