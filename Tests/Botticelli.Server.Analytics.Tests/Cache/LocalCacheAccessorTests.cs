using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Analytics.Shared.Requests;
using Botticelli.Server.Analytics.Cache;
using Botticelli.Server.Analytics.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Analytics.Tests.Cache;

[TestFixture]
public class LocalCacheAccessorTests
{
    private LocalCacheAccessor _cache = null!;

    [SetUp]
    public void SetUp()
    {
        _cache = new LocalCacheAccessor(1000);
        _cache.Clear(DateTime.UtcNow.AddYears(100));
    }

    [Test]
    public void Set_ReadCount_Remove_Should_Work()
    {
        var model = new MetricModel
        { InternalValue = string.Empty,
            Id = Guid.NewGuid().ToString(),
            BotId = "bot-1",
            Name = MetricNames.MessageSent,
            Timestamp = DateTime.UtcNow
        };

        _cache.Set(model);
        _cache.ReadCount(x => x.BotId == "bot-1").Should().Be(1);

        _cache.Remove(model);
        _cache.ReadCount(x => x.Id == model.Id).Should().Be(0);
    }

    [Test]
    public void Clear_Should_RemoveOlderEntries()
    {
        var oldId = Guid.NewGuid().ToString();
        var newId = Guid.NewGuid().ToString();
        _cache.Set(new MetricModel
        { InternalValue = string.Empty,
            Id = oldId,
            BotId = "bot",
            Name = MetricNames.BotStarted,
            Timestamp = DateTime.UtcNow.AddDays(-2)
        });
        _cache.Set(new MetricModel
        { InternalValue = string.Empty,
            Id = newId,
            BotId = "bot",
            Name = MetricNames.BotStarted,
            Timestamp = DateTime.UtcNow
        });

        _cache.Clear(DateTime.UtcNow.AddDays(-1));

        _cache.ReadCount(x => x.Id == oldId).Should().Be(0);
        _cache.ReadCount(x => x.Id == newId).Should().Be(1);
    }

    [Test]
    public void GetForIntervals_Should_ReturnNonEmptyBuckets()
    {
        var from = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc);
        var stamp = from.AddSeconds(5);
        _cache.Set(new MetricModel
        { InternalValue = string.Empty,
            Id = Guid.NewGuid().ToString(),
            BotId = "bot-x",
            Name = MetricNames.CommandReceived,
            Timestamp = stamp
        });

        var response = _cache.GetForIntervals(new GetMetricsForIntervalsRequest
        {
            BotId = "bot-x",
            Name = MetricNames.CommandReceived,
            From = from,
            To = from.AddSeconds(30),
            Interval = 10
        });

        response.MetricsForIntervals.Should().NotBeEmpty();
        response.MetricsForIntervals.Sum(x => x.Count).Should().BeGreaterThan(0);
    }
}
