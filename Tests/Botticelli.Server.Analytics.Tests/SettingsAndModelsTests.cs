using System.ComponentModel;
using Botticelli.Server.Analytics.Exceptions;
using Botticelli.Server.Analytics.Models;
using Botticelli.Server.Analytics.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Server.Analytics.Tests;

[TestFixture]
public class SettingsAndModelsTests
{
    [Test]
    public void AnalyticsServerSettings_Should_HoldValues()
    {
        var settings = new AnalyticsServerSettings
        {
            ConnectionString = "cs",
            MaxCacheSize = 42
        };

        settings.ConnectionString.Should().Be("cs");
        settings.MaxCacheSize.Should().Be(42);
    }

    [Test]
    public void MetricModel_Should_ExposePropertiesAndEvents()
    {
        PropertyChangedEventHandler? changed = null;
        PropertyChangingEventHandler? changing = null;
        var model = new MetricModel
        {
            Id = "id",
            BotId = "bot",
            Name = "n",
            Timestamp = DateTime.UtcNow,
            InternalValue = "v",
            AdditionalParameters = new Dictionary<string, string> { ["a"] = "b" }
        };
        model.PropertyChanged += changed;
        model.PropertyChanging += changing;

        model.Id.Should().Be("id");
        model.AdditionalParameters["a"].Should().Be("b");
        ((IMetricModel)model).InternalValue.Should().Be("v");
    }

    [Test]
    public void AnalyticsException_Should_PreserveInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new AnalyticsException("outer", inner);

        ex.Message.Should().Be("outer");
        ex.InnerException.Should().BeSameAs(inner);
    }
}
