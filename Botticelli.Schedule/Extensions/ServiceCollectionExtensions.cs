﻿using Botticelli.Interfaces;
using Botticelli.Scheduler.Settings;
using Hangfire;
using Hangfire.LiteDB;
using Hangfire.MemoryStorage;
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
    public static IServiceCollection AddHangfireScheduler<TBot>(this IServiceCollection services, IConfiguration config) 
            where TBot : class, IBot<TBot>
    {
        var settings = new SchedulerSettings();
        config.GetSection(nameof(SchedulerSettings)).Bind(settings);

        return services.AddHangfire(cfg => cfg
                                           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                           .UseSimpleAssemblyNameTypeSerializer()
                                           .UseRecommendedSerializerSettings()
                                           .UseActivator(new ContainerJobActivator(services))
                                           .UseMemoryStorage())
                       .AddHangfireServer()
                       .AddSingleton(services);
    }
}