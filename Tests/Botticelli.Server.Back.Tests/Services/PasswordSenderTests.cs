using Botticelli.Server.Back.Services.Auth;
using Botticelli.Server.Back.Settings;
using FluentAssertions;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using FluentEmail.MailKitSmtp;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Services;

[TestFixture]
public class PasswordSenderTests
{
    private static ServerSettings CreateSettings() =>
        new()
        {
            TokenLifetimeMin = 30,
            SmtpClientOptions = new SmtpClientOptions(),
            ServerEmail = "noreply@botticelli.local",
            ServerUrl = "https://localhost",
            SecureStorageConnection = "Data Source=test.db"
        };

    [Test]
    public async Task SendPassword_Should_SendEmail()
    {
        var sender = new Mock<ISender>();
        sender.Setup(s => s.SendAsync(It.IsAny<IFluentEmail>(), It.IsAny<CancellationToken?>()))
              .ReturnsAsync(new SendResponse());
        var service = new PasswordSender(sender.Object, CreateSettings(), NullLogger<PasswordSender>.Instance);

        await service.SendPassword("user@test.com", "secret", CancellationToken.None);

        sender.Verify(s => s.SendAsync(It.IsAny<IFluentEmail>(), It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Test]
    public async Task SendPassword_Should_Throw_WhenSendFails()
    {
        var sender = new Mock<ISender>();
        sender.Setup(s => s.SendAsync(It.IsAny<IFluentEmail>(), It.IsAny<CancellationToken?>()))
              .ReturnsAsync(new SendResponse { ErrorMessages = { "smtp down" } });
        var service = new PasswordSender(sender.Object, CreateSettings(), NullLogger<PasswordSender>.Instance);

        var act = async () => await service.SendPassword("user@test.com", "secret", CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*smtp down*");
    }

    [Test]
    public async Task SendPassword_Should_Skip_WhenCancelled()
    {
        var sender = new Mock<ISender>();
        var service = new PasswordSender(sender.Object, CreateSettings(), NullLogger<PasswordSender>.Instance);
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await service.SendPassword("user@test.com", "secret", cts.Token);

        sender.Verify(s => s.SendAsync(It.IsAny<IFluentEmail>(), It.IsAny<CancellationToken?>()), Times.Never);
    }
}
