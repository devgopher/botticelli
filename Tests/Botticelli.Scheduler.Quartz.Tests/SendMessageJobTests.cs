using System.Text.Json;
using Botticelli.Interfaces;
using Botticelli.Schedule.Quartz;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Quartz;

namespace Botticelli.Scheduler.Quartz.Tests;

[TestFixture]
public class SendMessageJobTests
{
    [Test]
    public async Task Execute_Should_DeserializeAndSendMessage()
    {
        var bot = new Mock<IBot>();
        bot.Setup(b => b.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(SendMessageResponse.GetInstance(Guid.NewGuid().ToString(), "OK"));

        var request = new SendMessageRequest { Message = new Message("s1") { Body = "payload" } };
        var context = CreateContext(JsonSerializer.Serialize(request));
        var job = new SendMessageJob(bot.Object);

        await job.Execute(context.Object);

        bot.Verify(b => b.SendMessageAsync(
            It.Is<SendMessageRequest>(r => r.Message!.Body == "payload"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Execute_Should_Skip_WhenJobDetailMissing()
    {
        var bot = new Mock<IBot>();
        var context = new Mock<IJobExecutionContext>();
        context.SetupGet(c => c.JobDetail).Returns((IJobDetail?)null);
        context.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);
        var job = new SendMessageJob(bot.Object);

        await job.Execute(context.Object);

        bot.Verify(b => b.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Execute_Should_Throw_WhenDeserializedRequestIsNull()
    {
        var bot = new Mock<IBot>();
        var context = CreateContext("null");
        var job = new SendMessageJob(bot.Object);

        var act = async () => await job.Execute(context.Object);

        await act.Should().ThrowAsync<NullReferenceException>();
    }

    private static Mock<IJobExecutionContext> CreateContext(object requestPayload)
    {
        var dataMap = new JobDataMap();
        dataMap.Put("sendMessageRequest", requestPayload);

        var jobDetail = new Mock<IJobDetail>();
        jobDetail.SetupGet(j => j.JobDataMap).Returns(dataMap);

        var context = new Mock<IJobExecutionContext>();
        context.SetupGet(c => c.JobDetail).Returns(jobDetail.Object);
        context.SetupGet(c => c.CancellationToken).Returns(CancellationToken.None);
        return context;
    }
}
