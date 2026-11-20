using System.Configuration;
using Botticelli.Broadcasting.Telegram.Extensions;
using Botticelli.Broadcasting.Telegram.Tests.Helpers;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Builders;
using Botticelli.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Botticelli.Broadcasting.Telegram.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    private static IConfiguration BuildConfig(bool includeSection = true)
    {
        var db = Path.Combine(Path.GetTempPath(), $"bcast-tg-{Guid.NewGuid():N}.db");
        var json = includeSection
            ? $$"""
              {
                "Broadcasting": {
                  "BotId": "bot-tg",
                  "ServerUri": "http://localhost",
                  "ConnectionString": "{{db.Replace("\\", "\\\\")}}"
                }
              }
              """
            : "{}";

        var path = Path.Combine(Path.GetTempPath(), $"bcast-tg-settings-{Guid.NewGuid():N}.json");
        File.WriteAllText(path, json);
        return new ConfigurationBuilder().AddJsonFile(path).Build();
    }

    [Test]
    public void AddBroadcasting_WhenSettingsMissing_Should_Throw()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var builder = new ConcreteTelegramBotBuilder(services);

        var act = () => builder.AddBroadcasting(BuildConfig(includeSection: false));

        act.Should().Throw<ConfigurationErrorsException>();
    }

    [Test]
    public void AddBroadcasting_Should_RegisterAndReturnBuilder()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IBot>(new Mock<IBot>().Object);

        var builder = new ConcreteTelegramBotBuilder(services);
        var result = builder.AddBroadcasting(BuildConfig());

        result.Should().BeSameAs(builder);
        services.Should().Contain(d => d.ServiceType.Name.Contains("BroadcastingContext")
                                      || d.ServiceType == typeof(Botticelli.Broadcasting.Dal.BroadcastingContext));
    }

    [Test]
    public void AddTelegramBroadcasting_Should_RegisterAndReturnBuilder()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IBot>(new Mock<IBot>().Object);

        // Cast inside AddTelegramBroadcasting requires TBotBuilder == TelegramBotBuilder<TelegramBot>
        var builder = new TelegramBotBuilder<TelegramBot, TelegramBotBuilder<TelegramBot>>(true);
        builder.AddServices(services);

        var result = builder.AddTelegramBroadcasting(BuildConfig());

        result.Should().BeSameAs(builder);
        services.Should().Contain(d => d.ServiceType == typeof(Botticelli.Broadcasting.Dal.BroadcastingContext));
    }
}
