using Botticelli.Broadcasting.Shared.Requests;
using Botticelli.Broadcasting.Shared.Responses;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Broadcasting.Shared.Tests;

[TestFixture]
public class RequestsAndResponsesTests
{
    [Test]
    public void GetBroadcastMessagesRequest_Should_HoldValues()
    {
        var request = new GetBroadcastMessagesRequest
        {
            BotId = "bot-1",
            HowOld = TimeSpan.FromMinutes(5)
        };

        request.BotId.Should().Be("bot-1");
        request.HowOld.Should().Be(TimeSpan.FromMinutes(5));
    }

    [Test]
    public void GetBroadcastMessagesResponse_Should_HoldMessages()
    {
        var response = new GetBroadcastMessagesResponse
        {
            Id = "resp-1",
            Messages =
            [
                new Message("m1") { Body = "hello" },
                new Message("m2") { Body = "world" }
            ]
        };

        response.Id.Should().Be("resp-1");
        response.Messages.Should().HaveCount(2);
        response.Messages[0].Uid.Should().Be("m1");
    }
}
