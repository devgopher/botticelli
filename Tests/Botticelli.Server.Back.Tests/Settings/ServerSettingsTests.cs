using Botticelli.Server.Back.Settings;
using Botticelli.Server.Data.Entities.Auth;
using FluentAssertions;
using FluentEmail.MailKitSmtp;
using NUnit.Framework;

namespace Botticelli.Server.Back.Tests.Settings;

[TestFixture]
public class ServerSettingsTests
{
    [Test]
    public void ServerSettings_Should_ExposeDefaultsAndAssignableValues()
    {
        var settings = new ServerSettings
        {
            TokenLifetimeMin = 45,
            SmtpClientOptions = new SmtpClientOptions { Server = "smtp" },
            ServerEmail = "noreply@test.com",
            HttpsPort = 443,
            ServerUrl = "https://localhost",
            AnalyticsUrl = "https://analytics",
            SecureStorageConnection = "Data Source=db.sqlite",
            UseSsl = true,
            PasswordMinLength = 10,
            PasswordMaxLength = 16,
            PasswordDeliveryMode = PasswordDeliveryMode.RegistrationWindow
        };

        settings.TokenLifetimeMin.Should().Be(45);
        settings.SmtpClientOptions.Server.Should().Be("smtp");
        settings.ServerEmail.Should().Be("noreply@test.com");
        settings.HttpsPort.Should().Be(443);
        settings.ServerUrl.Should().Be("https://localhost");
        settings.AnalyticsUrl.Should().Be("https://analytics");
        settings.SecureStorageConnection.Should().Be("Data Source=db.sqlite");
        settings.UseSsl.Should().BeTrue();
        settings.PasswordMinLength.Should().Be(10);
        settings.PasswordMaxLength.Should().Be(16);
        settings.PasswordDeliveryMode.Should().Be(PasswordDeliveryMode.RegistrationWindow);
    }

    [Test]
    public void ServerSettings_Should_UseDefaultPasswordLengthsAndMode()
    {
        var settings = new ServerSettings
        {
            SmtpClientOptions = new SmtpClientOptions(),
            ServerEmail = "a@b.c",
            ServerUrl = "https://x",
            SecureStorageConnection = "cs"
        };

        settings.PasswordMinLength.Should().Be(8);
        settings.PasswordMaxLength.Should().Be(12);
        settings.PasswordDeliveryMode.Should().Be(PasswordDeliveryMode.Email);
    }
}
