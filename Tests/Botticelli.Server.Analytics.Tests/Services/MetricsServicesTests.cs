using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Analytics.Shared.Requests;
using Botticelli.Server.Analytics.Cache;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Services;
using Botticelli.Server.Analytics.Tests.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Analytics.Tests.Services;

[TestFixture]
public class MetricsServicesTests
{
    [Test]
    public async Task MetricsInputService_Should_PersistMetric()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        var service = new MetricsInputService(new MetricsReaderWriter(context));
        var metric = new MetricObject
        {
            BotId = "bot-1",
            Name = MetricNames.MessageReceived,
            Timestamp = DateTime.UtcNow
        };

        await service.PushMetricAsync(new PushMetricRequest<IMetricObject> { Object = metric }, CancellationToken.None);

        context.MetricModels.Should().ContainSingle(m => m.BotId == "bot-1" && m.Name == MetricNames.MessageReceived);
    }

    [Test]
    public async Task MetricsOutputService_GetMetrics_Should_PreferCache()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        var cache = new Mock<ICacheAccessor>();
        cache.Setup(c => c.ReadCount(It.IsAny<Func<MetricModel, bool>>())).Returns(5);
        var service = new MetricsOutputService(new MetricsReaderWriter(context), cache.Object);

        var response = await service.GetMetricsAsync(new GetMetricsRequest
        {
            BotId = "bot",
            Name = MetricNames.MessageSent,
            From = DateTime.UtcNow.AddHours(-1),
            To = DateTime.UtcNow
        }, CancellationToken.None);

        response.Count.Should().Be(5);
    }

    [Test]
    public async Task MetricsOutputService_GetMetrics_Should_FallbackToDb()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        context.MetricModels.Add(new MetricModel
        { InternalValue = string.Empty,
            Id = Guid.NewGuid().ToString(),
            BotId = "bot",
            Name = MetricNames.MessageSent,
            Timestamp = DateTime.UtcNow
        });
        // Avoid ChangeTracker cache hook for seed: use another context without cache side effects
        await context.SaveChangesAsync();

        var cache = new Mock<ICacheAccessor>();
        cache.Setup(c => c.ReadCount(It.IsAny<Func<MetricModel, bool>>())).Returns(0);
        var service = new MetricsOutputService(new MetricsReaderWriter(context), cache.Object);

        var response = await service.GetMetricsAsync(new GetMetricsRequest
        {
            BotId = "bot",
            Name = MetricNames.MessageSent,
            From = DateTime.UtcNow.AddHours(-1),
            To = DateTime.UtcNow.AddHours(1)
        }, CancellationToken.None);

        response.Count.Should().Be(1);
    }

    [Test]
    public async Task MetricsOutputService_GetMetricNames_Should_MergeDefaultsAndDb()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        context.MetricModels.Add(new MetricModel
        { InternalValue = string.Empty,
            Id = Guid.NewGuid().ToString(),
            BotId = "bot",
            Name = "CustomMetric",
            Timestamp = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new MetricsOutputService(new MetricsReaderWriter(context), Mock.Of<ICacheAccessor>());
        var names = (await service.GetMetricNamesAsync(CancellationToken.None)).ToList();

        names.Should().Contain(MetricNames.MessageReceived);
        names.Should().Contain("CustomMetric");
    }

    [Test]
    public async Task MetricsOutputService_GetMetricsForInterval_Should_AggregateFrames()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        await using var _ = context;
        var from = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        context.MetricModels.AddRange(
            new MetricModel
            { InternalValue = string.Empty,
                Id = Guid.NewGuid().ToString(),
                BotId = "bot",
                Name = MetricNames.BotStarted,
                Timestamp = from.AddMinutes(1).ToUniversalTime()
            },
            new MetricModel
            { InternalValue = string.Empty,
                Id = Guid.NewGuid().ToString(),
                BotId = "bot",
                Name = MetricNames.BotStarted,
                Timestamp = from.AddMinutes(11).ToUniversalTime()
            });
        await context.SaveChangesAsync();

        var service = new MetricsOutputService(new MetricsReaderWriter(context), Mock.Of<ICacheAccessor>());
        var response = await service.GetMetricsForIntervalAsync(new GetMetricsForIntervalsRequest
        {
            BotId = "bot",
            Name = MetricNames.BotStarted,
            From = from,
            To = from.AddMinutes(20),
            Interval = 600
        }, CancellationToken.None);

        response.Count.Should().BeGreaterThan(0);
        response.MetricsForIntervals.Should().NotBeEmpty();
    }
}
