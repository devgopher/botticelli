using System.Configuration;
using Botticelli.Broadcasting.Dal;
using Botticelli.Broadcasting.Dal.Models;
using Botticelli.Broadcasting.Settings;
using Botticelli.Framework;
using Botticelli.Framework.Builders;
using Botticelli.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Botticelli.Broadcasting.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds broadcasting to a bot
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <typeparam name="TBotBuilder"></typeparam>
    /// <param name="botBuilder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static BotBuilder<TBot, TBotBuilder> AddBroadcasting<TBot, TBotBuilder>(this BotBuilder<TBot, TBotBuilder> botBuilder, 
                                                                                   IConfiguration config)
            where TBot : BaseBot, IBot<TBot> 
            where TBotBuilder : BotBuilder<TBot, TBotBuilder>
    {
        var settings = config.GetSection(BroadcastingSettings.Section).Get<BroadcastingSettings>();

        if (settings == null) throw new ConfigurationErrorsException("Broadcasting settings are missing!");
        
        botBuilder.Services
            .AddDbContext<BroadcastingContext>(opt => opt.UseSqlite($"Data source={settings.ConnectionString}"));

        botBuilder.Services.AddHostedService<BroadcastReceiver<TBot>>(sp =>
            new BroadcastReceiver<TBot>(sp.GetRequiredService<IBot>(),
                sp.GetRequiredService<BroadcastingContext>(),
                settings,
                sp.GetRequiredService<ILogger<BroadcastReceiver<TBot>>>()));
        
        ApplyMigrations(botBuilder.Services);
        
        return botBuilder.AddOnMessageReceived((_, args) =>
        {
            var context = botBuilder.Services.BuildServiceProvider().GetRequiredService<BroadcastingContext>();

            var disabledChats = context.Chats.Where(c => !c.IsActive && args.Message.ChatIds.Contains(c.ChatId))
                .AsQueryable();
            var nonExistingChats = args.Message.ChatIds.Where(c => context.Chats.All(cc => cc.ChatId != c))
                .Select(c  => new Chat
                {
                    ChatId = c,
                    IsActive = true
                })
                .ToList();

            context.Chats.AddRange(nonExistingChats);
            disabledChats.ExecuteUpdate(c => c.SetProperty(chat => chat.IsActive, true));
            
            context.SaveChanges();
        });
    }

    private static void ApplyMigrations(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        var context = sp.GetRequiredService<BroadcastingContext>();

        if (context.Database.EnsureCreated())
            context.Database.Migrate();
    }
}