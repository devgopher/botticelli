using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Data.Entities.Tests;

[TestFixture]
public class BotEntitiesTests
{
    [Test]
    public void BotAdditionalInfo_Should_HoldValues()
    {
        var info = new BotAdditionalInfo
        {
            BotId = "bot-1",
            ItemName = "key",
            ItemValue = "val"
        };

        info.BotId.Should().Be("bot-1");
        info.ItemName.Should().Be("key");
        info.ItemValue.Should().Be("val");
    }

    [Test]
    public void BotInfo_Should_HoldValues()
    {
        var keepAlive = DateTime.UtcNow;
        var bot = new BotInfo
        {
            BotId = "bot-1",
            BotName = "MyBot",
            LastKeepAlive = keepAlive,
            Status = BotStatus.Unlocked,
            Type = BotType.Telegram,
            BotKey = "key",
            AdditionalInfo =
            [
                new BotAdditionalInfo { BotId = "bot-1", ItemName = "a", ItemValue = "b" }
            ]
        };

        bot.BotId.Should().Be("bot-1");
        bot.BotName.Should().Be("MyBot");
        bot.LastKeepAlive.Should().Be(keepAlive);
        bot.Status.Should().Be(BotStatus.Unlocked);
        bot.Type.Should().Be(BotType.Telegram);
        bot.BotKey.Should().Be("key");
        bot.AdditionalInfo.Should().ContainSingle();
    }

    [Test]
    public void Broadcast_Should_HoldValuesAndDefaults()
    {
        var broadcast = new Broadcast
        {
            Id = "msg-1",
            BotId = "bot-1",
            Body = "hello",
            Timestamp = DateTime.UtcNow,
            Attachments = []
        };

        broadcast.Id.Should().Be("msg-1");
        broadcast.BotId.Should().Be("bot-1");
        broadcast.Body.Should().Be("hello");
        broadcast.Sent.Should().BeFalse();
        broadcast.Received.Should().BeFalse();
        broadcast.Attachments.Should().BeEmpty();
    }

    [Test]
    public void BroadcastAttachment_Should_HoldValues()
    {
        var id = Guid.NewGuid();
        var attachment = new BroadcastAttachment
        {
            Id = id,
            BroadcastId = "msg-1",
            MediaType = MediaType.Image,
            Filename = "pic.png",
            Content = [1, 2, 3]
        };

        attachment.Id.Should().Be(id);
        attachment.BroadcastId.Should().Be("msg-1");
        attachment.MediaType.Should().Be(MediaType.Image);
        attachment.Filename.Should().Be("pic.png");
        attachment.Content.Should().Equal(1, 2, 3);
    }
}
