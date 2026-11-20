using System.Security.Claims;
using Botticelli.Server.Back.Controllers;
using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Back.Settings;
using Botticelli.Server.Data.Entities.Auth;
using FluentAssertions;
using FluentEmail.MailKitSmtp;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Controllers;

[TestFixture]
public class UserControllerTests
{
    private Mock<IUserService> _users = null!;
    private Mock<IPasswordSender> _passwordSender = null!;
    private ServerSettings _settings = null!;
    private UserController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _users = new Mock<IUserService>();
        _passwordSender = new Mock<IPasswordSender>();
        _settings = new ServerSettings
        {
            TokenLifetimeMin = 30,
            SmtpClientOptions = new SmtpClientOptions(),
            ServerEmail = "noreply@test.com",
            ServerUrl = "https://localhost",
            SecureStorageConnection = "cs",
            PasswordMinLength = 8,
            PasswordMaxLength = 12,
            PasswordDeliveryMode = PasswordDeliveryMode.RegistrationWindow
        };
        _controller = new UserController(_users.Object, new Mapper(), _passwordSender.Object, _settings)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity([
                        new Claim("applicationUserName", "current")
                    ]))
                }
            }
        };
    }

    [Test]
    public async Task HasUsersAsync_Should_ReturnOk()
    {
        _users.Setup(u => u.HasUsers(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _controller.HasUsersAsync(CancellationToken.None);

        result.Value.Should().Be(true);
    }

    [Test]
    public void GetRegistrationSettings_Should_ReturnMode()
    {
        var result = _controller.GetRegistrationSettings();

        result.Result.Should().BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<RegistrationSettingsResponse>()
              .Which.PasswordDeliveryMode.Should().Be(PasswordDeliveryMode.RegistrationWindow);
    }

    [Test]
    public async Task AddDefaultUserAsync_Should_ReturnPassword_InRegistrationWindowMode()
    {
        _users.Setup(u => u.CheckAndAddAsync(It.IsAny<UserAddRequest>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);

        var result = await _controller.AddDefaultUserAsync(new DefaultUserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c"
        }, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<DefaultUserAddResponse>()
              .Which.Password.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public async Task AddDefaultUserAsync_Should_SendEmail_WhenEmailMode()
    {
        _settings.PasswordDeliveryMode = PasswordDeliveryMode.Email;
        _users.Setup(u => u.CheckAndAddAsync(It.IsAny<UserAddRequest>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);

        var result = await _controller.AddDefaultUserAsync(new DefaultUserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c"
        }, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
        _passwordSender.Verify(p => p.SendPassword("a@b.c", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AddDefaultUserAsync_Should_ReturnBadRequest_WhenExists()
    {
        _users.Setup(u => u.CheckAndAddAsync(It.IsAny<UserAddRequest>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(false);

        var result = await _controller.AddDefaultUserAsync(new DefaultUserAddRequest
        {
            UserName = "admin",
            Email = "a@b.c"
        }, CancellationToken.None);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task RegeneratePasswordAsync_Should_ReturnOk()
    {
        var result = await _controller.RegeneratePasswordAsync(new RegeneratePasswordRequest
        {
            UserName = "admin",
            Email = "a@b.c"
        }, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _users.Verify(u => u.UpdateAsync(It.IsAny<UserUpdateRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        _passwordSender.Verify(p => p.SendPassword("a@b.c", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AddUserAsync_Should_ReturnOk()
    {
        var result = await _controller.AddUserAsync(new UserAddRequest
        {
            UserName = "u",
            Email = "e",
            Password = "p"
        }, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }

    [Test]
    public async Task GetCurrentUserAsync_Should_ReturnUser()
    {
        _users.Setup(u => u.GetAsync(It.Is<UserGetRequest>(r => r.UserName == "current"), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new UserGetResponse { UserName = "current", Email = "c@x.com" });

        var result = await _controller.GetCurrentUserAsync(CancellationToken.None);

        result.Value!.UserName.Should().Be("current");
    }

    [Test]
    public async Task UpdateCurrentUserAsync_Should_ForceCurrentUserName()
    {
        var result = await _controller.UpdateCurrentUserAsync(new UserUpdateRequest
        {
            UserName = "other",
            Email = "e",
            Password = "p"
        }, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _users.Verify(u => u.UpdateAsync(It.Is<UserUpdateRequest>(r => r.UserName == "current"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DeleteUserAsync_Should_BlockSelfDelete()
    {
        var result = await _controller.DeleteUserAsync(new UserDeleteRequest { UserName = "current" }, CancellationToken.None);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task DeleteUserAsync_Should_DeleteOtherUser()
    {
        var result = await _controller.DeleteUserAsync(new UserDeleteRequest { UserName = "other" }, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _users.Verify(u => u.DeleteAsync(It.IsAny<UserDeleteRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ConfirmEmailAsync_Should_ReturnOk()
    {
        var result = await _controller.ConfirmEmailAsync(new ConfirmEmailRequest
        {
            Email = "a@b.c",
            Token = "tok"
        }, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _users.Verify(u => u.ConfirmCodeAsync("a@b.c", "tok", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetUserAsync_Should_ReturnBadRequest_OnError()
    {
        _users.Setup(u => u.GetAsync(It.IsAny<UserGetRequest>(), It.IsAny<CancellationToken>()))
              .ThrowsAsync(new InvalidOperationException("missing"));

        var result = await _controller.GetUserAsync(new UserGetRequest { UserName = "x" }, CancellationToken.None);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}
