using System.Net;
using Botticelli.Server.FrontNew.Clients;
using Botticelli.Server.FrontNew.Handler;
using Botticelli.Server.FrontNew.Models;
using Botticelli.Server.FrontNew.Settings;
using Botticelli.Server.Models.Responses;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Botticelli.Server.FrontNew.Tests.Handler;

[TestFixture]
public class AuthDelegatingHandlerTests
{
    private WireMockServer _server = null!;
    private SessionClient _sessionClient = null!;

    [SetUp]
    public void SetUp()
    {
        _server = WireMockServer.Start();
        var monitor = new Mock<IOptionsMonitor<BackSettings>>();
        monitor.Setup(m => m.CurrentValue).Returns(new BackSettings
        {
            BackUrl = _server.Urls[0],
            AnalyticsUrl = _server.Urls[0]
        });
        _sessionClient = new SessionClient(monitor.Object);
    }

    [TearDown]
    public void TearDown() => _server.Stop();

    [Test]
    public async Task SendAsync_Should_AddBearerToken()
    {
        _server.Given(Request.Create().WithPath("/auth/GetToken").UsingPost())
               .RespondWith(Response.Create()
                                    .WithStatusCode(200)
                                    .WithHeader("Content-Type", "application/json")
                                    .WithBody(System.Text.Json.JsonSerializer.Serialize(new GetTokenResponse
                                    {
                                        IsSuccess = true,
                                        Token = "jwt-token"
                                    })));
        await _sessionClient.CreateSession("a@b.c", "p");

        string? authHeader = null;
        var inner = new CapturingHandler(async (request, _) =>
        {
            authHeader = request.Headers.Authorization?.ToString();
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var handler = new AuthDelegatingHandler(_sessionClient) { InnerHandler = inner };
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.local/resource");

        authHeader.Should().Be("Bearer jwt-token");
    }

    [Test]
    public async Task SendAsync_Should_Throw_WhenSessionMissing()
    {
        var handler = new AuthDelegatingHandler(_sessionClient)
        {
            InnerHandler = new CapturingHandler((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
        };
        using var client = new HttpClient(handler);

        var act = async () => await client.GetAsync("https://example.local/resource");
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    private sealed class CapturingHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => handler(request, cancellationToken);
    }
}
