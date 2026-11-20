using System.ComponentModel.DataAnnotations;
using Botticelli.Server.FrontNew.Models;
using Botticelli.Server.FrontNew.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.FrontNew.Tests.Models;

[TestFixture]
public class FrontModelsTests
{
    [Test]
    public void Error_Session_Metrics_RegisterUser_Should_HoldValues()
    {
        var error = new Error { Code = 1, UserMessage = "fail" };
        var session = new Session { SessionId = "s", Login = "u", Token = "t" };
        var metrics = new MetricsModel { DateTime = DateTime.UtcNow, Value = 1.5m };
        var register = new RegisterUser { Email = "a@b.c", Password = "p" };

        error.Code.Should().Be(1);
        session.Token.Should().Be("t");
        metrics.Value.Should().Be(1.5m);
        register.Email.Should().Be("a@b.c");
    }

    [Test]
    public void RegisterUser_Should_RequireValidEmail()
    {
        var invalid = new RegisterUser { Email = "bad", Password = "p" };
        var results = new List<ValidationResult>();
        var ok = Validator.TryValidateObject(invalid, new ValidationContext(invalid), results, true);

        ok.Should().BeFalse();
        results.Should().NotBeEmpty();
    }

    [Test]
    public void BotModels_Should_HoldValues()
    {
        var bot = new BotModel
        {
            BotId = "1",
            BotKey = "k",
            BotName = "n",
            BotType = "Telegram"
        };
        var broadcast = new BotBroadcastModel
        {
            BotId = "1",
            BotName = "n",
            Message = "hi"
        };

        bot.BotType.Should().Be("Telegram");
        broadcast.Message.Should().Be("hi");
    }

    [Test]
    public void BackSettings_Should_HoldUrls()
    {
        var settings = new BackSettings
        {
            BackUrl = "https://back",
            AnalyticsUrl = "https://analytics"
        };

        settings.BackUrl.Should().Be("https://back");
        settings.AnalyticsUrl.Should().Be("https://analytics");
    }
}
