using Botticelli.Server.Back.Services.Broadcasting;
using Botticelli.Server.Back.Tests.Helpers;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Botticelli.Shared.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Services;

[TestFixture]
public class BroadcastServiceTests
{
    [Test]
    public async Task BroadcastMessage_Should_Persist()
    {
        await using var context = TestServerDataContextFactory.Create();
        var service = new BroadcastService(context);

        await service.BroadcastMessage(new Broadcast
        {
            Id = "m1",
            BotId = "bot-1",
            Body = "hi",
            Attachments =
            [
                new BroadcastAttachment
                {
                    Id = Guid.NewGuid(),
                    BroadcastId = "m1",
                    MediaType = MediaType.Image,
                    Filename = "a.png",
                    Content = [1]
                }
            ]
        });

        context.BroadcastMessages.Should().ContainSingle(m => m.Id == "m1" && m.Body == "hi");
    }

    [Test]
    public async Task GetMessages_Should_ReturnOnlyUnreceivedForBot()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BroadcastMessages.AddRange(
            new Broadcast { Id = "1", BotId = "bot-1", Body = "a", Received = false },
            new Broadcast { Id = "2", BotId = "bot-1", Body = "b", Received = true },
            new Broadcast { Id = "3", BotId = "bot-2", Body = "c", Received = false });
        await context.SaveChangesAsync();
        var service = new BroadcastService(context);

        var messages = (await service.GetMessages("bot-1")).ToList();

        messages.Should().ContainSingle(m => m.Id == "1");
    }

    [Test]
    public async Task MarkReceived_Should_FlagMessages()
    {
        await using var context = TestServerDataContextFactory.Create();
        context.BroadcastMessages.Add(new Broadcast
        {
            Id = "m1",
            BotId = "bot-1",
            Body = "a",
            Received = false
        });
        await context.SaveChangesAsync();
        var service = new BroadcastService(context);

        await service.MarkReceived("bot-1", "m1");

        context.BroadcastMessages.Single().Received.Should().BeTrue();
    }
}
