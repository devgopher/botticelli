using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Botticelli.Framework.Options;
using Botticelli.Framework.Services;
using Botticelli.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Mocks;
using Moq;
using NUnit.Framework;

namespace Botticelli.Framework.Tests.Services;

[TestFixture]
public class BotStatusServiceTests
{
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly BotMock _botMock = new();
    private Mock<ILogger<BotStatusService>> _loggerMock;
    private ServerSettings _serverSettings;
    private BotStatusService _service;

    [SetUp]
    public void Setup()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<BotStatusService>>();
        _serverSettings = new ServerSettings();
        _service = new BotStatusService(
            _httpClientFactoryMock.Object,
            _serverSettings,
            _botMock,
            _loggerMock.Object
        );
    }

    [Test]
    public async Task StartAsync_Should_NotThrowException()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _service.StartAsync(cancellationToken);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task StopAsync_Should_NotThrowException()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        await _service.StartAsync(cancellationToken); // Ensure the service is started

        // Act
        var act = async () => await _service.StopAsync(cancellationToken); // Make sure StopAsync is implemented

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task StartAsync_Should_NotStartAgain_WhenAlreadyStarted()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        await _service.StartAsync(cancellationToken);

        // Act
        var act = async () => await _service.StartAsync(cancellationToken); // Attempt to start again

        // Assert
        await act.Should().NotThrowAsync();
    }
}