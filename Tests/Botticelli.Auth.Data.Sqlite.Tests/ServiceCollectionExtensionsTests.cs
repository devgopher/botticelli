using Botticelli.Auth.Data;
using Botticelli.Auth.Data.Sqlite;
using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Auth.Data.Sqlite.Tests;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddSqliteBasicBotUserAuth_Should_RegisterAuthAndDbContext()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration("TestData/auth.settings.json");

        var result = services.AddSqliteBasicBotUserAuth(configuration);

        result.Should().BeSameAs(services);

        services.Should().Contain(d => d.ServiceType == typeof(AuthDefaultDbContext));
        services.Should().Contain(d => d.ServiceType == typeof(IIdentifier<BotAuthCredentials, BotUserInfo>));
        services.Should().Contain(d => d.ServiceType == typeof(IManager<BotUserInfo>));
        services.Should().Contain(d => d.ServiceType == typeof(IManager<BotUserRoleInfo>));

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<DbContextOptions<AuthDefaultDbContext>>();
        options.Extensions.Should().Contain(e => e.GetType().Name.Contains("Sqlite", StringComparison.Ordinal));
    }

    [Test]
    public void AddSqliteBasicBotUserAuth_WithDbParameters_Should_InvokeCallback()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration("TestData/auth.settings.json");
        var invoked = false;

        services.AddSqliteBasicBotUserAuth(configuration, _ => invoked = true);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<DbContextOptions<AuthDefaultDbContext>>();
        options.Should().NotBeNull();
        invoked.Should().BeTrue();
    }

    [Test]
    public void AddSqliteBasicBotUserAuth_WithMissingSettings_Should_ThrowWhenBuildingOptions()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddSqliteBasicBotUserAuth(configuration);

        using var provider = services.BuildServiceProvider();
        var act = () => provider.GetRequiredService<DbContextOptions<AuthDefaultDbContext>>();

        act.Should().Throw<NullReferenceException>();
    }

    private static IConfiguration BuildConfiguration(string relativePath) =>
        new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory)
            .AddJsonFile(relativePath)
            .Build();
}
