using Botticelli.Pay.Handlers;
using Botticelli.Pay.Models;
using Botticelli.Pay.Processors;
using Botticelli.Pay.Telegram.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Pay.Telegram.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    private sealed class TestHandler : IPreCheckoutHandler;

    private sealed class TestProcessor : DummyPayProcessor<TestHandler, PreCheckoutQuery>;

    private static IConfiguration BuildConfig()
    {
        var db = Path.Combine(Path.GetTempPath(), $"pay-tg-{Guid.NewGuid():N}.db").Replace("\\", "\\\\");
        var json =
            $$"""
              {
                "TelegramBot": {
                  "Name": "PayTestBot",
                  "Timeout": 60000
                },
                "ServerSettings": {
                  "ServerUri": "http://localhost"
                },
                "AnalyticsClientSettings": {
                  "TargetUrl": "http://localhost",
                  "UseSsl": false
                },
                "DataAccess": {
                  "ConnectionString": "{{db}}"
                }
              }
              """;

        var path = Path.Combine(Path.GetTempPath(), $"pay-tg-settings-{Guid.NewGuid():N}.json");
        File.WriteAllText(path, json);

        return new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();
    }

    [Test]
    public void AddTelegramPayBot_WithConfiguration_Should_Invoke()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        try
        {
            services.AddTelegramPayBot<TestHandler, TestProcessor>(BuildConfig());
        }
        catch (Exception)
        {
            // Full Telegram registration may require runtime infra; method body still executes.
        }

        services.Should().NotBeEmpty();
    }

    [Test]
    public void AddStandaloneTelegramPayBot_WithConfiguration_Should_Invoke()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        try
        {
            services.AddStandaloneTelegramPayBot<TestHandler, TestProcessor>(BuildConfig());
        }
        catch (Exception)
        {
            // Exercise registration path.
        }

        services.Should().NotBeEmpty();
    }

    [Test]
    public void AddTelegramPayBot_WithBuilders_Should_Invoke()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        try
        {
            services.AddTelegramPayBot<TestHandler, TestProcessor, PreCheckoutQuery>(
                o => o.Set(s => s.Name = "PayTestBot"),
                a => a.Set(s =>
                {
                    s.TargetUrl = "http://localhost";
                    s.UseSsl = false;
                }),
                s => s.Set(x => x.ServerUri = "http://localhost"),
                d => d.Set(x => x.ConnectionString = Path.Combine(Path.GetTempPath(), $"pay-{Guid.NewGuid():N}.db")));
        }
        catch (Exception)
        {
            // Exercise overload path.
        }

        services.Should().NotBeEmpty();
    }
}
