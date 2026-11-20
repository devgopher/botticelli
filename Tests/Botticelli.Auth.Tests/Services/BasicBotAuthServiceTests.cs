using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Mapping;
using Botticelli.Auth.Rules;
using Botticelli.Auth.Services;
using Botticelli.Auth.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Auth.Tests.Services;

[TestFixture]
public class BasicBotAuthServiceTests
{
    [SetUp]
    public void SetUp() => MappingProfile.UseProfile();

    [Test]
    public async Task Identify_Should_Succeed_ForActiveUser()
    {
        await using var ctx = AuthDbFactory.Create();
        await AuthDbFactory.SeedUserAsync(ctx, "alice");
        var service = new BasicBotAuthService(new CredentialsRulesBuilder<BotAuthCredentials>(), ctx);

        var result = await service.Identify(new BotAuthCredentials { UserId = "alice" });

        result.Success.Should().BeTrue();
        result.User!.UserId.Should().Be("alice");
        result.ErrorMessage.Should().BeEmpty();
    }

    [Test]
    public async Task Identify_Should_Fail_ForUnknownOrInactiveUser()
    {
        await using var ctx = AuthDbFactory.Create();
        await AuthDbFactory.SeedUserAsync(ctx, "bob", isActive: false);
        var service = new BasicBotAuthService(new CredentialsRulesBuilder<BotAuthCredentials>(), ctx);

        var result = await service.Identify(new BotAuthCredentials { UserId = "bob" });

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Invalid credentials");
    }

    [Test]
    public async Task Identify_Should_Fail_WhenUserMissingAfterRulePass()
    {
        await using var ctx = AuthDbFactory.Create();
        await AuthDbFactory.SeedUserAsync(ctx, "carol");
        // Custom builder without DB rule — always true, then DoLogin looks up missing user
        var builder = new CredentialsRulesBuilder<BotAuthCredentials>().AddRule(_ => true);
        var service = new BasicBotAuthService(builder, ctx);

        var result = await service.Identify(new BotAuthCredentials { UserId = "nobody" });

        // BasicBotAuthService ctor also adds DB rule; nobody fails that rule
        result.Success.Should().BeFalse();
    }
}
