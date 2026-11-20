using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics.Handlers;
using Botticelli.Client.Analytics.Requests;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Botticelli.Client.Analytics.Tests.Handlers;

[TestFixture]
public class MetricsHandlerTests
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
    public async Task Handle_Should_PublishConvertedMetric()
    {
        var publisher = new MetricsPublisher(new AnalyticsClientSettings { TargetUrl = _server.Urls[0] });
        var logger = new Mock<ILogger>();
        var handler = new TestMetricsHandler(
            new BotContext { BotId = "bot-1", BotKey = "key" },
            publisher,
            logger.Object);

        await handler.Handle(new TestMetricRequest { MetricName = MetricNames.CommandReceived }, CancellationToken.None);

        _server.LogEntries.Should().Contain(e =>
            e.RequestMessage.Path == "/metrics/receiver/ReceiveMetric");
    }

    [Test]
    public async Task Handle_Should_SwallowExceptions_AndLogError()
    {
        var publisher = new MetricsPublisher(new AnalyticsClientSettings
        {
            TargetUrl = "http://127.0.0.1:1"
        });
        var logger = new Mock<ILogger>();
        var handler = new TestMetricsHandler(
            new BotContext { BotId = "bot-1", BotKey = "key" },
            publisher,
            logger.Object);

        var act = async () => await handler.Handle(
            new TestMetricRequest { MetricName = MetricNames.BotError },
            CancellationToken.None);

        await act.Should().NotThrowAsync();
        logger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Test]
    public void Convert_Should_MapRequestToMetricObject()
    {
        var publisher = new MetricsPublisher(new AnalyticsClientSettings { TargetUrl = _server.Urls[0] });
        var handler = new TestMetricsHandler(
            new BotContext { BotId = "bot-9", BotKey = "key" },
            publisher,
            Mock.Of<ILogger>());

        var metric = handler.ConvertPublic(new TestMetricRequest { MetricName = MetricNames.MessageReceived }, "bot-9");

        metric.BotId.Should().Be("bot-9");
        metric.Name.Should().Be(MetricNames.MessageReceived);
        metric.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    private sealed class TestMetricRequest : IMetricRequest
    {
        public string MetricName { get; set; } = string.Empty;
    }

    private sealed class TestMetricsHandler : MetricsHandler<TestMetricRequest>
    {
        public TestMetricsHandler(BotContext context, MetricsPublisher metricsPublisher, ILogger logger)
            : base(context, metricsPublisher, logger)
        {
        }

        public MetricObject ConvertPublic(IMetricRequest args, string botId) => Convert(args, botId);
    }
}
