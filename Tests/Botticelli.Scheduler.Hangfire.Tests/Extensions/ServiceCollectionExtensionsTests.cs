using Botticelli.Scheduler.Hangfire.Extensions;
using Botticelli.Scheduler.Settings;
using FluentAssertions;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Scheduler.Hangfire.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddHangfireScheduler_Should_RegisterHangfireServices()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{nameof(SchedulerSettings)}:{nameof(SchedulerSettings.JobStorageConnectionString)}"] = "mem"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddHangfireScheduler(config);

        using var provider = services.BuildServiceProvider();
        provider.GetService<IGlobalConfiguration>().Should().NotBeNull();
        provider.GetRequiredService<IServiceCollection>().Should().BeSameAs(services);
    }
}
