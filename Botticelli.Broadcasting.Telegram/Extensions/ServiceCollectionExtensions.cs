using Botticelli.Broadcasting.Extensions;
using Botticelli.Framework.Builders;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Builders;
using Microsoft.Extensions.Configuration;

namespace Botticelli.Broadcasting.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds broadcasting to a bot
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="botBuilder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static TelegramBotBuilder<TBot> AddBroadcasting<TBot>(this TelegramBotBuilder<TBot> botBuilder,
                                                                 IConfiguration config)
            where TBot : TelegramBot
    {
        var builder = botBuilder as BotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>; 
        builder!.AddBroadcasting(config);

        return botBuilder;
    }

    /// <summary>
    ///     Adds broadcasting to a bot
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <typeparam name="TBotBuilder"></typeparam>
    /// <param name="botBuilder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static TelegramBotBuilder<TBot, TBotBuilder> AddTelegramBroadcasting<TBot, TBotBuilder>(this TelegramBotBuilder<TBot, TBotBuilder> botBuilder,
                                                                                           IConfiguration config)
            where TBot : TelegramBot
            where TBotBuilder : BotBuilder<TBot, TBotBuilder>
    {
        var builder = botBuilder as BotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>; 
        builder!.AddBroadcasting(config);

        return botBuilder;
    }
}