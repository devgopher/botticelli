using System.Configuration;
using Botticelli.Broadcasting.Dal;
using Botticelli.Broadcasting.Dal.Models;
using Botticelli.Broadcasting.Extensions;
using Botticelli.Broadcasting.Tests.Helpers;
using Botticelli.Framework.Events;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Botticelli.Broadcasting.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    private static IConfiguration BuildConfig(string? connectionString = null, bool includeSection = true)
    {
        var db = connectionString ?? Path.Combine(Path.GetTempPath(), $"bcast-cfg-{Guid.NewGuid():N}.db");
        var json = includeSection
            ? $$"""
              {
                "Broadcasting": {
                  "BotId": "bot-1",
                  "ServerUri": "http://localhost",
                  "ConnectionString": "{{db.Replace("\\", "\\\\")}}"
                }
              }
              """
            : "{}";

        var path = Path.Combine(Path.GetTempPath(), $"bcast-settings-{Guid.NewGuid():N}.json");
        File.WriteAllText(path, json);
        return new ConfigurationBuilder().AddJsonFile(path).Build();
    }

    [Test]
    public void AddBroadcasting_WhenSettingsMissing_Should_Throw()
    {
        var services = new ServiceCollection();
        var builder = new TestBotBuilder(services);

        var act = () => builder.AddBroadcasting(BuildConfig(includeSection: false));

        act.Should().Throw<ConfigurationErrorsException>()
           .WithMessage("*Broadcasting settings*");
    }

    [Test]
    public void AddBroadcasting_Should_RegisterServicesAndHandler()
    {
        var db = Path.Combine(Path.GetTempPath(), $"bcast-ext-{Guid.NewGuid():N}.db");
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IBot>(new Mock<IBot>().Object);
        services.AddSingleton(new Mock<ILogger<BroadcastReceiver<TestBroadcastBot>>>().Object);

        var builder = new TestBotBuilder(services);
        var result = builder.AddBroadcasting(BuildConfig(db));

        result.Should().BeSameAs(builder);

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<BroadcastingContext>().Should().NotBeNull();
        provider.GetServices<Microsoft.Extensions.Hosting.IHostedService>()
                .OfType<BroadcastReceiver<TestBroadcastBot>>()
                .Should().ContainSingle();
    }

    [Test]
    public void AddBroadcasting_OnMessageReceived_Should_ActivateAndAddChats()
    {
        var db = Path.Combine(Path.GetTempPath(), $"bcast-msg-{Guid.NewGuid():N}.db");
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IBot>(new Mock<IBot>().Object);
        services.AddSingleton(new Mock<ILogger<BroadcastReceiver<TestBroadcastBot>>>().Object);

        var builder = new TestBotBuilder(services);
        builder.AddBroadcasting(BuildConfig(db));

        // Seed inactive chat before handler runs
        using (var seed = services.BuildServiceProvider().GetRequiredService<BroadcastingContext>())
        {
            seed.Chats.Add(new Chat { ChatId = "existing-inactive", IsActive = false });
            seed.SaveChanges();
        }

        builder.RaiseMessageReceived(new object(), new MessageReceivedBotEventArgs
        {
            Message = new Message("m1")
            {
                ChatIds = ["existing-inactive", "brand-new"]
            }
        });

        using var verify = new BroadcastingContext(new DbContextOptionsBuilder<BroadcastingContext>()
            .UseSqlite($"Data Source={db}")
            .Options);

        var chats = verify.Chats.ToList();
        chats.Should().Contain(c => c.ChatId == "existing-inactive" && c.IsActive);
        chats.Should().Contain(c => c.ChatId == "brand-new" && c.IsActive);
    }
}
