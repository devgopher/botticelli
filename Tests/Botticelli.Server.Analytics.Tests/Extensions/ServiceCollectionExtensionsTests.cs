using Botticelli.Server.Analytics.Cache;
using Botticelli.Server.Analytics.Extensions;
using Botticelli.Server.Analytics.Services;
using Botticelli.Server.Analytics.Settings;
using Botticelli.Server.Analytics.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Server.Analytics.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddAnalytics_Should_RegisterCoreServices()
    {
        var (context, _) = TestMetricsContextFactory.Create();
        var services = new ServiceCollection();
        services.AddSingleton(context);
        services.AddAnalytics(new AnalyticsServerSettings
        {
            ConnectionString = "Host=localhost;Database=test",
            MaxCacheSize = 100
        });

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IMetricsInputService>().Should().BeOfType<MetricsInputService>();
        provider.GetRequiredService<IMetricsOutputService>().Should().BeOfType<MetricsOutputService>();
        provider.GetRequiredService<ICacheAccessor>().Should().BeOfType<LocalCacheAccessor>();
        provider.GetRequiredService<MetricsReaderWriter>().Should().NotBeNull();
    }
}
