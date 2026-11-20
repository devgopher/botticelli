using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Broadcasting.Telegram.Tests.Helpers;

public sealed class ConcreteTelegramBotBuilder : TelegramBotBuilder<TelegramBot>
{
    public ConcreteTelegramBotBuilder(IServiceCollection services) : base(true)
    {
        AddServices(services);
    }
}

public sealed class ConcreteTelegramBotBuilder2 : TelegramBotBuilder<TelegramBot, ConcreteTelegramBotBuilder2>
{
    public ConcreteTelegramBotBuilder2(IServiceCollection services) : base(true)
    {
        AddServices(services);
    }
}
