using Botticelli.Interfaces;
using Botticelli.Schedule.Quartz;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Quartz;

namespace Botticelli.Scheduler.Quartz.Tests;

[TestFixture]
public class ReliableSendMessageJobTests
{
    [Test]
    public async Task Execute_Should_ThrowNotImplemented()
    {
        var job = new ReliableSendMessageJob(new Mock<IBot>().Object);
        var context = new Mock<IJobExecutionContext>();

        var act = async () => await job.Execute(context.Object);

        await act.Should().ThrowAsync<NotImplementedException>();
    }
}
