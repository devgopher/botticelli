using Botticelli.Schedule.Quartz;
using Botticelli.Scheduler;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Mocks;
using NUnit.Framework;
using Quartz;
using Quartz.Impl;

namespace Botticelli.Scheduler.Quartz.Tests;

[TestFixture]
public class QuartzJobManagerTests
{
    private StdSchedulerFactory _factory = null!;
    private QuartzJobManager _manager = null!;
    private BotMock _bot = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new StdSchedulerFactory();
        _manager = new QuartzJobManager(_factory);
        _bot = new BotMock();
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            _manager.RemoveAllJobs();
        }
        catch (ObjectDisposedException)
        {
            // already disposed in a dedicated test
        }

        try
        {
            _manager.Dispose();
        }
        catch (ObjectDisposedException)
        {
        }
    }

    [Test]
    public void AddJob_WithoutReliability_Should_ScheduleAndReturnTriggerId()
    {
        var preprocessed = false;

        var triggerId = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = false },
            new Message("q1") { Body = "hi" },
            new Schedule { Cron = "0 0/1 * * * ?" },
            m => preprocessed = true);

        triggerId.Should().StartWith("sendMessageJobGroupTrigger_");
        preprocessed.Should().BeTrue();
    }

    [Test]
    public void AddJob_WithReliability_Should_ScheduleReliableJob()
    {
        var triggerId = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = true },
            new Message("q2") { Body = "rel" },
            new Schedule { Cron = "0 0/1 * * * ?" });

        triggerId.Should().StartWith("sendMessageJobGroupTrigger_");
    }

    [Test]
    public void AddJob_WithInvalidCron_Should_Throw()
    {
        var act = () => _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = false },
            new Message("q3"),
            new Schedule { Cron = "not-a-cron" });

        act.Should().Throw<InvalidDataException>().WithMessage("*invalid*");
    }

    [Test]
    public void RemoveJob_Should_UnscheduleExistingTrigger()
    {
        var triggerId = _manager.AddJob(
            _bot,
            new Reliability { IsEnabled = false },
            new Message("q4"),
            new Schedule { Cron = "0 0/1 * * * ?" });

        _manager.Invoking(m => m.RemoveJob(triggerId)).Should().NotThrow();
        _manager.Invoking(m => m.RemoveJob("missing")).Should().NotThrow();
    }

    [Test]
    public void RemoveAllJobs_WithoutScheduler_Should_NotThrow()
    {
        using var fresh = new QuartzJobManager(_factory);
        fresh.Invoking(m => m.RemoveAllJobs()).Should().NotThrow();
    }

    [Test]
    public void Dispose_Should_CancelTokenSource()
    {
        _manager.Invoking(m => m.Dispose()).Should().NotThrow();
    }
}
