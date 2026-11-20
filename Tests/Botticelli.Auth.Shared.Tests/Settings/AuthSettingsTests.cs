using Botticelli.Auth.Shared.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Auth.Shared.Tests.Settings;

[TestFixture]
public class AuthSettingsTests
{
    [Test]
    public void AuthSettings_Should_ExposeAssignableProperties()
    {
        var settings = new AuthSettings
        {
            ConnectionString = "Host=localhost;Database=auth",
            TokenLifetimeMin = 120
        };

        settings.ConnectionString.Should().Be("Host=localhost;Database=auth");
        settings.TokenLifetimeMin.Should().Be(120);
    }

    [Test]
    public void AuthSettings_Should_UseDefaultTokenLifetime()
    {
        var settings = new AuthSettings
        {
            ConnectionString = null
        };

        settings.ConnectionString.Should().BeNull();
        settings.TokenLifetimeMin.Should().Be(60);
    }
}
