using Botticelli.Analytics.Shared.Metrics;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Analytics.Shared.Tests.Metrics;

[TestFixture]
public class MetricObjectTests
{
    [Test]
    public void MetricObject_Should_ExposeAssignableProperties()
    {
        var timestamp = new DateTime(2026, 11, 20, 12, 0, 0, DateTimeKind.Utc);
        var additional = new Dictionary<string, string> { ["key"] = "value" };

        var metric = new MetricObject
        {
            BotId = "bot-1",
            Name = MetricNames.MessageSent,
            Timestamp = timestamp,
            AdditionalParameters = additional
        };

        metric.BotId.Should().Be("bot-1");
        metric.Name.Should().Be(MetricNames.MessageSent);
        metric.Timestamp.Should().Be(timestamp);
        metric.AdditionalParameters.Should().ContainKey("key").WhoseValue.Should().Be("value");
        metric.Id.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void MetricObject_Should_AllowNullOptionalFields()
    {
        var metric = new MetricObject
        {
            BotId = null,
            Name = MetricNames.UserDefined,
            Timestamp = DateTime.UtcNow,
            AdditionalParameters = null
        };

        metric.BotId.Should().BeNull();
        metric.AdditionalParameters.Should().BeNull();
        ((IMetricObject)metric).Name.Should().Be(MetricNames.UserDefined);
    }
}
