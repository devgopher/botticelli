using Botticelli.Interfaces;
using Botticelli.Scheduler.Interfaces;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Scheduler.Tests.Interfaces;

[TestFixture]
public class InterfaceContractsTests
{
    [Test]
    public void ISchedule_Should_ExposeNameAndCron()
    {
        ISchedule schedule = new TestSchedule();

        schedule.Name.Should().Be("daily");
        schedule.Cron.Should().Be("0 0 * * *");
    }

    [Test]
    public void IJobManager_Should_BeImplementedByStub()
    {
        IJobManager manager = new StubJobManager();

        var jobId = manager.AddJob(
            bot: null!,
            reliability: new Reliability { IsEnabled = false },
            message: new Message("msg-1"),
            schedule: new Schedule { Cron = "* * * * *" },
            preprocessFunc: m => m.Body = "pre");

        jobId.Should().Be("job-1");
        manager.Invoking(m => m.RemoveJob("job-1")).Should().NotThrow();
        manager.Invoking(m => m.RemoveAllJobs()).Should().NotThrow();
    }

    private sealed class TestSchedule : ISchedule
    {
        public string Name => "daily";
        public string Cron => "0 0 * * *";
    }

    private sealed class StubJobManager : IJobManager
    {
        public string AddJob(IBot bot, Reliability reliability, Message message, Schedule schedule, Action<Message>? preprocessFunc = null)
        {
            preprocessFunc?.Invoke(message);
            return "job-1";
        }

        public void RemoveJob(string triggerId)
        {
        }

        public void RemoveAllJobs()
        {
        }
    }
}
