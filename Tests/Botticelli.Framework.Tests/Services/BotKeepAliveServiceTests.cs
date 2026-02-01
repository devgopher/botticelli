using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Botticelli.Framework.Options;
using Botticelli.Framework.Services;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Botticelli.Framework.Tests.Services;

[TestFixture]
public class BotKeepAliveServiceTests : IDisposable
{
    private WireMockServer _server;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<IBot> _botMock;
    private Mock<ILogger<BotKeepAliveService>> _loggerMock;
    private ServerSettings _serverSettings;
    private const int Delay = 3000;

    [SetUp]
    public void Setup()
    {
        // Start WireMock server
        _server = WireMockServer.Start();

        // Mock IHttpClientFactory
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpClient = new HttpClient { BaseAddress = new Uri(_server.Url) };
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Mock IBot
        _botMock = new Mock<IBot>();

        // Mock logger
        _loggerMock = new Mock<ILogger<BotKeepAliveService>>();

        // Setup the server settings
        _serverSettings = new ServerSettings
        {
            ServerUri = _server.Url
        };
    }

    [Test]
    public async Task StartAsync_ShouldSendKeepAliveMessage_WhenCalled()
    {
        // Arrange: Set up the WireMock server to respond with a KeepAliveNotificationResponse
        var expectedResponse = new KeepAliveNotificationResponse { IsSuccess = true, BotId = "12345" };

        _server.Given(
                Request.Create()
                    .WithPath("/bot/client/keepalive")
                    .UsingPost())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(expectedResponse));

        var service = new BotKeepAliveService(
            _httpClientFactoryMock.Object,
            _serverSettings,
            _botMock.Object,
            _loggerMock.Object);
        
        // Act: Start the service
        await service.StartAsync(CancellationToken.None);
        
        // Allow time for the periodic task to execute
        await Task.Delay(Delay); // Wait long enough for the retry mechanism

        // Assert: Verify that a message was logged
        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => true),
                It.Is<It.IsAnyType>((@object, @type) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task StartAsync_ShouldLogError_WhenKeepAliveFails()
    {
        // Arrange: Set up the WireMock server to respond with an error
        _server.Given(
                Request.Create()
                    .WithPath("/bot/client/keepalive")
                    .UsingPost())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.InternalServerError));

        var service = new BotKeepAliveService(
            _httpClientFactoryMock.Object,
            _serverSettings,
            _botMock.Object,
            _loggerMock.Object);
        
        // Act: Start the service
        await service.StartAsync(CancellationToken.None);

        // Allow some time for processing
        await Task.Delay(Delay); // Wait long enough for retries

        // Assert that the error handling was triggered
        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => true),
                It.Is<It.IsAnyType>((@object, @type) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce);
    }

    public void Dispose()
    {
        _server.Stop();
        _server.Dispose();
    }
}