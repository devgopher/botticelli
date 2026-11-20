using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics.Settings;
using FluentAssertions;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Botticelli.Client.Analytics.Tests;

[TestFixture]
public class MetricsPublisherTests
{
    private WireMockServer _server = null!;

    [SetUp]
    public void SetUp()
    {
        _server = WireMockServer.Start();
        _server
            .Given(Request.Create().WithPath("/metrics/receiver/ReceiveMetric").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200));
    }

    [TearDown]
    public void TearDown()
    {
        _server.Stop();
        _server.Dispose();
    }

    [Test]
    public async Task Publish_Should_SendGetRequestToReceiver()
    {
        var publisher = new MetricsPublisher(new AnalyticsClientSettings
        {
            TargetUrl = _server.Urls[0]
        });

        var metric = new MetricObject
        {
            BotId = "bot-1",
            Name = MetricNames.MessageSent,
            Timestamp = DateTime.UtcNow
        };

        await publisher.Publish(metric, CancellationToken.None);

        _server.LogEntries.Should().Contain(e =>
            e.RequestMessage.Path == "/metrics/receiver/ReceiveMetric");
    }
}
