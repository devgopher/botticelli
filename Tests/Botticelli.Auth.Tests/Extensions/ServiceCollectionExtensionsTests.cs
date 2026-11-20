using Botticelli.Auth.Data;
using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Extensions;
using Botticelli.Auth.Rules;
using Botticelli.Auth.Services;
using Botticelli.Auth.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Botticelli.Auth.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddBasicBotUserAuth_Should_RegisterServices()
    {
        var services = new ServiceCollection();
        var ctx = AuthDbFactory.Create();
        services.AddSingleton(ctx);
        services.AddBasicBotUserAuth();

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IIdentifier<BotAuthCredentials, BotUserInfo>>().Should().BeOfType<BasicBotAuthService>();
        provider.GetRequiredService<IManager<BotUserInfo>>().Should().BeOfType<DefaultUserManager>();
        provider.GetRequiredService<IManager<BotUserRoleInfo>>().Should().BeOfType<DefaultUserRoleManager>();
        provider.GetRequiredService<CredentialsRulesBuilder<BotAuthCredentials>>().Should().NotBeNull();
    }
}
