using Botticelli.Bus.ZeroMQ.Agent;
using Botticelli.Bus.ZeroMQ.Settings;
using Botticelli.Bus.ZeroMQTests.Handler;
using Botticelli.Bus.ZeroMQTests.Mocks;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Botticelli.Bus.ZeroMQTests.Agent;

[TestFixture]
public class ZeroMqAgentTests
{
    [SetUp]
    public async Task Setup()
    {
        await _agent.Subscribe(CancellationToken.None);
    }

    private readonly ZeroMqAgent<BotMock, TestHandler> _agent;

    public ZeroMqAgentTests() =>
            _agent = new ZeroMqAgent<BotMock, TestHandler>(new ServiceProviderMock(),
                                                           new ZeroMqBusSettings
                                                           {
                                                               Timeout = TimeSpan.FromMinutes(5),
                                                               TargetUri = "tcp://localhost:5021",
                                                               ListenUri = "tcp://localhost:5022"
                                                           },
                                                           Mock.Of<ILogger<ZeroMqAgent<BotMock, TestHandler>>>());

    [Test]
    public async Task SendResponseAsyncTest()
    {
        var response = SendMessageResponse.GetInstance(Guid.NewGuid().ToString(), "test response");

        Assert.DoesNotThrowAsync(async () => await _agent.SendResponseAsync(response, CancellationToken.None, 60000));
    }

    [Test]
    public async Task StartAsyncTest()
        => Assert.DoesNotThrowAsync(async () => await _agent.StartAsync(CancellationToken.None));

    [Test]
    public void StopAsyncTest()
        => Assert.DoesNotThrowAsync(async () => await _agent.StopAsync(CancellationToken.None));

    [Test]
    public void SubscribeTest()
        => Assert.DoesNotThrowAsync(async () => await _agent.Subscribe(CancellationToken.None));
}