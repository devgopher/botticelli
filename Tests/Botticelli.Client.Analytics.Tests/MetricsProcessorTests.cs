using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics.Settings;
using FluentAssertions;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Botticelli.Client.Analytics.Tests;

[TestFixture]
public class MetricsProcessorTests
{
    private WireMockServer _server = null!;
    private MetricsProcessor _processor = null!;

    [SetUp]
    public void SetUp()
    {
        _server = WireMockServer.Start();
        _server
            .Given(Request.Create().WithPath("/metrics/receiver/ReceiveMetric").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200));

        var publisher = new MetricsPublisher(new AnalyticsClientSettings
        {
            TargetUrl = _server.Urls[0]
        });
        _processor = new MetricsProcessor(publisher);
    }

    [TearDown]
    public void TearDown()
    {
        _server.Stop();
        _server.Dispose();
    }

    [Test]
    public async Task Process_ByName_Should_PublishMetric()
    {
        _processor.Process(MetricNames.BotStarted, "bot-42");

        await WaitForRequestsAsync(1);

        _server.LogEntries.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Test]
    public async Task Process_ByObject_Should_PublishMetric()
    {
        var metric = new MetricObject
        {
            BotId = "bot-7",
            Name = MetricNames.BotError,
            Timestamp = DateTime.UtcNow
        };

        _processor.Process(metric);

        await WaitForRequestsAsync(1);

        _server.LogEntries.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    private async Task WaitForRequestsAsync(int expected)
    {
        for (var i = 0; i < 50; i++)
        {
            if (_server.LogEntries.Count() >= expected)
                return;

            await Task.Delay(50);
        }

        _server.LogEntries.Count().Should().BeGreaterThanOrEqualTo(expected);
    }
}
