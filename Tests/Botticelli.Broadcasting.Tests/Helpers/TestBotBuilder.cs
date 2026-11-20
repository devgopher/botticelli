using Botticelli.Framework;
using Botticelli.Framework.Builders;
using Botticelli.Framework.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Broadcasting.Tests.Helpers;

public sealed class TestBotBuilder : BotBuilder<TestBroadcastBot, TestBotBuilder>
{
    public TestBotBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public void RaiseMessageReceived(object sender, MessageReceivedBotEventArgs args)
        => MessageReceived?.Invoke(sender, args);

    protected override TestBroadcastBot? InnerBuild(IServiceProvider serviceProvider)
        => serviceProvider.GetService<TestBroadcastBot>();
}
