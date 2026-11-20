using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Rules;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Auth.Tests.Rules;

[TestFixture]
public class CredentialsRulesTests
{
    [Test]
    public async Task Compare_Should_Pass_WhenAllRulesSucceed()
    {
        var rules = new CredentialsRulesBuilder<BotAuthCredentials>()
            .AddRule(c => !string.IsNullOrWhiteSpace(c.UserId))
            .AddRule(c => c.UserId.Length > 2)
            .Build();

        (await rules.Compare(new BotAuthCredentials { UserId = "user-1" })).Should().BeTrue();
    }

    [Test]
    public async Task Compare_Should_Fail_WhenAnyRuleFails()
    {
        var rules = new CredentialsRulesBuilder<BotAuthCredentials>()
            .AddRule(_ => true)
            .AddRule(_ => false)
            .Build();

        (await rules.Compare(new BotAuthCredentials { UserId = "x" })).Should().BeFalse();
    }

    [Test]
    public void Builder_Should_ReturnSameInstance_OnAddRule()
    {
        var builder = new CredentialsRulesBuilder<BotAuthCredentials>();
        builder.AddRule(_ => true).Should().BeSameAs(builder);
    }
}
