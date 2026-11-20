using Botticelli.Schedule.Quartz;
using Botticelli.Schedule.Quartz.Extensions;
using Botticelli.Scheduler.Interfaces;
using Botticelli.Scheduler.Settings;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Quartz;

namespace Botticelli.Scheduler.Quartz.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddQuartzScheduler_Should_RegisterJobManagerAndHostedService()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{nameof(SchedulerSettings)}:{nameof(SchedulerSettings.JobStorageConnectionString)}"] = "mem"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddQuartzScheduler(config);

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IJobManager>().Should().BeOfType<QuartzJobManager>();
        provider.GetService<ISchedulerFactory>().Should().NotBeNull();
    }
}
