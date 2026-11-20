using System.Net;
using System.Text.Json;
using Botticelli.Server.Data.Entities.Auth;
using Botticelli.Server.FrontNew.Clients;
using Botticelli.Server.FrontNew.Settings;
using Botticelli.Server.Models.Responses;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Botticelli.Server.FrontNew.Tests.Clients;

[TestFixture]
public class SessionClientTests
{
    private WireMockServer _server = null!;
    private SessionClient _client = null!;

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
        _client = new SessionClient(monitor.Object);
    }

    [TearDown]
    public void TearDown() => _server.Stop();

    [Test]
    public async Task GetRegistrationSettingsAsync_Should_ReturnSettings()
    {
        _server.Given(Request.Create().WithPath("/user/GetRegistrationSettings").UsingGet())
               .RespondWith(Response.Create()
                                    .WithStatusCode(200)
                                    .WithHeader("Content-Type", "application/json")
                                    .WithBody(JsonSerializer.Serialize(new RegistrationSettingsResponse
                                    {
                                        PasswordDeliveryMode = PasswordDeliveryMode.RegistrationWindow
                                    })));

        var settings = await _client.GetRegistrationSettingsAsync();
        settings.PasswordDeliveryMode.Should().Be(PasswordDeliveryMode.RegistrationWindow);
    }

    [Test]
    public async Task RegisterDefaultUser_Should_ReturnPassword_OnSuccess()
    {
        _server.Given(Request.Create().WithPath("/user/AddDefaultUser").UsingPost())
               .RespondWith(Response.Create()
                                    .WithStatusCode(200)
                                    .WithHeader("Content-Type", "application/json")
                                    .WithBody(JsonSerializer.Serialize(new DefaultUserAddResponse { Password = "tmp" })));

        var (error, password) = await _client.RegisterDefaultUser("a@b.c");

        error.Code.Should().Be(0);
        password.Should().Be("tmp");
    }

    [Test]
    public async Task RegisterDefaultUser_Should_ReturnError_OnFailure()
    {
        _server.Given(Request.Create().WithPath("/user/AddDefaultUser").UsingPost())
               .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.BadRequest).WithStatusCode(400));

        var (error, password) = await _client.RegisterDefaultUser("a@b.c");

        error.Code.Should().Be(1);
        password.Should().BeNull();
    }

    [Test]
    public async Task RegisterUser_Should_ReturnSuccessAndError()
    {
        _server.Given(Request.Create().WithPath("/user").UsingPost())
               .RespondWith(Response.Create().WithStatusCode(200));

        (await _client.RegisterUser("a@b.c", "p")).Code.Should().Be(0);

        _server.Reset();
        _server.Given(Request.Create().WithPath("/user").UsingPost())
               .RespondWith(Response.Create().WithStatusCode(400));

        (await _client.RegisterUser("a@b.c", "p")).Code.Should().Be(1);
    }

    [Test]
    public async Task ResetPassword_Should_ReturnSuccessAndError()
    {
        _server.Given(Request.Create().WithPath("/user/RegeneratePassword").UsingPost())
               .RespondWith(Response.Create().WithStatusCode(200));
        (await _client.ResetPassword("a@b.c")).Code.Should().Be(0);

        _server.Reset();
        _server.Given(Request.Create().WithPath("/user/RegeneratePassword").UsingPost())
               .RespondWith(Response.Create().WithStatusCode(500));
        (await _client.ResetPassword("a@b.c")).Code.Should().Be(1);
    }

    [Test]
    public async Task HasUsersAsync_Should_ReturnBool()
    {
        _server.Given(Request.Create().WithPath("/user/HasUsers").UsingGet())
               .RespondWith(Response.Create()
                                    .WithStatusCode(200)
                                    .WithHeader("Content-Type", "application/json")
                                    .WithBody("true"));

        (await _client.HasUsersAsync()).Should().BeTrue();
    }

    [Test]
    public async Task CreateSession_Should_StoreSession_OnSuccess()
    {
        _server.Given(Request.Create().WithPath("/auth/GetToken").UsingPost())
               .RespondWith(Response.Create()
                                    .WithStatusCode(200)
                                    .WithHeader("Content-Type", "application/json")
                                    .WithBody(JsonSerializer.Serialize(new GetTokenResponse
                                    {
                                        IsSuccess = true,
                                        Token = "jwt"
                                    })));

        var (session, error) = await _client.CreateSession("a@b.c", "p");

        error.Code.Should().Be(0);
        session.Token.Should().Be("jwt");
        session.Login.Should().Be("a@b.c");
        _client.GetSession().Token.Should().Be("jwt");
    }

    [Test]
    public async Task CreateSession_Should_ReturnError_OnFailedLogin()
    {
        _server.Given(Request.Create().WithPath("/auth/GetToken").UsingPost())
               .RespondWith(Response.Create()
                                    .WithStatusCode(200)
                                    .WithHeader("Content-Type", "application/json")
                                    .WithBody(JsonSerializer.Serialize(new GetTokenResponse { IsSuccess = false })));

        var (session, error) = await _client.CreateSession("a@b.c", "bad");

        error.Code.Should().Be(1);
        session.Should().BeNull();
    }
}
