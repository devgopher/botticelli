using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;
using Botticelli.Server.Analytics.Controllers;
using Botticelli.Server.Analytics.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Analytics.Tests.Controllers;

[TestFixture]
public class MetricsControllersTests
{
    [Test]
    public async Task ReceiveMetric_Should_ReturnOk()
    {
        var service = new Mock<IMetricsInputService>();
        var controller = new MetricsInputController(service.Object);

        var result = await controller.ReceiveMetric(new PushMetricRequest<IMetricObject>
        {
            Object = new MetricObject
            {
                BotId = "b",
                Name = MetricNames.MessageSent,
                Timestamp = DateTime.UtcNow
            }
        }, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        service.Verify(s => s.PushMetricAsync(It.IsAny<PushMetricRequest<IMetricObject>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ReceiveMetric_Should_ReturnBadRequest_OnError()
    {
        var service = new Mock<IMetricsInputService>();
        service.Setup(s => s.PushMetricAsync(It.IsAny<PushMetricRequest<IMetricObject>>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new InvalidOperationException("boom"));
        var controller = new MetricsInputController(service.Object);

        var result = await controller.ReceiveMetric(new PushMetricRequest<IMetricObject>(), CancellationToken.None);

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("boom");
    }

    [Test]
    public async Task OutputController_Should_DelegateToService()
    {
        var service = new Mock<IMetricsOutputService>();
        service.Setup(s => s.GetMetricsAsync(It.IsAny<GetMetricsRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new GetMetricsResponse { Count = 1 });
        service.Setup(s => s.GetMetricsForIntervalAsync(It.IsAny<GetMetricsForIntervalsRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new GetMetricsIntervalsResponse { Count = 2, MetricsForIntervals = [] });
        service.Setup(s => s.GetMetricNamesAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(["A"]);

        var controller = new MetricsOutputController(service.Object);

        (await controller.GetMetrics(new GetMetricsRequest
        {
            BotId = "b",
            Name = "A",
            From = DateTime.UtcNow.AddHours(-1),
            To = DateTime.UtcNow
        }, CancellationToken.None)).Count.Should().Be(1);

        (await controller.GetMetricsForInterval(new GetMetricsForIntervalsRequest
        {
            BotId = "b",
            Name = "A",
            From = DateTime.UtcNow.AddHours(-1),
            To = DateTime.UtcNow,
            Interval = 60
        }, CancellationToken.None)).Count.Should().Be(2);

        (await controller.GetMetricNames(CancellationToken.None)).Should().ContainSingle("A");
    }
}
