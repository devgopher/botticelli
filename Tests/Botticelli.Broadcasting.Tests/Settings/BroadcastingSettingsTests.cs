using Botticelli.Broadcasting.Exceptions;
using Botticelli.Broadcasting.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Broadcasting.Tests.Settings;

[TestFixture]
public class BroadcastingSettingsTests
{
    [Test]
    public void Settings_Should_ExposeDefaultsAndSection()
    {
        var settings = new BroadcastingSettings
        {
            BotId = "bot-1",
            ServerUri = "http://localhost",
            ConnectionString = "broadcast.db"
        };

        BroadcastingSettings.Section.Should().Be("Broadcasting");
        settings.HowOld.Should().Be(TimeSpan.FromSeconds(60));
        settings.BotId.Should().Be("bot-1");
        settings.ServerUri.Should().Be("http://localhost");
        settings.ConnectionString.Should().Be("broadcast.db");
    }

    [Test]
    public void BroadcastingException_Should_StoreMessageAndInner()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new BroadcastingException("outer", inner);

        ex.Message.Should().Be("outer");
        ex.InnerException.Should().BeSameAs(inner);

        new BroadcastingException("only-message").InnerException.Should().BeNull();
    }
}
