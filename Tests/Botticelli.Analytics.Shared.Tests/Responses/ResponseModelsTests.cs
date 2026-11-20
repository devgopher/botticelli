using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Analytics.Shared.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Analytics.Shared.Tests.Responses;

[TestFixture]
public class ResponseModelsTests
{
    [Test]
    public void GetMetricsResponse_Should_HoldAggregateFields()
    {
        var from = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = new DateTime(2026, 2, 2, 0, 0, 0, DateTimeKind.Utc);
        var objects = new List<IMetricObject>
        {
            new MetricObject
            {
                BotId = "bot-1",
                Name = MetricNames.MessageSent,
                Timestamp = from
            }
        };

        var response = new GetMetricsResponse
        {
            From = from,
            To = to,
            Count = 3,
            Objects = objects
        };

        response.From.Should().Be(from);
        response.To.Should().Be(to);
        response.Count.Should().Be(3);
        response.Objects.Should().BeEquivalentTo(objects);
    }

    [Test]
    public void GetMetricsResponse_Should_AllowNullObjects()
    {
        var response = new GetMetricsResponse
        {
            From = DateTime.UtcNow.AddDays(-1),
            To = DateTime.UtcNow,
            Count = 0,
            Objects = null
        };

        response.Objects.Should().BeNull();
        response.Count.Should().Be(0);
    }

    [Test]
    public void GetMetricsIntervalsResponse_Should_HoldIntervalBuckets()
    {
        var bucket = new GetMetricsResponse
        {
            From = DateTime.UtcNow.AddMinutes(-15),
            To = DateTime.UtcNow,
            Count = 2,
            Objects = []
        };

        var response = new GetMetricsIntervalsResponse
        {
            From = bucket.From,
            To = bucket.To,
            Count = 2,
            Objects = [],
            MetricsForIntervals = [bucket]
        };

        response.MetricsForIntervals.Should().ContainSingle().Which.Count.Should().Be(2);
    }
}
