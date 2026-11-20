using Botticelli.Server.Back.Controllers;
using Botticelli.Server.Back.Services;
using Botticelli.Server.Back.Services.Broadcasting;
using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Controllers;

[TestFixture]
public class BotControllerTests
{
    private Mock<IBotManagementService> _management = null!;
    private Mock<IBotStatusDataService> _status = null!;
    private Mock<IBroadcastService> _broadcast = null!;
    private BotController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _management = new Mock<IBotManagementService>();
        _status = new Mock<IBotStatusDataService>();
        _broadcast = new Mock<IBroadcastService>();
        _controller = new BotController(_management.Object,
                                        _status.Object,
                                        _broadcast.Object,
                                        NullLogger<BotController>.Instance);
    }

    [Test]
    public async Task GetRequiredBotStatus_Should_ReturnContext()
    {
        _status.Setup(s => s.GetBotInfo("bot-1")).ReturnsAsync(new BotInfo
        {
            BotId = "bot-1",
            BotName = "N",
            BotKey = "key",
            Type = BotType.Telegram,
            Status = BotStatus.Unlocked,
            AdditionalInfo =
            [
                new BotAdditionalInfo { BotId = "bot-1", ItemName = "a", ItemValue = "b" }
            ]
        });
        _status.Setup(s => s.GetRequiredBotStatus("bot-1")).ReturnsAsync(BotStatus.Unlocked);

        var response = await _controller.GetRequiredBotStatus(new GetRequiredStatusFromServerRequest
        {
            BotId = "bot-1"
        });

        response.IsSuccess.Should().BeTrue();
        response.Status.Should().Be(BotStatus.Unlocked);
        response.BotContext!.BotKey.Should().Be("key");
        response.BotContext.Items.Should().ContainKey("a");
    }

    [Test]
    public async Task KeepAlive_Should_ReturnSuccess()
    {
        var response = await _controller.KeepAlive(new KeepAliveNotificationRequest { BotId = "bot-1" });

        response.IsSuccess.Should().BeTrue();
        _management.Verify(m => m.SetKeepAlive("bot-1"), Times.Once);
    }

    [Test]
    public async Task KeepAlive_Should_ReturnFailure_OnError()
    {
        _management.Setup(m => m.SetKeepAlive("bot-1")).ThrowsAsync(new InvalidOperationException("boom"));

        var response = await _controller.KeepAlive(new KeepAliveNotificationRequest { BotId = "bot-1" });

        response.IsSuccess.Should().BeFalse();
    }

    [Test]
    public async Task GetBroadcast_Should_ReturnMessages()
    {
        _broadcast.Setup(b => b.GetMessages("bot-1")).ReturnsAsync([
            new Broadcast
            {
                Id = "m1",
                BotId = "bot-1",
                Body = "hello",
                Attachments =
                [
                    new BroadcastAttachment
                    {
                        Id = Guid.NewGuid(),
                        BroadcastId = "m1",
                        Filename = "a.png",
                        MediaType = MediaType.Image,
                        Content = [1]
                    }
                ]
            }
        ]);

        var response = await _controller.GetBroadcast(new GetBroadCastMessagesRequest { BotId = "bot-1" });

        response.IsSuccess.Should().BeTrue();
        response.Messages.Should().ContainSingle(m => m.Uid == "m1" && m.Body == "hello");
        response.Messages![0].Attachments.Should().ContainSingle();
    }

    [Test]
    public async Task GetBroadcast_Should_ReturnFailure_OnError()
    {
        _broadcast.Setup(b => b.GetMessages("bot-1")).ThrowsAsync(new InvalidOperationException("boom"));

        var response = await _controller.GetBroadcast(new GetBroadCastMessagesRequest { BotId = "bot-1" });

        response.IsSuccess.Should().BeFalse();
        response.Messages.Should().BeEmpty();
    }

    [Test]
    public async Task BroadcastReceived_Should_MarkMessages()
    {
        var response = await _controller.BroadcastReceived(new BroadCastMessagesReceivedRequest
        {
            BotId = "bot-1",
            MessageIds = ["m1", "m2"]
        });

        response.IsSuccess.Should().BeTrue();
        _broadcast.Verify(b => b.MarkReceived("bot-1", "m1"), Times.Once);
        _broadcast.Verify(b => b.MarkReceived("bot-1", "m2"), Times.Once);
    }

    [Test]
    public async Task BroadcastReceived_Should_ReturnFailure_OnError()
    {
        _broadcast.Setup(b => b.MarkReceived(It.IsAny<string>(), It.IsAny<string>()))
                  .ThrowsAsync(new InvalidOperationException("boom"));

        var response = await _controller.BroadcastReceived(new BroadCastMessagesReceivedRequest
        {
            BotId = "bot-1",
            MessageIds = ["m1"]
        });

        response.IsSuccess.Should().BeFalse();
    }
}
