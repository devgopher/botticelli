using System.Text;
using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Back.Settings;
using FluentAssertions;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using FluentEmail.MailKitSmtp;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Services;

[TestFixture]
public class ConfirmationServiceTests
{
    private static ServerSettings CreateSettings() =>
        new()
        {
            TokenLifetimeMin = 30,
            SmtpClientOptions = new SmtpClientOptions(),
            ServerEmail = "noreply@botticelli.local",
            ServerUrl = "https://localhost:5001",
            SecureStorageConnection = "Data Source=test.db"
        };

    private static Mock<UserManager<IdentityUser<string>>> CreateUserManager()
    {
        var store = new Mock<IUserStore<IdentityUser<string>>>();
        return new Mock<UserManager<IdentityUser<string>>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    [Test]
    public async Task SendConfirmationCode_Should_SendEmail()
    {
        var fluent = new Mock<ISender>();
        fluent.Setup(s => s.SendAsync(It.IsAny<IFluentEmail>(), It.IsAny<CancellationToken?>()))
              .ReturnsAsync(new SendResponse());
        var userManager = CreateUserManager();
        userManager.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser<string>>()))
                   .ReturnsAsync("raw-token");
        var service = new ConfirmationService(fluent.Object, userManager.Object, CreateSettings());
        var user = new IdentityUser<string> { Email = "a@b.c", UserName = "a" };

        await service.SendConfirmationCode(user, CancellationToken.None);

        fluent.Verify(s => s.SendAsync(It.IsAny<IFluentEmail>(), It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public async Task SendConfirmationCode_Should_Throw_WhenSendFails()
    {
        var fluent = new Mock<ISender>();
        fluent.Setup(s => s.SendAsync(It.IsAny<IFluentEmail>(), It.IsAny<CancellationToken?>()))
              .ReturnsAsync(new SendResponse { ErrorMessages = { "fail" } });
        var userManager = CreateUserManager();
        userManager.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser<string>>()))
                   .ReturnsAsync("raw-token");
        var service = new ConfirmationService(fluent.Object, userManager.Object, CreateSettings());

        var act = async () => await service.SendConfirmationCode(
            new IdentityUser<string> { Email = "a@b.c" }, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task ConfirmCodeAsync_Should_DecodeTokenAndConfirm()
    {
        var fluent = new Mock<ISender>();
        var userManager = CreateUserManager();
        userManager.Setup(m => m.ConfirmEmailAsync(It.IsAny<IdentityUser<string>>(), "raw-token"))
                   .ReturnsAsync(IdentityResult.Success);
        var service = new ConfirmationService(fluent.Object, userManager.Object, CreateSettings());
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes("raw-token"));

        var ok = await service.ConfirmCodeAsync(encoded, new IdentityUser<string> { Email = "a@b.c" }, CancellationToken.None);

        ok.Should().BeTrue();
    }
}
