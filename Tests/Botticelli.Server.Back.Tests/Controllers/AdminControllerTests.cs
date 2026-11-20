using System.Text;
using System.Text.Json;
using Botticelli.Server.Back.Controllers;
using Botticelli.Server.Back.Services;
using Botticelli.Server.Back.Services.Broadcasting;
using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Controllers;

[TestFixture]
public class AdminControllerTests
{
    private Mock<IBotManagementService> _management = null!;
    private Mock<IBotStatusDataService> _status = null!;
    private Mock<IBroadcastService> _broadcast = null!;
    private AdminController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _management = new Mock<IBotManagementService>();
        _status = new Mock<IBotStatusDataService>();
        _broadcast = new Mock<IBroadcastService>();
        _controller = new AdminController(_management.Object,
                                          _status.Object,
                                          NullLogger<AdminController>.Instance,
                                          _broadcast.Object)
        {
            ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Test]
    public async Task AddNewBot_Should_ReturnSuccess()
    {
        _management.Setup(m => m.RegisterBot("b1", "k", "n", BotType.Telegram, null))
                   .ReturnsAsync(true);

        var response = await _controller.AddNewBot(new RegisterBotRequest
        {
            BotId = "b1",
            BotKey = "k",
            BotName = "n",
            Type = BotType.Telegram
        });

        response.IsSuccess.Should().BeTrue();
        response.BotId.Should().Be("b1");
    }

    [Test]
    public async Task UpdateBot_Should_ReturnSuccess()
    {
        _management.Setup(m => m.UpdateBot("b1", "k", "n", null)).ReturnsAsync(true);

        var response = await _controller.UpdateBot(new UpdateBotRequest
        {
            BotId = "b1",
            BotKey = "k",
            BotName = "n"
        });

        response.IsSuccess.Should().BeTrue();
    }

    [Test]
    public async Task SendBroadcast_Should_CallService()
    {
        var message = new Message("uid-1")
        {
            Body = "hello",
            Attachments =
            [
                new BinaryBaseAttachment("a1", "f.png", MediaType.Image, "", [1, 2])
            ]
        };

        await _controller.SendBroadcast("bot-1", message);

        _broadcast.Verify(b => b.BroadcastMessage(It.Is<Broadcast>(x =>
            x.Id == "uid-1" && x.BotId == "bot-1" && x.Body == "hello" && x.Attachments!.Count == 1)), Times.Once);
    }

    [Test]
    public async Task SendBroadcastBinary_Should_DeserializeAndBroadcast()
    {
        var message = new Message("uid-2") { Body = "bin" };
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _controller.ControllerContext.HttpContext.Request.Body = new MemoryStream(bytes);

        await _controller.SendBroadcastBinary("bot-1");

        _broadcast.Verify(b => b.BroadcastMessage(It.Is<Broadcast>(x => x.Id == "uid-2" && x.Body == "bin")), Times.Once);
    }

    [Test]
    public async Task GetBots_Should_ReturnCollection()
    {
        _status.Setup(s => s.GetBots()).Returns([
            new BotInfo { BotId = "1", BotName = "A", Type = BotType.Telegram, Status = BotStatus.Unlocked }
        ]);

        var bots = await _controller.GetBots();
        bots.Should().ContainSingle(b => b.BotId == "1");
    }

    [Test]
    public async Task ActivateDeactivateRemove_Should_Delegate()
    {
        await _controller.ActivateBot("b1");
        await _controller.DeactivateBot("b1");
        await _controller.RemoveBot("b1");

        _management.Verify(m => m.SetRequiredBotStatus("b1", BotStatus.Unlocked), Times.Once);
        _management.Verify(m => m.SetRequiredBotStatus("b1", BotStatus.Locked), Times.Once);
        _management.Verify(m => m.RemoveBot("b1"), Times.Once);
    }
}
