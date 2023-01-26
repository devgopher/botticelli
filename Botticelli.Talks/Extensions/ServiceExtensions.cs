﻿using Botticelli.Talks.OpenTts;
using Botticelli.Talks.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Talks.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddOpenTtsTalks(this IServiceCollection services,
                                                  IConfiguration config)
        {
            var settings = config.Get<TtsSettings>();

            services.ConfigureOptions<TtsSettings>();
            services.AddHttpClient<OpenTtsSpeaker>();
            services.AddScoped<ISpeaker, OpenTtsSpeaker>();

            return services;
        }
    }
}
