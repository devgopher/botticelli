using Botticelli.Broadcasting.Dal.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Broadcasting.Dal.Tests.Models;

[TestFixture]
public class DalModelsTests
{
    [Test]
    public void Chat_Should_ExposeProperties()
    {
        var chat = new Chat
        {
            ChatId = "chat-1",
            IsActive = true
        };

        chat.ChatId.Should().Be("chat-1");
        chat.IsActive.Should().BeTrue();
    }

    [Test]
    public void MessageCache_Should_StoreSerializedObject()
    {
        var cache = new MessageCache("id-1", "{\"body\":\"hi\"}")
        {
            Id = "id-1",
            SerializedMessageObject = "{\"body\":\"hi\"}"
        };

        cache.Id.Should().Be("id-1");
        cache.SerializedMessageObject.Should().Contain("hi");
    }

    [Test]
    public void MessageStatus_Should_ExposeDeliveryFields()
    {
        var created = DateTime.UtcNow;
        var status = new MessageStatus
        {
            MessageId = "m1",
            ChatId = "c1",
            IsSent = true,
            CreatedDate = created,
            SentDate = created.AddSeconds(2)
        };

        status.IsSent.Should().BeTrue();
        status.SentDate.Should().Be(created.AddSeconds(2));
        status.MessageId.Should().Be("m1");
        status.ChatId.Should().Be("c1");
    }
}
