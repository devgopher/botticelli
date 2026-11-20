using System.Reflection;
using Botticelli.Client.Analytics;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Controls.Layouts.CommandProcessors.InlineCalendar;
using Botticelli.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Controls.Layouts.Inlines;
using Botticelli.Controls.Parsers;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Validation;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Botticelli.Controls.Layouts.Tests.CommandProcessors;

[TestFixture]
public class ICCommandProcessorTests
{
    [Test]
    public async Task InnerProcess_MonthForward_Should_UpdateMessage()
    {
        await AssertProcessesAsync<MonthForwardCommand>();
    }

    [Test]
    public async Task InnerProcess_MonthBackward_Should_UpdateMessage()
    {
        await AssertProcessesAsync<MonthBackwardCommand>();
    }

    [Test]
    public async Task InnerProcess_YearForward_Should_UpdateMessage()
    {
        await AssertProcessesAsync<YearForwardCommand>();
    }

    [Test]
    public async Task InnerProcess_YearBackward_Should_UpdateMessage()
    {
        await AssertProcessesAsync<YearBackwardCommand>();
    }

    [Test]
    public async Task InnerProcess_UnknownCommandType_Should_UseCurrentCalendar()
    {
        await AssertProcessesAsync<SetDateCommand>();
    }

    [Test]
    public async Task InnerProcess_Should_Return_WhenDateCannotBeParsed()
    {
        var layoutSupplier = new Mock<ILayoutSupplier<object>>();
        var processor = CreateProcessor<MonthForwardCommand>(layoutSupplier.Object);
        processor.SetBot(new Mock<IBot>().Object);

        await InvokeInnerProcess(processor, new Message("cmd") { Body = "/MonthForward not-a-date" });

        layoutSupplier.Verify(s => s.GetMarkup(It.IsAny<ILayout>()), Times.Never);
    }

    private static async Task AssertProcessesAsync<TCommand>()
        where TCommand : BaseCalendarCommand
    {
        var layoutSupplier = new Mock<ILayoutSupplier<object>>();
        layoutSupplier.Setup(s => s.GetMarkup(It.IsAny<ILayout>())).Returns(new object());

        var bot = new Mock<IBot>();
        bot.Setup(b => b.UpdateMessageAsync(
                It.IsAny<SendMessageRequest>(),
                It.IsAny<ISendOptionsBuilder<object>?>(),
                It.IsAny<CancellationToken>()))
           .ReturnsAsync(SendMessageResponse.GetInstance(Guid.NewGuid().ToString(), "OK"));

        var processor = CreateProcessor<TCommand>(layoutSupplier.Object);
        processor.SetBot(bot.Object);

        var message = new Message("cmd")
        {
            Body = $"/MonthForward {DateTime.Today}",
            ChatIds = ["chat-1"]
        };

        await InvokeInnerProcess(processor, message);

        layoutSupplier.Verify(s => s.GetMarkup(It.IsAny<InlineCalendar>()), Times.Once);
        bot.Verify(b => b.UpdateMessageAsync(
            It.IsAny<SendMessageRequest>(),
            It.IsAny<ISendOptionsBuilder<object>?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private static ICCommandProcessor<TCommand, object> CreateProcessor<TCommand>(ILayoutSupplier<object> layoutSupplier)
        where TCommand : BaseCalendarCommand
    {
        var logger = new Mock<ILogger<ICCommandProcessor<TCommand, object>>>();
        var metrics = new MetricsProcessor(new MetricsPublisher(new AnalyticsClientSettings
        {
            TargetUrl = "http://127.0.0.1:1"
        }));

        return new ICCommandProcessor<TCommand, object>(
            logger.Object,
            new PassValidator<TCommand>(),
            layoutSupplier,
            metrics,
            new MessageValidator());
    }

    private static async Task InvokeInnerProcess(object processor, Message message)
    {
        var method = processor.GetType().GetMethod(
            "InnerProcess",
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            types: [typeof(Message), typeof(CancellationToken)],
            modifiers: null);

        method.Should().NotBeNull();
        var task = (Task)method!.Invoke(processor, [message, CancellationToken.None])!;
        await task;
    }
}
