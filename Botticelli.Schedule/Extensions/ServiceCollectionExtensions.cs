using Botticelli.Scheduler.Settings;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Scheduler.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a scheduler server (Hangfire)
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="config">Configuration</param>
    /// <returns></returns>
    public static IServiceCollection AddHangfireScheduler(this IServiceCollection services, IConfiguration config)
    {
        var settings = new SchedulerSettings();
        config.GetSection(nameof(SchedulerSettings)).Bind(settings);

        return services.AddHangfire(cfg => cfg
                                           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                           .UseSimpleAssemblyNameTypeSerializer()
                                           .UseRecommendedSerializerSettings()
                                           .UseLiteDbStorage(settings.JobStorageConnectionString))
                       .AddHangfireServer();
    }
}