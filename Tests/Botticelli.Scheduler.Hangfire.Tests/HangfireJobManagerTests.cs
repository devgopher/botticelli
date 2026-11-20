using Botticelli.Scheduler;
using Botticelli.Scheduler.Hangfire;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.Storage;
using Mocks;
using NUnit.Framework;

namespace Botticelli.Scheduler.Hangfire.Tests;

[TestFixture]
public class HangfireJobManagerTests
{
    private HangfireJobManager _manager = null!;
    private BotMock _bot = null!;

    [SetUp]
    public void SetUp()
    {
        GlobalConfiguration.Configuration.UseMemoryStorage();
        JobStorage.Current = new MemoryStorage();
        _manager = new HangfireJobManager();
        _bot = new BotMock();
    }

    [TearDown]
    public void TearDown()
    {
        _manager.RemoveAllJobs();
    }

    [Test]
    public void AddJob_WithoutReliability_Should_RegisterAndReturnId()
    {
        var message = new Message("m1") { Body = "hello" };
        var preprocessed = false;

        var jobId = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = false },
            message,
            new Schedule { Cron = "* * * * *" },
            m => preprocessed = true);

        jobId.Should().NotBeNullOrWhiteSpace();
        preprocessed.Should().BeTrue();
        GetRecurringJobIds().Should().Contain(jobId);
    }

    [Test]
    public void AddJob_WithReliability_Should_ReturnJobIdAndAllowRemoval()
    {
        var jobId = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = true, MaxTries = 3 },
            new Message("m2") { Body = "reliable" },
            new Schedule { Cron = "* * * * *" });

        jobId.Should().NotBeNullOrWhiteSpace();
        _manager.Invoking(m => m.RemoveJob(jobId)).Should().NotThrow();
    }

    [Test]
    public void RemoveJob_Should_UnregisterJob()
    {
        var jobId = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = false },
            new Message("m3"),
            new Schedule { Cron = "* * * * *" });

        _manager.RemoveJob(jobId);

        GetRecurringJobIds().Should().NotContain(jobId);
    }

    [Test]
    public void RemoveAllJobs_Should_ClearRegisteredJobs()
    {
        var first = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = false },
            new Message("m4"),
            new Schedule { Cron = "* * * * *" });
        var second = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = false },
            new Message("m5"),
            new Schedule { Cron = "* * * * *" });

        _manager.RemoveAllJobs();

        var ids = GetRecurringJobIds();
        ids.Should().NotContain(first);
        ids.Should().NotContain(second);
    }

    [Test]
    public async Task SendWithReliability_Should_ThrowNotImplemented()
    {
        var act = async () => await _manager.SendWithReliability(
            _bot,
            new SendMessageRequest { Message = new Message("m6") },
            new Reliability { IsEnabled = true },
            CancellationToken.None);

        await act.Should().ThrowAsync<NotImplementedException>();
    }

    private static IReadOnlyCollection<string> GetRecurringJobIds()
    {
        using var connection = JobStorage.Current.GetConnection();
        return connection.GetRecurringJobs().Select(j => j.Id).ToArray();
    }
}
