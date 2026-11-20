using System.Net;
using System.Text.Json;
using Botticelli.Broadcasting.Dal;
using Botticelli.Broadcasting.Dal.Models;
using Botticelli.Broadcasting.Settings;
using Botticelli.Broadcasting.Tests.Helpers;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Botticelli.Broadcasting.Tests;

[TestFixture]
public class BroadcastReceiverTests
{
    private WireMockServer _server = null!;
    private string _dbPath = null!;

    [SetUp]
    public void SetUp()
    {
        _server = WireMockServer.Start();
        _dbPath = Path.Combine(Path.GetTempPath(), $"bcast-{Guid.NewGuid():N}.db");
    }

    [TearDown]
    public void TearDown()
    {
        _server.Stop();
        _server.Dispose();
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch (IOException)
        {
            // SQLite may keep a short-lived lock after dispose.
        }
    }

    private BroadcastingContext CreateContext(params Chat[] chats)
    {
        var options = new DbContextOptionsBuilder<BroadcastingContext>()
            .UseSqlite($"Data Source={_dbPath};Cache=Shared")
            .Options;
        var context = new BroadcastingContext(options);
        context.Database.EnsureCreated();
        if (chats.Length > 0)
        {
            context.Chats.AddRange(chats);
            context.SaveChanges();
        }

        return context;
    }

    private static async Task StopReceiverAsync(BroadcastReceiver<TestBroadcastBot> receiver, CancellationTokenSource cts)
    {
        await cts.CancelAsync();
        try { await receiver.StopAsync(CancellationToken.None); } catch { /* ignore */ }
        await Task.Delay(50);
    }

    private BroadcastingSettings CreateSettings() => new()
    {
        BotId = "bot-1",
        ServerUri = _server.Urls[0],
        ConnectionString = _dbPath
    };

    [Test]
    public async Task StartAndStop_Should_Complete()
    {
        _server
            .Given(Request.Create().WithPath("/bot/client/GetBroadcast").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(204));

        var bot = new Mock<IBot>();
        await using var context = CreateContext();
        var receiver = new BroadcastReceiver<TestBroadcastBot>(
            bot.Object,
            context,
            CreateSettings(),
            new Mock<ILogger<BroadcastReceiver<TestBroadcastBot>>>().Object);

        using var cts = new CancellationTokenSource();
        await receiver.StartAsync(cts.Token);
        await Task.Delay(200);
        await StopReceiverAsync(receiver, cts);
    }

    [Test]
    public async Task StartAsync_WithMessages_Should_SendAndAck()
    {
        var message = new Message("msg-1")
        {
            Body = "hello",
            ChatIds = ["chat-1"]
        };

        var getResponse = new GetBroadCastMessagesResponse
        {
            BotId = "bot-1",
            IsSuccess = true,
            Messages = [message]
        };

        _server
            .Given(Request.Create().WithPath("/bot/client/GetBroadcast").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(getResponse)));

        _server
            .Given(Request.Create().WithPath("/bot/client/BroadcastReceived").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new BroadCastMessagesReceivedResponse { IsSuccess = true })));

        var bot = new Mock<IBot>();
        bot.Setup(b => b.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new SendMessageResponse("uid", "ok") { MessageSentStatus = MessageSentStatus.Ok });

        await using var context = CreateContext(new Chat { ChatId = "chat-1", IsActive = true });
        var receiver = new BroadcastReceiver<TestBroadcastBot>(
            bot.Object,
            context,
            CreateSettings(),
            new Mock<ILogger<BroadcastReceiver<TestBroadcastBot>>>().Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        await receiver.StartAsync(cts.Token);

        for (var i = 0; i < 40; i++)
        {
            if (bot.Invocations.Count > 0)
                break;
            await Task.Delay(50);
        }

        await StopReceiverAsync(receiver, cts);

        bot.Verify(b => b.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task StartAsync_WhenNoChatIds_Should_UseAllChatsFromContext()
    {
        var message = new Message("msg-2")
        {
            Body = "broadcast-all",
            ChatIds = []
        };

        _server
            .Given(Request.Create().WithPath("/bot/client/GetBroadcast").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new GetBroadCastMessagesResponse
                {
                    BotId = "bot-1",
                    IsSuccess = true,
                    Messages = [message]
                })));

        _server
            .Given(Request.Create().WithPath("/bot/client/BroadcastReceived").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new BroadCastMessagesReceivedResponse { IsSuccess = true })));

        SendMessageRequest? captured = null;
        var bot = new Mock<IBot>();
        bot.Setup(b => b.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
           .Callback<SendMessageRequest, CancellationToken>((r, _) => captured = r)
           .ReturnsAsync(new SendMessageResponse("uid", "ok") { MessageSentStatus = MessageSentStatus.Ok });

        await using var context = CreateContext(
            new Chat { ChatId = "c1", IsActive = true },
            new Chat { ChatId = "c2", IsActive = true });

        var receiver = new BroadcastReceiver<TestBroadcastBot>(
            bot.Object,
            context,
            CreateSettings(),
            new Mock<ILogger<BroadcastReceiver<TestBroadcastBot>>>().Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        await receiver.StartAsync(cts.Token);

        for (var i = 0; i < 40 && captured is null; i++)
            await Task.Delay(50);

        await StopReceiverAsync(receiver, cts);

        captured.Should().NotBeNull();
        captured!.Message!.ChatIds.Should().BeEquivalentTo("c1", "c2");
    }

    [Test]
    public async Task StartAsync_WhenSendFails_Should_SkipAck()
    {
        _server
            .Given(Request.Create().WithPath("/bot/client/GetBroadcast").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(JsonSerializer.Serialize(new GetBroadCastMessagesResponse
                {
                    BotId = "bot-1",
                    IsSuccess = true,
                    Messages = [new Message("msg-fail") { ChatIds = ["c1"], Body = "x" }]
                })));

        var bot = new Mock<IBot>();
        bot.Setup(b => b.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new SendMessageResponse("uid", "fail") { MessageSentStatus = MessageSentStatus.Fail });

        await using var context = CreateContext(new Chat { ChatId = "c1", IsActive = true });
        var receiver = new BroadcastReceiver<TestBroadcastBot>(
            bot.Object,
            context,
            CreateSettings(),
            new Mock<ILogger<BroadcastReceiver<TestBroadcastBot>>>().Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await receiver.StartAsync(cts.Token);
        await Task.Delay(300);
        await StopReceiverAsync(receiver, cts);

        _server.LogEntries.Any(e => e.RequestMessage.Path?.Contains("BroadcastReceived") == true)
               .Should().BeFalse();
    }

    [Test]
    public async Task StartAsync_WhenGetBroadcastFails_Should_Continue()
    {
        _server
            .Given(Request.Create().WithPath("/bot/client/GetBroadcast").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.InternalServerError));

        var bot = new Mock<IBot>();
        await using var context = CreateContext();
        var receiver = new BroadcastReceiver<TestBroadcastBot>(
            bot.Object,
            context,
            CreateSettings(),
            new Mock<ILogger<BroadcastReceiver<TestBroadcastBot>>>().Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(400));
        await receiver.StartAsync(cts.Token);
        await Task.Delay(250);
        await StopReceiverAsync(receiver, cts);

        bot.Verify(b => b.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
