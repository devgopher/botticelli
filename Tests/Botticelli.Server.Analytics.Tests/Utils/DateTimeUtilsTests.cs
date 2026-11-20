using Botticelli.Server.Analytics.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Analytics.Tests.Utils;

[TestFixture]
public class DateTimeUtilsTests
{
    [Test]
    public void GetRange_Sync_Should_SplitByPeriod()
    {
        var from = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = from.AddMinutes(45);

        var ranges = DateTimeUtils.GetRange(from, to, TimeSpan.FromMinutes(15)).ToList();

        ranges.Should().HaveCount(3);
        ranges[0].Should().Be((from, from.AddMinutes(15)));
        ranges[1].Should().Be((from.AddMinutes(15), from.AddMinutes(30)));
        ranges[2].Should().Be((from.AddMinutes(30), from.AddMinutes(45)));
    }

    [Test]
    public void GetRange_Sync_Should_Throw_WhenToBeforeFrom()
    {
        var from = DateTime.UtcNow;
        var act = () => DateTimeUtils.GetRange(from, from.AddMinutes(-1), TimeSpan.FromMinutes(1)).ToList();

        act.Should().Throw<InvalidDataException>();
    }

    [Test]
    public async Task GetRange_Async_Should_SplitByPeriod()
    {
        var from = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = from.AddHours(1);

        var ranges = new List<(DateTime, DateTime)>();
        await foreach (var frame in DateTimeUtils.GetRange(from, to, TimeSpan.FromMinutes(20), CancellationToken.None))
            ranges.Add(frame);

        ranges.Should().HaveCount(3);
    }

    [Test]
    public async Task GetRange_Async_Should_Throw_WhenToBeforeFrom()
    {
        var from = DateTime.UtcNow;
        var act = async () =>
        {
            await foreach (var _ in DateTimeUtils.GetRange(from, from.AddSeconds(-1), TimeSpan.FromSeconds(1), CancellationToken.None))
            {
            }
        };

        await act.Should().ThrowAsync<InvalidDataException>();
    }

    [Test]
    public async Task GetRange_Async_Should_Stop_OnCancellation()
    {
        var from = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = from.AddHours(10);
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var ranges = new List<(DateTime, DateTime)>();
        await foreach (var frame in DateTimeUtils.GetRange(from, to, TimeSpan.FromMinutes(1), cts.Token))
            ranges.Add(frame);

        ranges.Should().BeEmpty();
    }
}
