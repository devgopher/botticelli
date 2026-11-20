using System.Configuration;
using Botticelli.Client.Analytics.Extensions;
using Botticelli.Client.Analytics.Settings;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Client.Analytics.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddAnalyticsClient_WithSettingsInstance_Should_RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddAnalyticsClient(new AnalyticsClientSettings
        {
            TargetUrl = "http://localhost",
            UseSsl = false
        });

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<MetricsPublisher>().Should().NotBeNull();
        provider.GetRequiredService<MetricsProcessor>().Should().NotBeNull();
        provider.GetRequiredService<AnalyticsClientSettings>().TargetUrl.Should().Be("http://localhost");
    }

    [Test]
    public void AddAnalyticsClient_WithBuilderAction_Should_RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddAnalyticsClient(b => b.Set(s => s.TargetUrl = "http://from-builder"));

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<AnalyticsClientSettings>().TargetUrl.Should().Be("http://from-builder");
    }

    [Test]
    public void AddAnalyticsClient_WithConfiguration_Should_BindSectionByTypeName()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory)
            .AddJsonFile("TestData/analytics.settings.json")
            .Build();

        services.AddAnalyticsClient(configuration);

        using var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<AnalyticsClientSettings>();
        settings.TargetUrl.Should().Be("http://from-config");
        settings.UseSsl.Should().BeFalse();
    }

    [Test]
    public void AddAnalyticsClient_WithMissingConfigurationSection_Should_Throw()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        var act = () => services.AddAnalyticsClient(configuration);

        act.Should().Throw<ConfigurationErrorsException>()
           .WithMessage("*AnalyticsClientSettings*");
    }

    [Test]
    public void AddAnalyticsClient_GenericBuilder_Should_RegisterTypedSettings()
    {
        var services = new ServiceCollection();

        services.AddAnalyticsClient<AnalyticsClientSettings>(b =>
            b.Set(s =>
            {
                s.TargetUrl = "http://typed";
                s.UseSsl = true;
            }));

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<AnalyticsClientSettings>().TargetUrl.Should().Be("http://typed");
        provider.GetRequiredService<MetricsProcessor>().Should().NotBeNull();
    }
}
