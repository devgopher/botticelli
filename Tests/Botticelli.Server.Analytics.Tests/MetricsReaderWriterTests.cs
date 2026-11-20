using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Tests.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Analytics.Tests;

[TestFixture]
public class MetricsReaderWriterTests
{
    [Test]
    public async Task WriteAsync_Should_AddMetricAndNotifyCache()
    {
        var (context, cache) = TestMetricsContextFactory.Create();
        await using var _ = context;
        var rw = new MetricsReaderWriter(context);

        await rw.WriteAsync(new MetricObject
        {
            BotId = "bot-2",
            Name = MetricNames.BotError,
            Timestamp = DateTime.UtcNow
        }, CancellationToken.None);

        context.MetricModels.Should().HaveCount(1);
        cache.Verify(c => c.Set(It.IsAny<MetricModel>()), Times.AtLeastOnce);
    }

    [Test]
    public async Task ReadCountAsync_Should_FilterMetrics()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        context.MetricModels.Add(new MetricModel
        { InternalValue = string.Empty,
            Id = "1",
            BotId = "bot",
            Name = MetricNames.MessageSent,
            Timestamp = DateTime.UtcNow
        });
        context.MetricModels.Add(new MetricModel
        { InternalValue = string.Empty,
            Id = "2",
            BotId = "other",
            Name = MetricNames.MessageSent,
            Timestamp = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var rw = new MetricsReaderWriter(context);
        var count = await rw.ReadCountAsync(m => m.BotId == "bot", CancellationToken.None);

        count.Should().Be(1);
    }

    [Test]
    public async Task GetMetricNamesAsync_Should_ReturnDistinctNames()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        context.MetricModels.AddRange(
            new MetricModel { InternalValue = string.Empty, Id = "1", BotId = "b", Name = "A", Timestamp = DateTime.UtcNow },
            new MetricModel { InternalValue = string.Empty, Id = "2", BotId = "b", Name = "A", Timestamp = DateTime.UtcNow },
            new MetricModel { InternalValue = string.Empty, Id = "3", BotId = "b", Name = "B", Timestamp = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var names = (await new MetricsReaderWriter(context).GetMetricNamesAsync(CancellationToken.None)).ToList();
        names.Should().BeEquivalentTo(["A", "B"]);
    }

    [Test]
    public async Task ReadCountForFramesAsync_Should_ReturnNonEmptyFrames()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        var from = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);
        context.MetricModels.Add(new MetricModel
        { InternalValue = string.Empty,
            Id = "1",
            BotId = "bot",
            Name = MetricNames.CommandReceived,
            Timestamp = from.AddSeconds(30).ToUniversalTime()
        });
        await context.SaveChangesAsync();

        var frames = await new MetricsReaderWriter(context).ReadCountForFramesAsync(
            MetricNames.CommandReceived,
            "bot",
            from,
            from.AddMinutes(2),
            TimeSpan.FromMinutes(1),
            CancellationToken.None);

        var list = new List<(DateTime, DateTime, int)>();
        await foreach (var frame in frames)
            list.Add(frame);

        list.Should().NotBeEmpty();
        list.Sum(f => f.Item3).Should().BeGreaterThan(0);
    }
}
