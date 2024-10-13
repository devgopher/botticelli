using Botticelli.Bot.Data.Settings;
using Botticelli.Client.Analytics.Settings;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.Options;
using Botticelli.Framework.Vk.Messages.API.Markups;
using Botticelli.Framework.Vk.Messages.Builders;
using Botticelli.Framework.Vk.Messages.Layout;
using Botticelli.Framework.Vk.Messages.Options;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Vk.Messages.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly BotSettingsBuilder<VkBotSettings> SettingsBuilder = new();
    private static readonly ServerSettingsBuilder<ServerSettings> ServerSettingsBuilder = new();
    private static readonly AnalyticsClientSettingsBuilder<AnalyticsClientSettings> AnalyticsClientOptionsBuilder = new();
    private static readonly DataAccessSettingsBuilder<DataAccessSettings> DataAccessSettingsBuilder = new();
    
    
    /// <summary>
    ///     Adds a Vk bot
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsBuilderFunc"></param>
    /// <param name="analyticsOptionsBuilderFunc"></param>
    /// <param name="serverSettingsBuilderFunc"></param>
    /// <param name="dataAccessSettingsBuilderFunc"></param>
    /// <returns></returns>
    public static IServiceCollection AddVkBot(this IServiceCollection services,
                                              Action<BotSettingsBuilder<VkBotSettings>> optionsBuilderFunc,
                                              Action<AnalyticsClientSettingsBuilder<AnalyticsClientSettings>> analyticsOptionsBuilderFunc,
                                              Action<ServerSettingsBuilder<ServerSettings>> serverSettingsBuilderFunc,
                                              Action<DataAccessSettingsBuilder<DataAccessSettings>> dataAccessSettingsBuilderFunc)
    {
        optionsBuilderFunc(SettingsBuilder);
        serverSettingsBuilderFunc(ServerSettingsBuilder);
        analyticsOptionsBuilderFunc(AnalyticsClientOptionsBuilder);
        dataAccessSettingsBuilderFunc(DataAccessSettingsBuilder);
        
        var clientBuilder = LongPollMessagesProviderBuilder.Instance(SettingsBuilder);
        
        var botBuilder = VkBotBuilder.Instance(services,
                                                     ServerSettingsBuilder,
                                                     SettingsBuilder, 
                                                     DataAccessSettingsBuilder,
                                                     AnalyticsClientOptionsBuilder)
                                           .AddClient(clientBuilder);
        var bot = botBuilder.Build();
        return services.AddSingleton<IBot<VkBot>>(bot)
                       .AddSingleton<IBot>(bot);
    }
    public static IServiceCollection AddVkLayoutsSupport(this IServiceCollection services) =>
            services.AddScoped<ILayoutParser, JsonLayoutParser>()
                    .AddScoped<ILayoutSupplier<VkKeyboardMarkup>, VkLayoutSupplier>()
                    .AddScoped<ILayoutLoader<VkKeyboardMarkup>,
                            LayoutLoader<ILayoutParser, ILayoutSupplier<VkKeyboardMarkup>, VkKeyboardMarkup>>();
}