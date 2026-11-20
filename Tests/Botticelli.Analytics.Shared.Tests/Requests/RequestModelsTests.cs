using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Analytics.Shared.Requests;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Analytics.Shared.Tests.Requests;

[TestFixture]
public class RequestModelsTests
{
    [Test]
    public void PushMetricRequest_Should_HoldMetricObject()
    {
        var metric = new MetricObject
        {
            BotId = "bot-42",
            Name = MetricNames.BotStarted,
            Timestamp = DateTime.UtcNow
        };

        var request = new PushMetricRequest<MetricObject> { Object = metric };

        request.Object.Should().BeSameAs(metric);
    }

    [Test]
    public void PushMetricRequest_Should_AllowNullObject()
    {
        var request = new PushMetricRequest<MetricObject> { Object = null };
        request.Object.Should().BeNull();
    }

    [Test]
    public void GetMetricsRequest_Should_HoldFilterFields()
    {
        var from = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc);

        var request = new GetMetricsRequest
        {
            BotId = "bot-7",
            Name = MetricNames.MessageReceived,
            From = from,
            To = to
        };

        request.BotId.Should().Be("bot-7");
        request.Name.Should().Be(MetricNames.MessageReceived);
        request.From.Should().Be(from);
        request.To.Should().Be(to);
    }

    [Test]
    public void GetMetricsForIntervalsRequest_Should_ExtendBaseRequest()
    {
        var request = new GetMetricsForIntervalsRequest
        {
            BotId = "bot-9",
            Name = MetricNames.CommandReceived,
            From = DateTime.UtcNow.AddHours(-1),
            To = DateTime.UtcNow,
            Interval = 15
        };

        request.Interval.Should().Be(15);
        request.BotId.Should().Be("bot-9");
        request.Name.Should().Be(MetricNames.CommandReceived);
    }
}
