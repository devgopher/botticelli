using System.Security.Cryptography.X509Certificates;
using Botticelli.Framework.Options;
using Botticelli.Framework.Viber.Options;
using Botticelli.Framework.Viber.WebHook;
using Botticelli.Interfaces;
using Botticelli.Serialization;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Viber.Api;
using Viber.Api.Settings;

namespace Botticelli.Framework.Viber.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    ///     Adds a Telegram bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddViberBot(this IServiceCollection services,
                                                 BotOptionsBuilder<ViberBotSettings> optionsBuilder)
    {
        var settings = optionsBuilder.Build();
        services.AddSingleton(new ViberApiSettings
        {
            HookUrl = "https://botticellibots.com//",
            RemoteUrl = "https://chatapi.viber.com/pa/",
            ViberToken = settings.ViberToken
        });

        services.AddHttpClient<ViberService>();

        return services.AddSingleton<IViberService, ViberService>()
                       .AddSingleton<IBot<ViberBot>, ViberBot>()
                       .AddTransient<ISerializerFactory, JsonSerializerFactory>()
                       .AddTransient<INotificator<ViberBot>, Notificator<ViberBot>>()
                       .AddTransient<IMapper, Mapper>()
                       .AddTransient<WebHookHandler>()
                       .AddCertAuthentication(settings)
                       .AddSingleton(settings);
    }
    
    public static IServiceCollection AddCertAuthentication(this IServiceCollection services, ViberBotSettings settings)
    {
        services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(opt =>
                {
                    opt.AllowedCertificateTypes = CertificateTypes.All;
                    opt.CustomTrustStore.Add(new X509Certificate2(settings.CertificatePath, settings.CertificatePassword));
                });

        return services;
    }
}