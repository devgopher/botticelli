using Botticelli.Bus.ZeroMQ.Client;
using Botticelli.Bus.ZeroMQ.Settings;
using Botticelli.Bus.ZeroMQTests.Handler;
using Botticelli.Bus.ZeroMQTests.Mocks;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Botticelli.Bus.ZeroMQTests.Client;

[TestFixture]
public class ZeroMqClientTests
{
    private readonly ZeroMqClient<BotMock> _client;
    private readonly AgentMock<BotMock, TestHandler> _agent;

    public ZeroMqClientTests()
    {
        var settings = new ZeroMqBusSettings
        {
            Timeout = TimeSpan.FromMinutes(5),
            TargetUri = "tcp://localhost:5022",
            ListenUri = "tcp://localhost:5021"
        };

        _client = new ZeroMqClient<BotMock>(settings,
                                            Mock.Of<ILogger<ZeroMqClient<BotMock>>>());

        _agent = new AgentMock<BotMock, TestHandler>(settings);
    }

    [Test]
    [TestCase]
    [TestCase]
    [TestCase]
    public async Task SendResponse()
    {
        var uid = Guid.NewGuid().ToString();
        var response = SendMessageResponse.GetInstance(uid, "test response");

        response.Message = new Message(Guid.NewGuid().ToString())
        {
            Uid = uid,
            ChatIds = new() {"ASDFGH"},
            Body = "1234567890"
        };

        await _client.SendResponse(response, CancellationToken.None);
    }

    [Test]
    public async Task GetResponseTest()
    {
        var request = SendMessageRequest.GetInstance();
        request.Message = new Message(Guid.NewGuid().ToString())
        {
            Body = "test"
        };

        await _agent.SendResponseAsync(new SendMessageResponse(request.Uid, "test"),
                                       CancellationToken.None);

        var response = await _client.SendAndGetResponse(request, CancellationToken.None);

        Assert.Fail();
    }
}