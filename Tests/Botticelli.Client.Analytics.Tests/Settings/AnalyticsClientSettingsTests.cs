using Botticelli.Client.Analytics.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Client.Analytics.Tests.Settings;

[TestFixture]
public class AnalyticsClientSettingsTests
{
    [Test]
    public void Section_Should_BeAnalyticsClient()
    {
        AnalyticsClientSettings.Section.Should().Be("AnalyticsClient");
    }

    [Test]
    public void Defaults_Should_EnableSsl()
    {
        var settings = new AnalyticsClientSettings();

        settings.UseSsl.Should().BeTrue();
        settings.TargetUrl.Should().BeNull();
    }

    [Test]
    public void Properties_Should_BeAssignable()
    {
        var settings = new AnalyticsClientSettings
        {
            TargetUrl = "http://localhost:5050",
            UseSsl = false
        };

        settings.TargetUrl.Should().Be("http://localhost:5050");
        settings.UseSsl.Should().BeFalse();
    }
}
