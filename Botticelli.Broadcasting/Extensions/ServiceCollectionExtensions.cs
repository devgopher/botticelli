using System.Configuration;
using Botticelli.Broadcasting.Dal;
using Botticelli.Broadcasting.Settings;
using Botticelli.Framework;
using Botticelli.Framework.Builders;
using Botticelli.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Broadcasting.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds broadcasting to a bot
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <typeparam name="TBotBuilder"></typeparam>
    /// <param name="config"></param>
    /// <returns></returns>
    public static BotBuilder<TBot, TBotBuilder> AddBroadcasting<TBot, TBotBuilder>(this BotBuilder<TBot, TBotBuilder> botBuilder, IConfiguration config)
            where TBot : BaseBot, IBot<TBot> 
            where TBotBuilder : BotBuilder<TBot, TBotBuilder>
    {
        var settings = config.Get<BroadcastingSettings>();

        if (settings == null) throw new ConfigurationErrorsException("Broadcasting settings are missing!");

        // TODO: add broadcasting service (IHostedService which uses injected IServiceProvider and creates a new IBot??)
        botBuilder.Services.AddHostedService<Broadcaster<TBot>>();
        
        botBuilder.AddOnMessageReceived(async (sender, args) =>
        {
            var context = botBuilder.Services.BuildServiceProvider().GetRequiredService<BroadcastingContext>();
            
            var disabledChats = context.Chats.Where(c => !c.IsActive && args.Message.ChatIds.Contains(c.ChatId)).AsQueryable();
            var nonExistingChats = context.Chats.Where(c => !args.Message.ChatIds.Contains(c.ChatId)).ToArray();

            await context.Chats.AddRangeAsync(nonExistingChats);
            await context.SaveChangesAsync();
            
            await disabledChats.ExecuteUpdateAsync(c => c.SetProperty(chat => chat.IsActive, true));
            await context.SaveChangesAsync();
        });
        
        return botBuilder;
    }
}