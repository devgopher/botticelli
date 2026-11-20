using Botticelli.Client.Analytics.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Client.Analytics.Tests.Settings;

[TestFixture]
public class AnalyticsClientSettingsBuilderTests
{
    [Test]
    public void Set_WithInstance_Should_ReplaceSettings()
    {
        var builder = new AnalyticsClientSettingsBuilder<AnalyticsClientSettings>();
        var settings = new AnalyticsClientSettings { TargetUrl = "http://a", UseSsl = false };

        builder.Set(settings);

        builder.Build().Should().BeSameAs(settings);
    }

    [Test]
    public void Set_WithAction_Should_MutateAndReturnBuilder()
    {
        var builder = new AnalyticsClientSettingsBuilder<AnalyticsClientSettings>();

        var returned = builder.Set(s =>
        {
            s.TargetUrl = "http://b";
            s.UseSsl = true;
        });

        returned.Should().BeSameAs(builder);
        var built = builder.Build();
        built.TargetUrl.Should().Be("http://b");
        built.UseSsl.Should().BeTrue();
    }

    [Test]
    public void Build_Should_ReturnDefaultInstance_WhenNothingConfigured()
    {
        var builder = new AnalyticsClientSettingsBuilder<AnalyticsClientSettings>();
        var built = builder.Build();

        built.Should().NotBeNull();
        built.UseSsl.Should().BeTrue();
    }
}
